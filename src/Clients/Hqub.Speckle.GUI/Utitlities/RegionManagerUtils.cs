using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Hqub.Speckle.GUI.Utitlities
{
    public static class RegionManagerUtils
    {
        public static void ClearRegion(string regionName)
        {
            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            foreach (var view in regionManager.Regions[regionName].ActiveViews.ToArray())
            {
                regionManager.Regions[regionName].Remove(view);
            }
        }

        public static void AddToRegion(string regionName, object view)
        {
            ClearRegion(regionName);

            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();

            regionManager.AddToRegion(regionName, view);
        }
    }
}
