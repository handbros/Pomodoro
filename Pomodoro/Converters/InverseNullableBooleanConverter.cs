using System;
using System.Windows.Data;

namespace Pomodoro.Converters
{
    [ValueConversion(typeof(bool), typeof(Nullable<bool>))]
    public class InverseNullableBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Nullable<bool>))
                throw new InvalidOperationException("The target must be a Nullable<bool>!");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a bool!");

            return !(bool)value;
        }

        #endregion
    }
}
