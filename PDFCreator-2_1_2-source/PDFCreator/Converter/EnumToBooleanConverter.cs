using System;
using System.Windows;
using System.Windows.Data;

namespace pdfforge.PDFCreator.Converter
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter.ToString();   
            return Enum.Parse(targetType, parameterString);
        }
    }
}
