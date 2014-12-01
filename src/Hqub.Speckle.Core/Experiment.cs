using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Hqub.Speckle.Core.Annotations;
using Hqub.Speckle.Core.Model;

namespace Hqub.Speckle.Core
{
    using System.Drawing;

    public class Experiment : IExperiment, INotifyPropertyChanged
    {
        private ObservableCollection<ImageWrapper> _images;

        private Experiment()
        {
            StartExperiment = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            Period = 10;
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

        /// <summary>
        /// Область сравнения изображений
        /// </summary>
        public Rectangle WorkAreay { get; set; }

        public DateTime StartExperiment { get; set; }
        public int Period { get; set; }

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
