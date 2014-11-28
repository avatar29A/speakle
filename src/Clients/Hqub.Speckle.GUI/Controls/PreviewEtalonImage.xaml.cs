﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Hqub.Speckle.Core.Model;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Win32;

namespace Hqub.Speckle.GUI.Controls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using JetBrains.Annotations;

    /// <summary>
    /// Interaction logic for PreviewEtalonImage.xaml
    /// </summary>
    public partial class PreviewEtalonImage : UserControl, INotifyPropertyChanged
    {
        #region Fields

        private bool _isBackgroundSet;
        private Point _startPoint;
        private Rectangle _rect;
        private IEventAggregator _eventAggregator;

        private double mouseX;

        private double mouseY;

        private System.Drawing.Rectangle workarea;

        #endregion

        #region .ctor

        public PreviewEtalonImage()
        {
            InitializeComponent();

            SubsribeOnEvents();

         Holst.SizeChanged += Holst_SizeChanged;
        }

        private void Holst_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var deltaHeight = Math.Abs(e.NewSize.Height - e.PreviousSize.Height);
            var deltaWidth = Math.Abs(e.NewSize.Width - e.PreviousSize.Width);

            var percentChangeWidth = deltaWidth / e.PreviousSize.Width;
            var precentChangeHeight = deltaHeight / e.PreviousSize.Height;

            var rectWidth = 0;
            var rectHeight = 0;

        }

        private int GetSign(double oldValue, double newValue)
        {
            return oldValue <= newValue ? 1 : -1;
        }

        #endregion

        #region Properties

        public string EtalonFilePath { get; set; }

        public System.Drawing.Rectangle Workarea
        {
            get
            {
                return this.workarea;
            }
            set
            {
                this.workarea = value;
                this.OnPropertyChanged();
            }
        }

        public Size OriginalSize { get; set; }

        public string ImageInfo
        {
            get
            {
                return string.Format("Image: X={2}; Y={3}; W={0}; H={1};", Holst.Width, Holst.Height);
            }
        }

        #region Mouse XY

        public double MouseX
        {
            get
            {
                return this.mouseX;
            }
            set
            {
                this.mouseX = value;
                this.OnPropertyChanged();
            }
        }

        public double MouseY
        {
            get
            {
                return this.mouseY;
            }
            set
            {
                this.mouseY = value;
                this.OnPropertyChanged();
            }
        }

        #endregion

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
            
            OriginalSize = new Size(theImage.Width, theImage.Height);

            var myImageBrush = new ImageBrush(theImage);
            myImageBrush.Stretch = Stretch.None;

            Holst.Background = myImageBrush;
            Holst.Width = OriginalSize.Width;
            Holst.Height = OriginalSize.Height;

            Workarea = new System.Drawing.Rectangle(0,0, (int)OriginalSize.Width, (int)OriginalSize.Height);
            Core.Experiment.Get().WorkAreay = Workarea;

            this.Background = Brushes.Black;
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
            if (!_isBackgroundSet)
                return;

            var pos = e.GetPosition(Holst);
            MouseX = pos.X;
            MouseY = pos.Y;

            if (e.LeftButton == MouseButtonState.Released || _rect == null)
                return;

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
            if (_rect == null) return;

            var point = e.GetPosition(Holst);

            Workarea = new System.Drawing.Rectangle((int)_startPoint.X, (int)_startPoint.Y, (int)point.X, (int)point.Y);
            var experiment = Core.Experiment.Get();
            experiment.WorkAreay = Workarea;
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
                _isBackgroundSet = true;
            }

            SetBackground(dialog.FileName);
            EtalonFilePath = dialog.FileName;

            // Уведомляем всех подписчиков, что загрузили новый эталон:
            var customEvent = _eventAggregator.GetEvent<Events.EtalonLoadedEvent>();
            customEvent.Publish(new ImageWrapper
            {
                Name = System.IO.Path.GetFileName(EtalonFilePath),
                Path = EtalonFilePath,
                Correlation = new CorrelationValue()

            });
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
