using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using Hqub.Speckle.Core;

namespace Hqub.Speckle.GUI.Experiment
{
    public class SpeckleExperiment : IExperiment, INotifyPropertyChanged
    {
        #region Methods


        #endregion

        #region Properties

        public ICorrelationEngine CurrentEngine { get; set; }
        public string CurrentEngineName { get; set; }

        public List<Bitmap> Images { get; set; } 

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
