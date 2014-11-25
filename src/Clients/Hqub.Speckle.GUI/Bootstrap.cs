using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Hqub.Speckle.GUI.View;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
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
            ModuleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = "ShellModule",
                ModuleType =
                    typeof(Modules.ShellModule)
                    .AssemblyQualifiedName,
                InitializationMode =
                    InitializationMode.OnDemand
            });
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterInstance(Container);

            // View
            ConfigureViewContainer();

            // ViewModel
            ConfigureViewModelContainer();

            // Other
            ConfigureOtherTypes();

            base.ConfigureContainer();
        }

        private void ConfigureModuleContainer()
        {
            Container.RegisterType(typeof (Modules.ShellModule));
        }

        private void ConfigureViewContainer()
        {
            Container.RegisterType(typeof(MainWindowView));
            Container.RegisterType(typeof (View.Shell.ShellView));
        }

        private void ConfigureViewModelContainer()
        {
            Container.RegisterType(typeof(ViewModel.MainViewModel));
            Container.RegisterType(typeof(ViewModel.Shell.ShellViewModel));
        }

        private void ConfigureOtherTypes()
        {
            // Logger
            Container.RegisterType(typeof (Core.ILogger), typeof (Logger));

            // Correlation Engines
            Container.RegisterType(typeof (Core.Correlation.PHashCorrelationEngine), new InjectionProperty("Logger"));
            Container.RegisterType(typeof (Core.Correlation.SpegoCorrelationEngine), new InjectionProperty("Logger"));

            Container.RegisterInstance(typeof (IEventAggregator), Events.AggregationEventService.Instance);
        }
    }
}
