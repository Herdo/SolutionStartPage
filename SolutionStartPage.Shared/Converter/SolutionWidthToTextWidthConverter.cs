namespace SolutionStartPage.Shared.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class SolutionWidthToTextWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            return d - 32.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}