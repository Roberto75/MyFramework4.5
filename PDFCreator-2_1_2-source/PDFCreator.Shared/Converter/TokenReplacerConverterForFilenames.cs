﻿using System;
using System.Globalization;
using System.Windows.Data;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Tokens;

namespace pdfforge.PDFCreator.Shared.Converter
{
    [ValueConversion(typeof(string), typeof(string))]
    public class TokenReplacerConverterForFilenames : IValueConverter
    {
        public TokenReplacer TokenReplacer { get; set; }

        public string Footer { get; set; }

        public TokenReplacerConverterForFilenames()
        {
            TokenReplacer = TokenHelper.TokenReplacerWithPlaceHolders;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;

            if (stringValue == null)
                throw new NotImplementedException();

            stringValue = TokenReplacer.ReplaceTokens(stringValue);

            if (Footer != null)
                stringValue += Footer;

            return FileUtil.MakeValidFileName(stringValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}

