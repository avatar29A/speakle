using System.Windows.Controls;

namespace Hqub.Speckle.GUI.Controls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    using JetBrains.Annotations;

    using Telerik.Windows.Controls;

    /// <summary>
    /// Interaction logic for WorkareaSettings.xaml
    /// </summary>
    public partial class WorkareaSettings : UserControl, INotifyPropertyChanged
    {
        private int areaWidth;

        private int y;

        private int x;

        private int areaHeight;

        public WorkareaSettings()
        {
            InitializeComponent();
        }

        public void Init(Rect workarea)
        {
            X = (int)workarea.X;
            Y = (int)workarea.Y;
            AreaWidth = (int)workarea.Width;
            AreaHeight = (int)workarea.Height;
        }


        #region Properties

        public int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
                this.OnPropertyChanged();
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
                this.OnPropertyChanged();
            }
        }

        public int AreaWidth
        {
            get
            {
                return this.areaWidth;
            }
            set
            {
                this.areaWidth = value;
                this.OnPropertyChanged();
            }
        }

        public int AreaHeight
        {
            get
            {
                return this.areaHeight;
            }
            set
            {
                this.areaHeight = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
