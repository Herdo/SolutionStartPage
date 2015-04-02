namespace SolutionStartPage.Shared.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value
                ? 1.0
                : Double.Parse(parameter.ToString(), CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}