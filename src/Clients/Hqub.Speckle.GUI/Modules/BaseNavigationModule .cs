using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.GUI.Modules
{
    public abstract class BaseNavigationModule : BaseModule
    {
        public abstract string Name { get; }

        public virtual void BeforeBack() { }

        public virtual void BeforeNext() { }
    }
}
