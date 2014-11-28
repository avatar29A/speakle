using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.Core.Correlation
{
    public class PHashCorrelationEngine : ICorrelationEngine
    {
        [DllImport("pHash.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ph_compare_images(string file1, string file2, out double pcc, double sigma = 3.5,
            double gamma = 1.0, int N = 180, double threshold = 0.90);

        public void AddImage()
        {
            throw new NotImplementedException();
        }

        public double Compare(string pathA, string pathB)
        {
            double pcc = 0.0;

            try
            {
                ph_compare_images(pathA, pathB, out pcc);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return pcc;
        }

        public double Compare(string pathA, string pathB, Rectangle bound)
        {
            return Compare(pathA, pathB);
        }

        public double Compare(Bitmap imageA, Bitmap imageB)
        {
            throw new NotImplementedException(
                "Алгоритм требует передавать ему пути до изображений. Воспользуйте другой перегрузкой метода Compare (string->string->double)");
        }

        public ILogger Logger { get; set; }
    }
}
