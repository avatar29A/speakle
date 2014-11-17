using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Speckle.GUI.Utitlities;

namespace Hqub.Speckle.GUI.Modules
{
    public class BaseModule
    {
        Dictionary<string, object> region2View =
            new Dictionary<string, object>();

        public virtual void Show()
        {
            foreach (var kv in region2View)
            {
                RegionManagerUtils.AddToRegion(kv.Key, kv.Value);
            }
        }

        public virtual void Hide()
        {
            foreach (var kv in region2View)
            {
                RegionManagerUtils.ClearRegion(kv.Key);
            }
        }

        protected void RegisterView(string regionName, object view)
        {
            region2View.Add(regionName, view);
        }
    }
}
