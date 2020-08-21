using System;
using System.Windows.Data;

namespace Pomodoro.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(Nullable<DateTime>))]
    public class DateTimeAndTimeSpanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Nullable<DateTime>))
                throw new InvalidOperationException("The target must be a Nullable<DateTime>!");

            TimeSpan timeSpan = (TimeSpan)value;
            return new DateTime(1, 1, 1, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(TimeSpan))
                throw new InvalidOperationException("The target must be a TimeSpan!");

            DateTime dateTime = (DateTime)value;
            return new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        #endregion
    }
}
