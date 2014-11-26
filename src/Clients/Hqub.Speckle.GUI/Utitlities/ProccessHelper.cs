using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Hqub.Speckle.GUI.Utitlities
{
    public static class ProccessHelper
    {
        public static void OpenImage(string filename)
        {
            Process.Start(filename);
        }
    }
}
