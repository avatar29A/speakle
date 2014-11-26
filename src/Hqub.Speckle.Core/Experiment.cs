using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Hqub.Speckle.Core.Annotations;
using Hqub.Speckle.Core.Model;

namespace Hqub.Speckle.Core
{
    public class Experiment : IExperiment, INotifyPropertyChanged
    {
        private ObservableCollection<ImageWrapper> _images;

        private Experiment()
        {
            
        }

        #region Public Properties

        public ObservableCollection<ImageWrapper> Images
        {
            get { return _images; }
            set
            {
                if (Equals(value, _images)) return;
                _images = value;
                OnPropertyChanged();
                OnPropertyChanged("ImageCount");
            }
        }

        public int ImageCount
        {
            get
            {
                if (_images == null)
                    return 100;

                return _images.Count;
            }
        }

        #endregion

        #region Static Interfaces

        private static Experiment _instance;
        private static Experiment Create()
        {
            return new Experiment();
        }

        public static Experiment Get(bool isForseCreate=false)
        {
            if (isForseCreate)
                return _instance = Create();

            return _instance = _instance ?? Create();
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IExperiment
    {
    }
}
