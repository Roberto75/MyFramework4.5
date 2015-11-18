using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Shared.Helper;

namespace pdfforge.PDFCreator.Views
{
    public partial class ApplicationSettingsWindow : Window
    {
        public ApplicationSettingsWindow(ApplicationSettings applicationSettings,
            ApplicationProperties applicationProperties, IEnumerable<ConversionProfile> conversionProfiles)
            : this()
        {
            GeneralTabUserControl.ViewModel.ApplicationSettings = applicationSettings;
            GeneralTabUserControl.ViewModel.ApplicationProperties = applicationProperties;
            GeneralTabUserControl.PreviewLanguageAction = PreviewLanguageAction;
            TitleTabUserControl.ViewModel.ApplicationSettings = applicationSettings;
            DebugTabUserControl.ViewModel.ApplicationSettings = applicationSettings;
            DebugTabUserControl.UpdateSettings = UpdateSettingsAction;
            PrinterTabUserControl.ViewModel.ConversionProfiles = conversionProfiles;
            PrinterTabUserControl.ViewModel.ApplicationSettings = applicationSettings;
        }

        private void UpdateSettingsAction(PdfCreatorSettings settings)
        {
            GeneralTabUserControl.ViewModel.ApplicationSettings = settings.ApplicationSettings;
            GeneralTabUserControl.ViewModel.ApplicationProperties = settings.ApplicationProperties;
            TitleTabUserControl.ViewModel.ApplicationSettings = settings.ApplicationSettings;
            PrinterTabUserControl.ViewModel.ConversionProfiles = settings.ConversionProfiles;
            PrinterTabUserControl.ViewModel.ApplicationSettings = settings.ApplicationSettings;
        }


        private void PreviewLanguageAction()
        {
            TranslationHelper.Instance.TranslatorInstance.Translate(this);
            TranslationHelper.Instance.TranslatorInstance.Translate(PrinterTabUserControl);
            PrinterTabUserControl.UpdateProfilesList();
            TranslationHelper.Instance.TranslatorInstance.Translate(TitleTabUserControl);
            TranslationHelper.Instance.TranslatorInstance.Translate(DebugTabUserControl);
            TranslationHelper.Instance.TranslatorInstance.Translate(PdfArchitectTabUserControl);
        }

        public ApplicationSettingsWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ShowConextBasedHelp()
        {
            var active = TabControl.SelectedItem as TabItem;
            if (active == GeneralTab)
                UserGuideHelper.ShowHelp(HelpTopic.AppGeneral);
            else if (active == PrinterTab)
                UserGuideHelper.ShowHelp(HelpTopic.AppPrinters);
            else if (active == TitleTab)
                UserGuideHelper.ShowHelp(HelpTopic.AppTitle);
            else if (active == ApiTab)
                UserGuideHelper.ShowHelp(HelpTopic.AppApiServices);
            else if (active == DebugTab)
                UserGuideHelper.ShowHelp(HelpTopic.AppDebug);
            else
                UserGuideHelper.ShowHelp(HelpTopic.AppGeneral);
        }

        private void HelpButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowConextBasedHelp();
        }

        private void ApplicationSettingsWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                ShowConextBasedHelp();
        }

        private void ApplicationSettingsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TranslationHelper.Instance.TranslatorInstance.Translate(this);
        }

        private void ApplicationSettingsWindow_OnClosed(object sender, EventArgs e)
        {
            TranslationHelper.Instance.RevertTemporaryTranslation();
        }
    }
}