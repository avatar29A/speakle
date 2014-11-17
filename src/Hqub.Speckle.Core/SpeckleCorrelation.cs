using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.Core
{
    public class SpeckleCorrelation
    {
        private double LineInterp(double dbX1, double dbX2, double dbY1, double dbY2, double dbX)
        {
            return (dbY1 + ((dbY2 - dbY1)/(dbX2 - dbX1))*(dbX - dbX1));
        }
    }
}
