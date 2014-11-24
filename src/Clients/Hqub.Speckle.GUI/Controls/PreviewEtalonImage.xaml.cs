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
    /// <summary>
    /// Interaction logic for PreviewEtalonImage.xaml
    /// </summary>
    public partial class PreviewEtalonImage : UserControl
    {
        public PreviewEtalonImage()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            DrawRectangle(0, 0, Holst.ActualWidth - 1, Holst.ActualHeight - 1);
            base.OnRender(drawingContext);
        }

        private void DrawRectangle(double x, double y, double width, double height)
        {
            Holst.Children.Remove(rect);

            rect = new Rectangle
            {
                Stroke = Brushes.LightBlue,
                StrokeThickness = 2,
                Width = width,
                Height = height
            };

            Canvas.SetLeft(rect, x);
            Canvas.SetRight(rect, y);

            Holst.Children.Add(rect);
        }

        private Point startPoint;
        private Rectangle rect;

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            startPoint = e.GetPosition(Holst);

            DrawRectangle(startPoint.X, startPoint.Y, 0, 0);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || rect == null)
                return;

            var pos = e.GetPosition(Holst);

            var x = Math.Min(pos.X, startPoint.X);
            var y = Math.Min(pos.Y, startPoint.Y);

            var w = Math.Max(pos.X, startPoint.X) - x;
            var h = Math.Max(pos.Y, startPoint.Y) - y;

            rect.Width = w;
            rect.Height = h;

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
//            rect = null;
        }
    }
}
