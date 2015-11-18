using System;
using System.Windows;
using System.Windows.Input;
using NLog;
using pdfforge.PDFCreator.Assistants;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Properties;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.Helper.Logging;

namespace pdfforge.PDFCreator.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AppSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            PdfCreatorSettings settings = SettingsHelper.Settings;
            string currentLanguage = settings.ApplicationSettings.Language;

            ApplicationSettings tmpSettings = settings.ApplicationSettings.Copy();
            var tmpProperties = settings.ApplicationProperties.Copy();
            var window = new ApplicationSettingsWindow(tmpSettings, tmpProperties, settings.ConversionProfiles);

            if (window.ShowDialog() == true)
            {
                settings.ApplicationSettings = tmpSettings;
                settings.ApplicationProperties = tmpProperties;
                if (!string.Equals(currentLanguage, settings.ApplicationSettings.Language, StringComparison.InvariantCultureIgnoreCase))
                {
                    TranslationHelper.Instance.InitTranslator(settings.ApplicationSettings.Language);
                    TranslationHelper.Instance.TranslatorInstance.Translate(this);
                }
                SettingsHelper.SaveSettings();
            }
            //Translation of profiles are stored in their name property and could have been changed in the AppSettingsWindow
            //To include the current language it must be translated here 
            TranslationHelper.Instance.TranslateProfileList(SettingsHelper.Settings.ConversionProfiles);

            LoggingHelper.ChangeLogLevel(settings.ApplicationSettings.LoggingLevel);
        }

        private void ProfileSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new ProfileSettingsWindow(SettingsHelper.Settings);
            window.ShowDialog();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TranslationHelper.Instance.TranslatorInstance.Translate(this);

            ApplicationNameText.Text = "PDFCreator " + VersionHelper.GetCurrentApplicationVersion_TwoDigits();

            // Apply company name for customized setups
            ApplyCustomization();

            var welcomeSettingsHelper = new WelcomeSettingsHelper();
            if (welcomeSettingsHelper.IsFirstRun())
            {
                welcomeSettingsHelper.SetCurrentApplicationVersionAsWelcomeVersionInRegistry();
                WelcomeWindow.ShowDialogTopMost();
            }
        }

        private void ApplyCustomization()
        {
            if (Properties.Licensing.ApplyLicensing.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            {
                LicensedForText.Visibility = Visibility.Visible;
                LicensedForText.Text = Properties.Licensing.MainForm;
            }
            else
            {
                LicensedForText.Visibility = Visibility.Hidden;
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            var w = new AboutWindow();
            w.ShowDialog();
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                UserGuideHelper.ShowHelp(HelpTopic.General);
            }
        }

        private void MainWindow_OnDragEnter(object sender, DragEventArgs e)
        {
            if (((string[])e.Data.GetData(DataFormats.FileDrop)).Length == 0)
                e.Effects = DragDropEffects.None;
            else
                e.Effects = DragDropEffects.Copy;
        }

        private void MainWindow_OnDrop(object sender, DragEventArgs e)
        {
            var printFile = new PrintFileAssistant();
            if (printFile.AddFiles((string[])e.Data.GetData(DataFormats.FileDrop, false)))
                printFile.PrintAll();
        }

        public static bool? ShowDialogTopMost()
        {
            var w = new MainWindow();
            return TopMostHelper.ShowDialogTopMost(w, true);
        }
    }
}
