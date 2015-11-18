using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.ViewModels;
using pdfforge.PDFCreator.Shared.Views;
using pdfforge.PDFCreator.Shared.Views.ActionControls;
using pdfforge.PDFCreator.ViewModels;

namespace pdfforge.PDFCreator.Views.ActionControls
{
    public partial class OpenViewerActionControl : ActionControl
    {
        public OpenViewerActionControl()
        {
            InitializeComponent();
            DisplayName = TranslationHelper.Instance.GetTranslation("OpenViewerSettings", "DisplayName", "Open Document");
            Description = TranslationHelper.Instance.GetTranslation("OpenViewerSettings", "Description", "Open document after saving.");

            TranslationHelper.Instance.TranslatorInstance.Translate(this);
        }

        public override bool IsActionEnabled
        {
            get
            {
                if (CurrentProfile == null)
                    return false;
                return CurrentProfile.OpenViewer;
            }
            set
            {
                CurrentProfile.OpenViewer = value;
            }
        }

        private void OpenWithArchitectCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CurrentProfile.OpenWithPdfArchitect) 
                return;

            if (PdfArchitectHelper.IsPdfArchitectInstalled)
                return;

            const string caption = "PDF Architect";
            var message = TranslationHelper.Instance.GetTranslation("OpenViewerSettings", "ArchitectNotInstalled",
                "PDF Architect is not installed.\r\nDo You want to download it from pdfforge.org?");
            var result = MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.YesNo, MessageWindowIcon.PDFForge);
            if (result == MessageWindowResponse.Yes)
            {
                Process.Start(Urls.ArchitectDownloadUrl);
            }
            OpenWithArchitectCheckBox.IsChecked = false;
        }

        private void GetPdfArchitectButton_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start(Urls.ArchitectWebsiteUrl);
        }
    }
}