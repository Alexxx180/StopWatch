using System;
using System.Globalization;
using System.Windows.Data;

namespace StopWatch
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] time = value as byte[];
            return string.Format("{4:0}:{3:00}:{2:00}:{1:00}.{0:0}",
              time[0], time[1], time[2], time[3], time[4]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
