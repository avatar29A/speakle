using System.Windows;

namespace Hqub.Speckle.GUI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        protected override void LoadCommandExecute(RoutedEventArgs args)
        {
            ManagerModule.LoadModule(typeof(Modules.ShellModule));
        }
    }
}
