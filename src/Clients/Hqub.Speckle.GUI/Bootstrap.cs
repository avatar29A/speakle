using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hqub.Speckle.GUI.View;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace Hqub.Speckle.GUI
{
    public class Bootstrap : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var main = Container.Resolve<View.MainWindowView>();

            main.Show();

            return main;
        }

        protected override void ConfigureModuleCatalog()
        {
//            ModuleCatalog.AddModule(new ModuleInfo
//            {
//                ModuleName = "AuthModule",
//                ModuleType =
//                    typeof(Modules.AuthModule)
//                    .AssemblyQualifiedName,
//                InitializationMode =
//                    InitializationMode.OnDemand
//            });
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterInstance(Container);

            // View
            Container.RegisterType(typeof(View.MainWindowView));

            // ViewModel
            Container.RegisterType(typeof (ViewModel.MainViewModel));

            base.ConfigureContainer();
        }
    }
}
