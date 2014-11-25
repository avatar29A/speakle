using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Hqub.Speckle.GUI.ViewModel
{
    public class BaseViewModel : Microsoft.Practices.Prism.Mvvm.BindableBase
    {
        public ICommand LoadCommand
        {
            get { return new DelegateCommand<RoutedEventArgs>(LoadCommandExecute); }
        }

        protected virtual void LoadCommandExecute(RoutedEventArgs args)
        {

        }

        public ICommand ClosingCommand { get { return new DelegateCommand<CancelEventArgs>(ClosingCommandExecute); } }

        protected virtual void ClosingCommandExecute(CancelEventArgs args)
        {

        }
    }
}
