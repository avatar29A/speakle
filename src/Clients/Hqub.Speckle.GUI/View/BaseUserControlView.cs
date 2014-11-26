using System.ComponentModel;
using System.Windows.Controls;
using Hqub.Speckle.GUI.ViewModel;

namespace Hqub.Speckle.GUI.View
{
    public class BaseUserControlView : UserControl
    {
        public BaseUserControlView(BaseViewModel viewModel)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
                // Design-mode specific functionality
            }

            ViewModel = viewModel;

            Loaded += (loadedSender, loadedArgs) => viewModel.LoadCommand.Execute(null);
        }

        public BaseViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}
