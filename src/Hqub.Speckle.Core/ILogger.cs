using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.Core
{
    public interface ILogger
    {
        void Error(string message, Exception exception);
        void Fatal(string message, Exception exception);

        void Warning(string message);
    }
}
