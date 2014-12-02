using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Speckle.GUI.Converters
{
    using System.Globalization;
    using System.Windows.Data;

    public class StartStopTooltipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isRunning = (bool)value;

            return isRunning ? "Остановить работу эксперимента" : "Запустить эксперимент";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
