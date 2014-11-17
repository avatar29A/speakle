using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Hqub.Speckle.GUI.ViewModel
{
    public class BaseViewModel : Microsoft.Practices.Prism.ViewModel.NotificationObject
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
