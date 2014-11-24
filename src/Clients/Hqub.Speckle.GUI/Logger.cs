using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hqub.Speckle.GUI
{
    public class Logger : Core.ILogger
    {
        public static NLog.Logger Main
        {
            get { return NLog.LogManager.GetLogger("Main"); }
        }

        public void Error(string message,Exception exception = null)
        {
            Main.Error(message);
        }

        public void Warning(string message)
        {
            Main.Warn(message);
        }

        public void Fatal(string message, Exception exception)
        {
            Main.Fatal(message, exception);
        }
    }
}
