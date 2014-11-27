using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hqub.Speckle.Core.Model
{
    using System.Text.RegularExpressions;

    public class ImageWrapper : INotifyPropertyChanged
    {
        private string _name;
        private bool _isChecked = true;
        private bool _isProcessed;

        public ImageWrapper(string filename)
        {
            Name = System.IO.Path.GetFileName(filename);
            Path = filename;
        }

        public ImageWrapper()
        {
            
        }


        public CorrelationValue Correlation { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Path { get; set; }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsProcessed
        {
            get { return _isProcessed; }
            set
            {
                _isProcessed = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
