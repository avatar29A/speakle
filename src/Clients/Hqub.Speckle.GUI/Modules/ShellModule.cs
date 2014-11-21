using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.GUI.Modules
{
    public class ShellModule : BaseNavigationModule
    {
        public ShellModule(View.Shell.ShellView view)
        {
            RegisterView(RegionNames.MainRegionName, view);
        }

        public override string Name
        {
            get { return "ShellModule"; }
        }
    }
}
