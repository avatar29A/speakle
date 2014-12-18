using System.Windows;

namespace Hqub.Speckle.GUI.Dialogs
{
    using System.Windows.Input;

    using Telerik.Windows.Controls;

    /// <summary>
    /// Interaction logic for WorkAreaSettingsDialog.xaml
    /// </summary>
    public partial class WorkAreaSettingsDialog : Window
    {
        public WorkAreaSettingsDialog()
        {
            InitializeComponent();
        }

        public WorkAreaSettingsDialog(Rect workarea)
        {
            InitializeComponent();

            WorkAreaSettings.Init(workarea);
            
        }

        #region Commands

        public ICommand SaveCommand
        {
            get
            {
                return new DelegateCommand(this.SaveCommandExecute);
            }
        }

        private void SaveCommandExecute(object obj)
        {
            DialogResult = true;
            this.Close();
        }


        public ICommand CloseCommand
        {
            get
            {
                return new DelegateCommand(CancelCommandExecute);
            }
        }

        private void CancelCommandExecute(object obj)
        {
            DialogResult = false;
            this.Close();
        }

        #endregion

        #region Propeties

        public int X
        {
            get
            {
                return WorkAreaSettings.X;
            }
        }

        public int Y
        {
            get
            {
                return WorkAreaSettings.Y;
            }
        }

        public int AreaWidth
        {
            get
            {
                return WorkAreaSettings.AreaWidth;
            }
        }

        public int AreaHeight
        {
            get
            {
                return WorkAreaSettings.AreaHeight;
            }
        }

        #endregion
    }
}
