using MaterialDesignColors;
using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Pomodoro.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(Brush))]
    public class TimeSpanToBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                throw new InvalidOperationException("The target must be a brush!");

            if (((TimeSpan)value).TotalSeconds <= 5.0f)
            {
                Color color = SwatchHelper.Lookup[MaterialDesignColor.RedA700];
                return new SolidColorBrush(color);
            }
            else if (((TimeSpan)value).TotalSeconds <= 10.0f)
            {
                Color color = SwatchHelper.Lookup[MaterialDesignColor.DeepOrange500];
                return new SolidColorBrush(color);
            }
            else
            {
                Color color = SwatchHelper.Lookup[MaterialDesignColor.Blue500];
                return new SolidColorBrush(color);
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
