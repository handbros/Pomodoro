using System;
using System.Windows.Data;

namespace Pomodoro.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a string!");

            if (((TimeSpan)value).Days > 0)
            {
                throw new ArgumentOutOfRangeException("The time span must be not exceed hour range.");
            }
            else
            {
                return string.Format("{0:D2}:{1:D2}:{2:D2}", ((TimeSpan)value).Hours, ((TimeSpan)value).Minutes, ((TimeSpan)value).Seconds);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
