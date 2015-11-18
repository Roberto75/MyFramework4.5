using System;
using System.Globalization;
using System.Windows.Data;

namespace pdfforge.PDFCreator.Shared.Converter
{
    internal class ColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof (System.Windows.Media.Color))
            {
                return ConvertWpfColor(value);
            }

            if (targetType == typeof(System.Drawing.Color))
            {
                return ConvertFormsColor(value);
            }

            throw new NotImplementedException();
        }

        private System.Windows.Media.Color ConvertWpfColor(object value)
        {
            if (value is System.Windows.Media.Color)
                return (System.Windows.Media.Color)value;

            if (value is System.Drawing.Color)
            {
                var color = (System.Drawing.Color) value;
                return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            }

            throw new NotImplementedException();
        }

        private System.Drawing.Color ConvertFormsColor(object value)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
