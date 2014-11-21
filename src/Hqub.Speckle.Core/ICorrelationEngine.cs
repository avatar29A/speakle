using System.Drawing;

namespace Hqub.Speckle.Core
{
    public interface ICorrelationEngine
    {
        void AddImage();
        double Compare(string pathA, string pathB);
        double Compare(Bitmap imageA, Bitmap imageB);
    }
}
