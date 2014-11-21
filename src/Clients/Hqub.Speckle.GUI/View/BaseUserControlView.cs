using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Hqub.Speckle.GUI.ViewModel;

namespace Hqub.Speckle.GUI.View
{
    public class BaseUserControlView : UserControl
    {
        public BaseUserControlView(BaseViewModel viewModel)
        {
            ViewModel = viewModel;

            Loaded += (loadedSender, loadedArgs) => viewModel.LoadCommand.Execute(null);
        }

        public BaseViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}
