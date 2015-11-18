using System;
using System.Windows;
using System.Windows.Input;
using pdfforge.PDFCreator.Assistants;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.ViewModels;
using pdfforge.PDFCreator.WindowsApi;

namespace pdfforge.PDFCreator.Views
{
    public partial class PrintJobWindow : Window
    {
        private PdfCreatorSettings _settings = SettingsHelper.Settings;
        
        public PrintJobWindow()
        {
            InitializeComponent();
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = (PrintJobViewModel)DataContext;

            TopMostHelper.UndoTopMostWindow(this);
            _settings.ApplicationSettings.LastUsedProfileGuid = vm.SelectedProfile.Guid;

            var window = new ProfileSettingsWindow(_settings);
            if (window.ShowDialog() == true)
            {
                _settings = window.Settings;

                vm.Profiles = _settings.ConversionProfiles;
                vm.ApplicationSettings = _settings.ApplicationSettings;
                vm.SelectProfileByGuid(_settings.ApplicationSettings.LastUsedProfileGuid);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TranslationHelper.Instance.TranslatorInstance.Translate(this);

            if (Properties.Licensing.ApplyLicensing.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                Title = Properties.Licensing.PrintJobWindowCaption;

            FlashWindow.Flash(this, 3);
        }

        private void CommandButtons_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (((string[])e.Data.GetData(DataFormats.FileDrop)).Length == 0)
                e.Effects = DragDropEffects.None;
            else
                e.Effects = DragDropEffects.Copy;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            var printFile = new PrintFileAssistant();
            if (printFile.AddFiles((string[])e.Data.GetData(DataFormats.FileDrop, false)))
                printFile.PrintAll();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
                UserGuideHelper.ShowHelp(null, HelpTopic.CreatingPdf);
        }
    }
}
