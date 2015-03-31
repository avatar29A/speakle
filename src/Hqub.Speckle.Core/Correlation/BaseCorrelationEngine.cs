using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Speckle.Core.BitmapExtensions;

namespace Hqub.Speckle.Core.Correlation
{
    public abstract class BaseCorrelationEngine : ICorrelationEngine
    {
        public abstract double Compare(string pathA, string pathB);

        public abstract double Compare(string pathA, string pathB, Rectangle bound);

        public abstract double Compare(Bitmap imageA, Bitmap imageB);

        public ILogger Logger { get; set; }

        /// <summary>
        /// Раскалдывает картинку в одномерный массив
        /// </summary>
        /// <param name="source">Картинка</param>
        /// <returns>Массив пикселей</returns>
        public int[] ScaffBitmap(Bitmap source)
        {
            var lockSource = new LockBitmap(source);
            lockSource.LockBits();

            var m = new int[lockSource.Width * lockSource.Height];
            var counter = 0;

            for (var i = 0; i < lockSource.Width; ++i)
            {
                for (var j = 0; j < lockSource.Height; ++j)
                {
                    m[counter] = lockSource.GetPixel(i, j).R;
                    ++counter;
                }
            }

            lockSource.UnlockBits();

            return m;
        }
    }
}
