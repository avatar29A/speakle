using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Win32;

namespace Hqub.Speckle.GUI.Controls
{
    /// <summary>
    /// Interaction logic for PreviewEtalonImage.xaml
    /// </summary>
    public partial class PreviewEtalonImage : UserControl
    {
        #region Fields

        private bool _isBackgroundSet;
        private Point _startPoint;
        private Rectangle _rect;
        private IEventAggregator _eventAggregator;

        #endregion

        #region .ctor

        public PreviewEtalonImage()
        {
            InitializeComponent();

            SubsribeOnEvents();
        }

        #endregion

        #region Properties

        public string EtalonFilePath { get; set; }

        public Rectangle Workarea
        {
            get { return _rect; }
            set { _rect = value; }
        } 

        #endregion

        #region Private Methods

        private void SubsribeOnEvents()
        {
            _eventAggregator = Events.AggregationEventService.Instance;

            _eventAggregator.GetEvent<Events.ExperimentCreatedEvent>().Subscribe(OnExperimentCreated);
        }

        private void OnExperimentCreated(Model.Events.ExperimentCreateEventEntity args)
        {
            _isBackgroundSet = false;
            _rect = null;

            Holst.Children.Clear();

            var bgr = new BitmapImage();
            bgr.BeginInit();
            
            bgr.UriSource = new Uri("pack://application:,,,/Hqub.SpeckleStudio;component/Content/loadEtalonBgr.png");
            bgr.EndInit();

            Holst.Background = new ImageBrush(bgr) { Stretch = Stretch.Uniform };
        }

        private void DrawRectangle(double x, double y, double width, double height)
        {
            Holst.Children.Remove(_rect);

            _rect = new Rectangle
            {
                Stroke = Brushes.LightBlue,
                StrokeThickness = 2,
                Width = width,
                Height = height
            };

            Canvas.SetLeft(_rect, x);
            Canvas.SetRight(_rect, y);

            Holst.Children.Add(_rect);
        }

        private void SetBackground(string filename, UriKind kind = UriKind.Absolute)
        {
            var theImage = new BitmapImage
                (new Uri(filename, kind));

            var myImageBrush = new ImageBrush(theImage);

            Holst.Background = myImageBrush;
        }

        #endregion

        #region Canvas Events

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!_isBackgroundSet)
                return;

            _startPoint = e.GetPosition(Holst);

            DrawRectangle(_startPoint.X, _startPoint.Y, 0, 0);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || _rect == null)
                return;

            var pos = e.GetPosition(Holst);

            var x = Math.Min(pos.X, _startPoint.X);
            var y = Math.Min(pos.Y, _startPoint.Y);

            var w = Math.Max(pos.X, _startPoint.X) - x;
            var h = Math.Max(pos.Y, _startPoint.Y) - y;

            _rect.Width = w;
            _rect.Height = h;

            Canvas.SetLeft(_rect, x);
            Canvas.SetTop(_rect, y);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
//            rect = null;
        }

        private void PreviewEtalonImage_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image Files (*.bmp, *.jpg)|*.bmp;*.jpg";
            var result = dialog.ShowDialog();

            if(result != true)
                return;

            if (!_isBackgroundSet)
            {
                DrawRectangle(0, 0, Holst.ActualWidth - 1, Holst.ActualHeight - 1);
                _isBackgroundSet = true;
            }

            SetBackground(dialog.FileName);
            EtalonFilePath = dialog.FileName;
        }

        #endregion
    }
}
