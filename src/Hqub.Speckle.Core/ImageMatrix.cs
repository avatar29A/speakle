using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.Core
{
    public class ImageMatrix
    {
        public Color[,] Pixels { get; set; }

        public static ImageMatrix Create(Bitmap image)
        {
            var instance = new ImageMatrix {Pixels = new Color[image.Width, image.Height]};

            for (var i = 0; i < image.Width; i++)
            {
                for (var j = 0; j < image.Height; j++)
                {
                    instance.Pixels[i,j] = image.GetPixel(i, j);
                }
            }

            return instance;
        }
    }
}
