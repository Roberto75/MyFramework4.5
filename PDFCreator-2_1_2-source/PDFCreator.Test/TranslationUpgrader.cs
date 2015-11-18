using pdfforge.PDFCreator.Helper;

namespace PDFCreator.Test
{
    class TranslationUpgrader : DataUpgrader
    {

        public void UpdateApplicationSettignsWindowTranslations()
        {
            CreateGeneralTabSection();
            CreatePrinterTabSection();
            CreateTitleTabSection();
            CreateDebugTabSection();
            CreatePdfArchitectTabSection();
        }

        private void CreatePdfArchitectTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ApplicationSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Views.UserControls.PdfArchitectTab\\";
            MoveValue("DownloadPdfArchitectButton.Content", oldSection, newSection);
            MoveValue("GetPdfArchitectButton.Content", oldSection, newSection);
            MoveValue("LaunchPdfArchitectButton.Content", oldSection, newSection);
            MoveValue("PdfArchitectAddModulesText.Text", oldSection, newSection);
            MoveValue("PdfArchitectConvertLongText.Text", oldSection, newSection);
            MoveValue("PdfArchitectConvertText.Text", oldSection, newSection);
            MoveValue("PdfArchitectEditAndFontsText.Text", oldSection, newSection);
            MoveValue("PdfArchitectEditText.Text", oldSection, newSection);
            MoveValue("PdfArchitectFormatAndMarginsText.Text", oldSection, newSection);
            MoveValue("PdfArchitectFreeText.Text", oldSection, newSection);
            MoveValue("PdfArchitectInsertAndDeleteText.Text", oldSection, newSection);
            MoveValue("PdfArchitectInstalledText.Text", oldSection, newSection);
            MoveValue("PdfArchitectIntroText.Text", oldSection, newSection);
            MoveValue("PdfArchitectOcrLongText.Text", oldSection, newSection);
            MoveValue("PdfArchitectSplitAndMergeText.Text", oldSection, newSection);
            MoveValue("PdfArchitectTextRecognitionOcrText.Text", oldSection, newSection);
            MoveValue("PdfArchitectViewAndPrintText.Text", oldSection, newSection);
        }

        private void CreateDebugTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ApplicationSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Views.UserControls.DebugTab\\";
            MoveValue("ClearLogFileButton.Content", oldSection, newSection);
            MoveValue("ExportSettingsControl.Header", oldSection, newSection);
            MoveValue("LoadSettingsButton.Content", oldSection, newSection);
            MoveValue("LoggingControl.Header", oldSection, newSection);
            MoveValue("LoggingLevelLabel.Text", oldSection, newSection);
            MoveValue("SaveSettingsButton.Content", oldSection, newSection);
            MoveValue("ShowLogFileButton.Content", oldSection, newSection);
            MoveValue("TestPagesControl.Header", oldSection, newSection);
            MoveValue("WindowsTestpageButton.Content", oldSection, newSection);
            MoveValue("PdfCreatorTestpageButton.Content", oldSection, newSection);
        }

        private void CreateTitleTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ApplicationSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Views.UserControls.TitleTab\\";
            MoveValue("ReplaceColumn.Header", oldSection, newSection);
            MoveValue("SearchColumn.Header", oldSection, newSection);
            MoveValue("TitleReplacementControl.Header", oldSection, newSection);

        }
        
        private void CreatePrinterTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ApplicationSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Views.UserControls.PrinterTab\\";
            MoveValue("AddPrinterButton.Text", oldSection, newSection);
            MoveValue("DeletePrinterButton.Text", oldSection, newSection);
            MoveValue("ManagePrintersControl.Header", oldSection, newSection);
            MoveValue("PrimaryColumn.Header", oldSection, newSection);
            MoveValue("PrimaryPrinterControl.Header", oldSection, newSection);
            MoveValue("PrinterColumn.Header", oldSection, newSection);
            MoveValue("ProfileColumn.Header", oldSection, newSection);
            MoveValue("RenamePrinterButton.Text", oldSection, newSection);
        }

        private void CreateGeneralTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ApplicationSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Views.UserControls.GeneralTab\\";
            MoveValue("AddMenuIntegrationText.Text", oldSection, newSection);
            MoveValue("ChangeDefaultPrinterLabel.Text", oldSection, newSection);
            MoveValue("CheckUpdateButton.Content", oldSection, newSection);
            MoveValue("DefaultPrinterControl.Header", oldSection, newSection);
            MoveValue("DownloadLatestVersionText.Text", oldSection, newSection);
            MoveValue("LanguageControl.Header", oldSection, newSection);
            MoveValue("LanguagePreviewButton.Content", oldSection, newSection);
            MoveValue("MenuIntegrationControl.Header", oldSection, newSection);
            MoveValue("RemoveMenuIntegrationText.Text", oldSection, newSection);
            MoveValue("SelectLanguageLabel.Text", oldSection, newSection);
            MoveValue("UpdateCheckControl.Header", oldSection, newSection);
            MoveValue("UpdateIntervalLabel.Text", oldSection, newSection);
        }













        public void UpdateProfileSettingsWindowTranslations()
        {
            MoveSection("pdfforge.PDFCreator.Views.ActionControls.AttachmentActionControl\\", "pdfforge.PDFCreator.Shared.Views.ActionControls.AttachmentActionControl\\");
            MoveSection("pdfforge.PDFCreator.Views.ActionControls.BackgroundActionControl\\", "pdfforge.PDFCreator.Shared.Views.ActionControls.BackgroundActionControl\\");
            MoveSection("pdfforge.PDFCreator.Views.ActionControls.CoverActionControl\\", "pdfforge.PDFCreator.Shared.Views.ActionControls.CoverActionControl\\");
            MoveSection("pdfforge.PDFCreator.Views.ActionControls.EmailSmtpActionControl\\", "pdfforge.PDFCreator.Shared.Views.ActionControls.EmailSmtpActionControl\\");
            MoveSection("pdfforge.PDFCreator.Views.ActionControls.FtpActionControl\\", "pdfforge.PDFCreator.Shared.Views.ActionControls.FtpActionControl\\");
            MoveSection("pdfforge.PDFCreator.Views.ActionControls.PrintActionControl\\", "pdfforge.PDFCreator.Shared.Views.ActionControls.PrintActionControl\\");
            MoveSection("pdfforge.PDFCreator.Views.ActionControls.ScriptActionControl\\", "pdfforge.PDFCreator.Shared.Views.ActionControls.ScriptActionControl\\");
            MoveSection("pdfforge.PDFCreator.Views.EditEmailTextWindow\\", "pdfforge.PDFCreator.Shared.Views.EditEmailTextWindow\\");
            MoveSection("pdfforge.PDFCreator.Views.EncryptionPasswordsWindow\\", "pdfforge.PDFCreator.Shared.Views.EncryptionPasswordsWindow\\");
            MoveSection("pdfforge.PDFCreator.Views.FtpPasswordWindow\\", "pdfforge.PDFCreator.Shared.Views.FtpPasswordWindow\\");
            MoveSection("pdfforge.PDFCreator.Views.InputBoxWindow\\", "pdfforge.PDFCreator.Shared.Views.InputBoxWindow\\");
            MoveSection("pdfforge.PDFCreator.Views.SignaturePasswordWindow\\", "pdfforge.PDFCreator.Shared.Views.SignaturePasswordWindow\\");
            MoveSection("pdfforge.PDFCreator.Views.SmtpPasswordWindow\\", "pdfforge.PDFCreator.Shared.Views.SmtpPasswordWindow\\");
            MoveSection("pdfforge.PDFCreator.Views.TimeServerPasswordWindow\\", "pdfforge.PDFCreator.Shared.Views.TimeServerPasswordWindow\\");


            UpdateProfileSettingsWindow();
        }

        private void UpdateProfileSettingsWindow()
        {
            CreateActionsTabSection();
            CreatePdfTabSection();
            CreateDocumentTabSection();
            CreateAutosaveTabSection();
            CreateSaveTabSection();
            CreateImageFormatsTabSection();
        }

        private void CreateImageFormatsTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ProfileSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Shared.Views.UserControls.ImageFormatsTab\\";

            MoveValue("JpegColorsLabel.Content", oldSection, newSection);
            MoveValue("JpegControl.Header", oldSection, newSection);
            MoveValue("JpegQualityLabel.Content", oldSection, newSection);
            MoveValue("JpegResolutionLabel.Content", oldSection, newSection);
            MoveValue("PngColorsLabel.Content", oldSection, newSection);
            MoveValue("PngControl.Header", oldSection, newSection);
            MoveValue("PngResolutionLabel.Content", oldSection, newSection);
            MoveValue("TiffColorsLabel.Content", oldSection, newSection);
            MoveValue("TiffControl.Header", oldSection, newSection);
            MoveValue("TiffResolutionLabel.Content", oldSection, newSection);
        }

        private void CreateSaveTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ProfileSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Views.UserControls.SaveTab\\";

            MoveValue("ConversionControl.Header", oldSection, newSection);
            MoveValue("DefaultFileFormatLabel.Content", oldSection, newSection);
            MoveValue("FilenameControl.Header", oldSection, newSection);
            MoveValue("FilenamePreviewLabel.Content", oldSection, newSection);
            MoveValue("FilenameTemplateLabel.Content", oldSection, newSection);
            MoveValue("FilenameTokenLabel.Content", oldSection, newSection);
            MoveValue("SaveDialogFolderCheckBox.Content", oldSection, newSection);
            MoveValue("SaveDialogFolderControl.Header", oldSection, newSection);
            MoveValue("SaveDialogFolderPreviewLabel.Content", oldSection, newSection);
            MoveValue("SaveDialogFolderTemplateLabel.Content", oldSection, newSection);
            MoveValue("SaveDialogFolderTokenLabel.Content", oldSection, newSection);
            MoveValue("ShowProgressCheckBox.Content", oldSection, newSection);
            MoveValue("SkipPrintDialogCheckBox.Content", oldSection, newSection);
        }

        private void CreateAutosaveTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ProfileSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Views.UserControls.AutosaveTab\\";

            MoveValue("AutomaticSavingHintText.Text", oldSection, newSection);
            MoveValue("AutomaticSavingControl.Header", oldSection, newSection);
            MoveValue("AutomaticSavingCheckBox.Content", oldSection, newSection);
            MoveValue("EnsureUniqueFilenamesCheckBox.Content", oldSection, newSection);
            MoveValue("TargetFolderLabel.Content", oldSection, newSection);
            MoveValue("TargetFolderPreviewLabel.Content", oldSection, newSection);
            MoveValue("TargetFolderTokenLabel.Content", oldSection, newSection);
        }

        private void CreateDocumentTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ProfileSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Shared.Views.UserControls.DocumentTab\\";

            MoveValue("AuthorLabel.Content", oldSection, newSection);
            MoveValue("AuthorPreviewLabel.Content", oldSection, newSection);
            MoveValue("AuthorTokenLabel.Content", oldSection, newSection);
            MoveValue("StampCheckBox.Content", oldSection, newSection);
            MoveValue("StampControl.Header", oldSection, newSection);
            MoveValue("StampFontAsOutlineCheckBox.Content", oldSection, newSection);
            MoveValue("StampFontColorLabel.Content", oldSection, newSection);
            MoveValue("StampFontLabel.Content", oldSection, newSection);
            MoveValue("StampOutlineWidthLabel.Content", oldSection, newSection);
            MoveValue("StampTextLabel.Content", oldSection, newSection);
            MoveValue("TitleAndAuthorTemplatesControl.Header", oldSection, newSection);
            MoveValue("TitleLabel.Content", oldSection, newSection);
            MoveValue("TitlePreviewLabel.Content", oldSection, newSection);
            MoveValue("TitleTokenLabel.Content", oldSection, newSection);
        }

        private void CreatePdfTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ProfileSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Shared.Views.UserControls.PdfTab\\";

            MoveValue("AllowMultiSigningCheckBox.Content", oldSection, newSection);
            MoveValue("CompressionColorAndGrayCheckBox.Content", oldSection, newSection);
            MoveValue("CompressionColorAndGrayContentControl.Header", oldSection, newSection);
            MoveValue("CompressionColorAndGrayDpiLabel.Content", oldSection, newSection);
            MoveValue("CompressionColorAndGrayJpegFactorLabel.Content", oldSection, newSection);
            MoveValue("CompressionColorAndGrayResampleCheckBox.Content", oldSection, newSection);
            MoveValue("CompressionMonochromeCheckBox.Content", oldSection, newSection);
            MoveValue("CompressionMonochromeControl.Header", oldSection, newSection);
            MoveValue("CompressionMonochromeDpiLabel.Content", oldSection, newSection);
            MoveValue("CompressionMonochromeResampleCheckBox.Content", oldSection, newSection);
            MoveValue("ColorModelControl.Header", oldSection, newSection);
            MoveValue("CertificateFileLabel.Content", oldSection, newSection);
            MoveValue("CopyContentPermissionCheckBox.Content", oldSection, newSection);
            MoveValue("DefaultTimeServerButton.Content", oldSection, newSection);
            MoveValue("DisplaySignatureCheckBox.Content", oldSection, newSection);
            MoveValue("DocumentViewLabel.Content", oldSection, newSection);
            MoveValue("EditAssemblyPermissionCheckBox.Content", oldSection, newSection);
            MoveValue("EditCommentsPermissionCheckbox.Content", oldSection, newSection);
            MoveValue("EditDocumentPermissionCheckBox.Content", oldSection, newSection);
            MoveValue("EncryptionLevelControl.Header", oldSection, newSection);
            MoveValue("FillFormsPermissionCheckBox.Content", oldSection, newSection);
            MoveValue("HighEncryptionRadioButton.Content", oldSection, newSection);
            MoveValue("LowEncryptionRadioButton.Content", oldSection, newSection);
            MoveValue("LowQualityPrintPermissionCheckBox.Content", oldSection, newSection);
            MoveValue("MediumEncryptionRadioButton.Content", oldSection, newSection);
            MoveValue("PageOrientationControl.Header", oldSection, newSection);
            MoveValue("PageViewLabel.Content", oldSection, newSection);
            MoveValue("PdfCompressionTab.Header", oldSection, newSection);
            MoveValue("PdfGeneralTab.Header", oldSection, newSection);
            MoveValue("PdfSecurityTab.Header", oldSection, newSection);
            MoveValue("PdfSignatureTab.Header", oldSection, newSection);
            MoveValue("PermissionsControl.Header", oldSection, newSection);
            MoveValue("PrintDocumentPermissionCheckbox.Content", oldSection, newSection);
            MoveValue("ScreenReaderPermissionCheckBox.Content", oldSection, newSection);
            MoveValue("SecuredTimeserverCheckBox.Content", oldSection, newSection);
            MoveValue("SecurityCheckBox.Content", oldSection, newSection);
            MoveValue("SecurityPasswordsButton.Content", oldSection, newSection);
            MoveValue("SignatureCheckBox.Content", oldSection, newSection);
            MoveValue("SignatureContactLabel.Content", oldSection, newSection);
            MoveValue("SignatureLeftXLabel.Content", oldSection, newSection);
            MoveValue("SignatureLeftYLabel.Content", oldSection, newSection);
            MoveValue("SignatureLocationLabel.Content", oldSection, newSection);
            MoveValue("SignaturePageLabel.Content", oldSection, newSection);
            MoveValue("SignaturePasswordButton.Content", oldSection, newSection);
            MoveValue("SignatureReasonLabel.Content", oldSection, newSection);
            MoveValue("SignatureRightXLabel.Content", oldSection, newSection);
            MoveValue("SignatureRightYLabel.Content", oldSection, newSection);
            MoveValue("TimeServerLoginButton.Content", oldSection, newSection);
            MoveValue("TimeServerUrlLabel.Content", oldSection, newSection);
            MoveValue("UserPasswordCheckBox.Content", oldSection, newSection);
            MoveValue("ViewerSettingsControl.Header", oldSection, newSection);
            MoveValue("ViewerStartsOnPageLabel.Content", oldSection, newSection);
        }

        private void CreateActionsTabSection()
        {
            string oldSection = "pdfforge.PDFCreator.Views.ProfileSettingsWindow\\";
            string newSection = "pdfforge.PDFCreator.Shared.Views.UserControls.ActionsTab\\";

            MoveValue("ActionsGridViewColumn.Header", oldSection, newSection);

        }
    }
}
