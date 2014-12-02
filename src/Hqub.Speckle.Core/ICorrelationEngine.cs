using System.Drawing;

namespace Hqub.Speckle.Core
{
    public interface ICorrelationEngine
    {
        double Compare(string pathA, string pathB);
        double Compare(string pathA, string pathB, Rectangle bound);

        double Compare(Bitmap imageA, Bitmap imageB);

        ILogger Logger { get; set; }
    }
}
