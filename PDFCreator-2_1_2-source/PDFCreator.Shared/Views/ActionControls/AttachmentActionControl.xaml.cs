﻿using System.Windows;
using pdfforge.PDFCreator.Shared.Helper;

namespace pdfforge.PDFCreator.Shared.Views.ActionControls
{
    public partial class AttachmentActionControl : ActionControl
    {
        public override bool IsActionEnabled
        {
            get
            {
                if (CurrentProfile == null)
                    return false;
                return CurrentProfile.AttachmentPage.Enabled;
            }
            set
            {
                CurrentProfile.AttachmentPage.Enabled = value;
            }
        }

        public AttachmentActionControl()
        {
            InitializeComponent();

            DisplayName = TranslationHelper.Instance.GetTranslation("AttachmentSettings", "DisplayName", "Add attachment");
            Description = TranslationHelper.Instance.GetTranslation("AttachmentSettings", "Description", "Add an attachment to your documents.\r\nThe attachment file must be a PDF file and may contain multiple pages.");

            TranslationHelper.Instance.TranslatorInstance.Translate(this);
        }

        private void SelectAttachmentFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            var title = TranslationHelper.Instance.GetTranslation("AttachmentSettings", "SelectAttachmentFile", "Select attachment file");
            var filter = TranslationHelper.Instance.GetTranslation("AttachmentSettings", "PDFFiles", "PDF files")
                                    + @" (*.pdf)|*.pdf|"
                                    + TranslationHelper.Instance.GetTranslation("AttachmentSettings", "AllFiles", "All files")
                                    + @" (*.*)|*.*";
            
            FileDialogHelper.ShowSelectFileDialog(AttachmentFileTextBox, title, filter);
        }
    }
}