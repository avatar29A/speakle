using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Speckle.GUI.Modules;
using Microsoft.Practices.ServiceLocation;

namespace Hqub.Speckle.GUI
{
    public static class ManagerModule
    {
        public static BaseNavigationModule LoadModule(Type module)
        {
            var m = (BaseNavigationModule)ServiceLocator.Current.GetInstance(module);
            m.Show();

            return m;
        }
    }
}
