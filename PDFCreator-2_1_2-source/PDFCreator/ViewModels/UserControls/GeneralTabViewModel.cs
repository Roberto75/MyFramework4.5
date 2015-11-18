using System.Collections.Generic;
using pdfforge.DynamicTranslator;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Shared.Helper;

namespace pdfforge.PDFCreator.ViewModels.UserControls
{
    public class GeneralTabViewModel : ApplicationSettingsViewModel
    {
        private ApplicationProperties _applicationProperties;
        private IList<Language> _languages;

        public GeneralTabViewModel()
        {
            Languages = Translator.FindTranslations(TranslationHelper.Instance.TranslationPath);
        }

        public ApplicationProperties ApplicationProperties
        {
            get { return _applicationProperties; }
            set
            {
                _applicationProperties = value;
                RaisePropertyChanged("ApplicationProperties");
            }
        }

        public IList<Language> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                RaisePropertyChanged("Languages");
            }
        }

        public IEnumerable<AskSwitchPrinter> AskSwitchPrinterValues
        {
            get
            {
                return new List<AskSwitchPrinter>
                {
                    new AskSwitchPrinter(
                        TranslationHelper.Instance.GetTranslation("ApplicationSettingsWindow", "Ask", "Ask"), true),
                    new AskSwitchPrinter(
                        TranslationHelper.Instance.GetTranslation("ApplicationSettingsWindow", "Yes", "Yes"), false)
                };
            }
        }

        public IEnumerable<EnumValue<UpdateInterval>> UpdateIntervals
        {
            get { return TranslationHelper.Instance.GetEnumTranslation<UpdateInterval>(); }
        }
    }

    public class AskSwitchPrinter
    {
        public AskSwitchPrinter(string name, bool value)
        {
            Value = value;
            Name = name;
        }

        public bool Value { get; set; }
        public string Name { get; set; }
    }
}