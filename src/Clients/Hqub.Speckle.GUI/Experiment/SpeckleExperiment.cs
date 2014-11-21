using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Hqub.Speckle.GUI.Experiment
{
    public class SpeckleExperiment : IExperiment, INotifyPropertyChanged
    {
        #region Methods


        #endregion

        #region Properties

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
