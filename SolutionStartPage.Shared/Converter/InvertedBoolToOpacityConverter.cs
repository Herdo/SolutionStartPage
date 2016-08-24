namespace SolutionStartPage.Shared.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class InvertedBoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value
                ? double.Parse(parameter.ToString(), CultureInfo.InvariantCulture)
                : 1.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}