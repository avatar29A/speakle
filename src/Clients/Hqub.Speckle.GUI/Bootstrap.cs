// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrap.cs" company="HQUB">
//   Glebov Boris
// </copyright>
// <summary>
//   Defines the Bootstrap type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Hqub.Speckle.GUI
{
    using System.Windows;

    using Hqub.Speckle.GUI.View;

    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.Prism.UnityExtensions;
    using Microsoft.Practices.Unity;

    public class Bootstrap : UnityBootstrapper
    {
        /// <summary>
        /// The create shell.
        /// </summary>
        /// <returns>
        /// The <see cref="DependencyObject"/>.
        /// </returns>
        protected override DependencyObject CreateShell()
        {
            var main = Container.Resolve<MainWindowView>();

            main.Show();

            return main;
        }

        /// <summary>
        /// The configure module catalog.
        /// </summary>
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

        /// <summary>
        /// The configure container.
        /// </summary>
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

        /// <summary>
        /// The configure view container.
        /// </summary>
        private void ConfigureViewContainer()
        {
            Container.RegisterType(typeof(MainWindowView));
            Container.RegisterType(typeof(View.Shell.ShellView));
        }

        /// <summary>
        /// The configure view model container.
        /// </summary>
        private void ConfigureViewModelContainer()
        {
            Container.RegisterType(typeof(ViewModel.MainViewModel));
            Container.RegisterType(typeof(ViewModel.Shell.ShellViewModel));
        }

        /// <summary>
        /// The configure other types.
        /// </summary>
        private void ConfigureOtherTypes()
        {
            // Logger
            Container.RegisterType(typeof(Core.ILogger), typeof(Logger));

            // Correlation Engines
            Container.RegisterType(typeof(Core.Correlation.PHashCorrelationEngine), new InjectionProperty("Logger"));
            Container.RegisterType(typeof(Core.Correlation.SpegoCorrelationEngine), new InjectionProperty("Logger"));

            Container.RegisterInstance(typeof(IEventAggregator), Events.AggregationEventService.Instance);
        }
    }
}