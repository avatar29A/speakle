using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.GUI
{
    public static class Logger
    {
        public static NLog.Logger Main
        {
            get { return NLog.LogManager.GetLogger("Main"); }
        }
    }
}
