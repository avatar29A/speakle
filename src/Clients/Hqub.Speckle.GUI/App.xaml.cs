using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hqub.Speckle.GUI
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Bootstrap _bootstrap = new Bootstrap();

        public App()
        {
            DispatcherUnhandledException +=
                (sender, dispatcherUnhandled) =>
                {
                    Logger.Main.Error("Возникло не обработанное исключение в Dispatcher.",
                                               dispatcherUnhandled.Exception);

                    dispatcherUnhandled.Handled = true;
                };

            _bootstrap.Run(true);
        }
    }
}
