using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hqub.Speckle.GUI.Controls
{
    using System.Drawing;

    using AviFile;

    using Image = System.Windows.Controls.Image;

    /// <summary>
    /// Interaction logic for AviSampler.xaml
    /// </summary>
    public partial class AviSampler : UserControl
    {
        public AviSampler()
        {
            InitializeComponent();
        }

        private void CreateVideo()
        {
            var aviManager = new AviManager(@"new.avi", false);

            var experiment = Core.Experiment.Get();
            var images = experiment.Images.Where(x => x.IsChecked).ToList();

            if (images.Count == 0) return;

            VideoStream aviStream = aviManager.AddVideoStream(true, 2, (Bitmap)Bitmap.FromFile(images.First().Path));

            foreach (var image in images)
            {
                var b = (Bitmap)Bitmap.FromFile(image.Path);
                aviStream.AddFrame(b);
                b.Dispose();
            }

            aviManager.Close();
        }
    }
}
