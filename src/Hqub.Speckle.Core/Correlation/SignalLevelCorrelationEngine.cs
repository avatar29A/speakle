using System;

namespace Hqub.Speckle.Core.Correlation
{
    using System.Drawing;
    using BitmapExtensions;

    public class SignalLevelCorrelationEngine : BaseCorrelationEngine
    {
        public override double Compare(string pathA, string pathB)
        {
            var image1 = new Bitmap(pathA);
            var image2 = new Bitmap(pathB);

            return Compare(image1, image2);
        }

        public override double Compare(string pathA, string pathB, Rectangle bound)
        {
            var image2 = BitmapTools.CropImage(new Bitmap(pathB), bound);

            // Для этого метода требуется только одно изображение:
            return Compare(image2, image2);
        }

        public override double Compare(Bitmap imageA, Bitmap imageB)
        {
            // Кол-во пикселей:
            var amountA = imageA.Width * imageA.Height;

            var lockSource = new LockBitmap(imageA);
            lockSource.LockBits();

            double sum = 0;
            for (var i = 0; i < lockSource.Width; ++i)
            {
                for (var j = 0; j < lockSource.Height; ++j)
                {
                    var pixel = lockSource.GetPixel(i, j);

                    sum += CalcBrightness(pixel.R, pixel.G, pixel.B);
                }
            }

            lockSource.UnlockBits();

            return sum/amountA;
        }

        private double CalcBrightness(double r, double g, double b)
        {
            return CalcBrightness3(r, g, b);
        }

        private double CalcBrightness1(double r, double g, double b)
        {
            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        private double CalcBrightness2(double r, double g, double b)
        {
            return 0.299 * r + 0.587 * g + 0.114 * b;
        }

        private double CalcBrightness3(double r, double g, double b)
        {
            return Math.Sqrt(0.299*Math.Pow(r, 2) + 0.587*Math.Pow(g, 2) + 0.114*Math.Pow(b, 2));
        }

        public new ILogger Logger { get; set; }
    }
}
