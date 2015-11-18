using System.Collections.Generic;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Shared.Helper;

namespace pdfforge.PDFCreator.ViewModels.UserControls
{
    public class DebugTabViewModel : ApplicationSettingsViewModel
    {
        public IEnumerable<EnumValue<LoggingLevel>> LoggingValues
        {
            get { return TranslationHelper.Instance.GetEnumTranslation<LoggingLevel>(); }
        }
    }
}