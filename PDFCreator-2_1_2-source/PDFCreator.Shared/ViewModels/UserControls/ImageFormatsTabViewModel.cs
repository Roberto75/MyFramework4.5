using System.Collections.Generic;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Shared.Helper;

namespace pdfforge.PDFCreator.Shared.ViewModels.UserControls
{
    public class ImageFormatsTabViewModel : CurrentProfileViewModel
    {
        private static readonly TranslationHelper TranslationHelper = TranslationHelper.Instance;

        public static IEnumerable<EnumValue<JpegColor>> JpegColorValues
        {
            get { return TranslationHelper.GetEnumTranslation<JpegColor>(); }
        }

        public static IEnumerable<EnumValue<PngColor>> PngColorValues
        {
            get { return TranslationHelper.GetEnumTranslation<PngColor>(); }
        }

        public static IEnumerable<EnumValue<TiffColor>> TiffColorValues
        {
            get { return TranslationHelper.GetEnumTranslation<TiffColor>(); }
        }

    }
}
