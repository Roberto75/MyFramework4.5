﻿using System;
using System.Windows.Data;

namespace pdfforge.PDFCreator.Shared.Converter
{
    public class StringIsNullOrWhiteSpaceToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            var valueString = value as string;

            return !string.IsNullOrWhiteSpace(valueString);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
