using System.Windows;

namespace Hqub.Speckle.GUI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Modules.ShellModule _shellModule;

        protected override void LoadCommandExecute(RoutedEventArgs args)
        {
            ManagerModule.LoadModule(typeof(Modules.ShellModule));
        }
    }
}
