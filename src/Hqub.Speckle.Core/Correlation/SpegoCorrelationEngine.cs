using System;
using System.Drawing;
using System.Linq;
using Hqub.Speckle.Core.BitmapExtensions;

namespace Hqub.Speckle.Core.Correlation
{
    public class SpegoCorrelationEngine : ICorrelationEngine
    {
        public void AddImage()
        {
            throw new NotImplementedException();
        }

        public double Compare(string pathA, string pathB)
        {
            var image1 = new Bitmap(pathA);
            var image2 = new Bitmap(pathB);

            return Compare(image1, image2);
        }

        public double Compare(Bitmap imageA, Bitmap imageB)
        {
            // Получаем одноканальное изображение:
            var grayA = imageA; //BitmapTools.ConvertBitmapToGrayScale(image1);
            var grayB = imageB; //BitmapTools.ConvertBitmapToGrayScale(image2);

            // Запоминаем ширину и высоту изображения
            var w = Math.Max(grayA.Width, grayB.Width);
            var h = Math.Max(grayA.Height, grayB.Height);

            // Кол-во пикселей:
            var amountA = grayA.Width * grayA.Height;
            var amountB = grayB.Width * grayB.Height;
            var amount = w * h;


            // Расскалдываем картинки в одномерный массив
            var scaffA = ScaffBitmap(grayA);
            var scaffB = ScaffBitmap(grayB);

            // Получаем среднее значение цвета
            var averageA = scaffA.Sum() / amountA;
            var averageB = scaffB.Sum() / amountB;

            var numenator = 0;
            double denumerator = 0;
            double dA = 0;
            double dB = 0;

            for (var i = 0; i < amount; i++)
            {
                // Вычисляем числитель:
                numenator += (scaffA[i] - averageA) * (scaffB[i] - averageB);

                // Вычисляем часть знаменателя:
                dA += Math.Pow(scaffA[i] - averageA, 2);
                dB += Math.Pow(scaffB[i] - averageB, 2);
            }

            numenator /= amount;
            denumerator = Math.Sqrt(dA / amount) * Math.Sqrt(dB / amount);

            return numenator / denumerator;
        }

        public double Compare(string pathA, string pathB, Rectangle bound)
        {
            throw new NotImplementedException();
        }

        public double Compare(Bitmap imageA, Bitmap imageB, Rectangle bound)
        {
            throw new NotImplementedException();
        }

        public ILogger Logger { get; set; }

        #region Private Methods

        /// <summary>
        /// Раскалдывает картинку в одномерный массив
        /// </summary>
        /// <param name="source">Картинка</param>
        /// <returns>Массив пикселей</returns>
        private int[] ScaffBitmap(Bitmap source)
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

        private int Sqr(int n)
        {
            return n * n;
        }

        #endregion
    }
}
