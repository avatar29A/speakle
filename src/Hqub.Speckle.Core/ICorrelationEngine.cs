using System.Drawing;

namespace Hqub.Speckle.Core
{
    public interface ICorrelationEngine
    {
        void AddImage();
        double Compare(string pathA, string pathB);
        double Compare(Bitmap imageA, Bitmap imageB);
        double Compare(string pathA, string pathB, Rectangle bound);
        double Compare(Bitmap imageA, Bitmap imageB, Rectangle bound);

        ILogger Logger { get; set; }
    }
}
