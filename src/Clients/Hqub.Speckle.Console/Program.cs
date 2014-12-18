using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.Console
{
    using System.Drawing;

    using Console = System.Console;

    internal class Program
    {
        public static void Main(string[] args)
        {
           
            var rect = new Rectangle(437, 391, 634 - 437, 527 - 391);


            foreach (var f in System.IO.Directory.GetFiles("c:\\temp\\photo", "*.bmp"))
            {
                var image = new Bitmap(f);
                var cropImage = Core.BitmapExtensions.BitmapTools.CropImage(image, rect);
                cropImage.Save("c:\\temp\\photo_mini\\" + System.IO.Path.GetFileName(f));
            }

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
