namespace Hqub.Speckle.GUI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    public class StartStopIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            const string StartIcon = "play.png";
            const string StopIcon = "stop.png";

            var isRunning = (bool)value;

            var bgr = new BitmapImage();
            bgr.BeginInit();
            bgr.UriSource =
                new Uri(
                    "pack://application:,,,/Hqub.SpeckleStudio;component/Content/" + (isRunning ? StopIcon : StartIcon));


            bgr.EndInit();

            return bgr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
