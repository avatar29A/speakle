namespace Hqub.Speckle.Core.Correlation
{
    using System.Drawing;

    using Hqub.Speckle.Core.BitmapExtensions;

    public class SignalLevelCorrelationEngine : ICorrelationEngine
    {
        public double Compare(string pathA, string pathB)
        {
            var image1 = new Bitmap(pathA);
            var image2 = new Bitmap(pathB);

            return Compare(image1, image2);
        }

        public double Compare(string pathA, string pathB, Rectangle bound)
        {
            var boundOnePixel = new Rectangle(bound.X, bound.Y, 1, 1);
            var image1 = BitmapTools.CropImage(new Bitmap(pathA), boundOnePixel);
            var image2 = BitmapTools.CropImage(new Bitmap(pathB), boundOnePixel);

            return Compare(image1, image2);
        }

        public double Compare(Bitmap imageA, Bitmap imageB)
        {
            // Получаем одноканальное изображение:
//            var grayA = BitmapTools.MakeGrayscale3(imageA);
            var grayB = BitmapTools.MakeGrayscale3(imageB);

//            var pixelA = grayA.GetPixel(0, 0);
            var pixelB = grayB.GetPixel(0, 0);

//            var binA = pixelA.R;
            var binB = pixelB.R;

            return binB / 100.0;    
        }

        public ILogger Logger { get; set; }
    }
}
