using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hqub.Speckle.GUI
{
    using System.Windows.Navigation;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Bootstrap _bootstrap = new Bootstrap();

        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;

            Exit += App_Exit;

            DispatcherUnhandledException +=
                (sender, dispatcherUnhandled) =>
                {
                    Logger.Main.Error("Возникло не обработанное исключение в Dispatcher.",
                                               dispatcherUnhandled.Exception);

                    dispatcherUnhandled.Handled = true;
                };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Logger.Main.Trace("Запуск приложения {0}", typeof(App).Assembly.GetName());

            _bootstrap.Run(true);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Logger.Main.Trace("Закрытие приложения {0}", typeof(App).Assembly.GetName());
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var message = string.Format("Ошибка в приложении {0}", typeof(App).Assembly.GetName());
            Logger.Main.Error(message, e.Exception);
            e.Handled = true;
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var message = string.Format("Ошибка в приложении {0}", typeof(App).Assembly.GetName());
            Logger.Main.Error(message, e.Exception);

            e.Handled = true;
        }
    }
}
