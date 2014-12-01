using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hqub.Speckle.Core.Model
{
    using System.Text.RegularExpressions;

    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;

    public class ImageWrapper : INotifyPropertyChanged
    {
        private string name;
        private bool isChecked = true;
        private bool isProcessed;

        private IEventAggregator _eventAggregator;

        public ImageWrapper(string filename)
        {
            Name = System.IO.Path.GetFileName(filename);
            Path = filename;

            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        }

        public ImageWrapper()
        {
            
        }

        public CorrelationValue Correlation { get; set; }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                OnPropertyChanged();
            }
        }

        public string Path { get; set; }

        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;

                _eventAggregator.GetEvent<Events.ExperimentPhotoSeletChangedEvent>().Publish(this);
                OnPropertyChanged();
            }
        }

        public bool IsProcessed
        {
            get { return this.isProcessed; }
            set
            {
                this.isProcessed = value;
                OnPropertyChanged();
            }
        }

        public int Number
        {
            get
            {
                var regValue = Regex.Match(Name, "[0-9]{6}");

                var val = 0;
                if (string.IsNullOrEmpty(regValue.Value) || int.TryParse(regValue.Value, out val))
                {
                    return val;
                }

                return val;
            }
        }

        #region Implementation INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
