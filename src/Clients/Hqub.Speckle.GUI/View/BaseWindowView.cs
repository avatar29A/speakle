using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hqub.Speckle.GUI.ViewModel;

namespace Hqub.Speckle.GUI.View
{
    public class BaseWindowView : Window
    {
        public BaseWindowView(BaseViewModel viewModel)
        {
            ViewModel = viewModel;

            Loaded += (loadedSender, loadedArgs) => viewModel.LoadCommand.Execute(loadedArgs);
            Closing += (closingSender, closingArgs) => viewModel.ClosingCommand.Execute(closingArgs);

            KeyDown += HandleKeyDown;

            //Если диалог, то выставим ему метод для закрытия
            if (viewModel is BaseDialogViewModel)
            {
                ((BaseDialogViewModel)viewModel).SetCloseAction(Close);
            }
        }

        protected virtual void HandleKeyDown(object sender, KeyEventArgs e)
        {
            //            if (e.Key == Key.Escape)
            //                Close();
        }

        public BaseViewModel ViewModel
        {
            get { return (BaseViewModel)DataContext; }
            set { DataContext = value; }
        }

        protected virtual void SetupRegions()
        {

        }
    }
}
