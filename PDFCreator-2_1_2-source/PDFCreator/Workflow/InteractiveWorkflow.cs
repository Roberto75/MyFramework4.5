using System;
using System.IO;
using System.Windows.Forms;
using NLog;
using pdfforge.PDFCreator.Core;
using pdfforge.PDFCreator.Core.Actions;
using pdfforge.PDFCreator.Core.Jobs;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Exceptions;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.ViewModels;
using pdfforge.PDFCreator.Shared.Views;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.ViewModels;
using pdfforge.PDFCreator.Views;

namespace pdfforge.PDFCreator.Workflow
{
    /// <summary>
    ///     The interactive workflow implements the workflow steps where user interaction is required.
    /// </summary>
    public class InteractiveWorkflow : ConversionWorkflow
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TranslationHelper _translationHelper = TranslationHelper.Instance;

        /// <summary>
        ///     Create a new Workflow object with the given job info
        /// </summary>
        /// <param name="job">Job to use for the conversion</param>
        /// <param name="settings">Settigns to use during the conversion workflow</param>
        public InteractiveWorkflow(IJob job, PdfCreatorSettings settings)
        {
            WorkflowStep = WorkflowStep.Init;

            job.OnActionAdded += job_OnActionAdded;

            JobInfo = job.JobInfo;
            Job = job;
            Settings = settings;
        }

        protected override void QueryTargetFile()
        {
            if (!Job.Profile.SkipPrintDialog)
            {
                Job.ApplyMetadata();
                var w = new PrintJobWindow();
                var model = new PrintJobViewModel(Job.JobInfo, Job.Profile);
                w.DataContext = model;

                if ((TopMostHelper.ShowDialogTopMost(w, true) != true) || model.PrintJobAction == PrintJobAction.Cancel)
                {
                    Cancel = true;
                    WorkflowStep = WorkflowStep.AbortedByUser;
                    return;
                }

                if (model.PrintJobAction == PrintJobAction.ManagePrintJobs)
                    throw new ManagePrintJobsException();

                Job.Profile = model.SelectedProfile.Copy();
                Job.ApplyMetadata();

                if (model.PrintJobAction == PrintJobAction.EMail)
                {
                    Job.SkipSaveFileDialog = false;
                    Job.Profile.EmailClient.Enabled = true;
                    Job.Profile.AutoSave.Enabled = false;
                    Job.Profile.OpenViewer = false;
                }
            }

            if (Job.SkipSaveFileDialog)
            {
                string sendFilesFolder = Job.JobTempFolder + "_SendFiles";
                Directory.CreateDirectory(sendFilesFolder);
                string filePath = Path.Combine(sendFilesFolder, Job.ComposeOutputFilename());
                filePath = FileUtil.EllipsisForTooLongPath(filePath);
                Job.OutputFilenameTemplate = filePath;
            }
            else
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = _translationHelper.GetTranslation("InteractiveWorkflow", "SelectDestination", "Select destination");
                saveFileDialog.Filter = _translationHelper.GetTranslation("InteractiveWorkflow", "PdfFile", "PDF file") +
                                        @" (*.pdf)|*.pdf";
                saveFileDialog.Filter += @"|" +
                                            _translationHelper.GetTranslation("InteractiveWorkflow", "PdfA1bFile", "PDF/A-1b file") +
                                            @" (*.pdf)|*.pdf";
                saveFileDialog.Filter += @"|" +
                                            _translationHelper.GetTranslation("InteractiveWorkflow", "PdfA2bFile", "PDF/A-2b file") +
                                            @" (*.pdf)|*.pdf";
                saveFileDialog.Filter += @"|" +
                                            _translationHelper.GetTranslation("InteractiveWorkflow", "PdfXFile", "PDF/X file") +
                                            @" (*.pdf)|*.pdf";
                saveFileDialog.Filter += @"|" +
                                            _translationHelper.GetTranslation("InteractiveWorkflow", "JpegFile", "JPEG file") +
                                            @" (*.jpg)|*.jpg;*.jpeg;";
                saveFileDialog.Filter += @"|" +
                                            _translationHelper.GetTranslation("InteractiveWorkflow", "PngFile", "PNG file") +
                                            @" (*.png)|*.png;";
                saveFileDialog.Filter += @"|" +
                                            _translationHelper.GetTranslation("InteractiveWorkflow", "TiffFile", "TIFF file") +
                                            @" (*.tif)|*.tif;*.tiff";

                saveFileDialog.FilterIndex = ((int)Job.Profile.OutputFormat) + 1;
                saveFileDialog.OverwritePrompt = true;

                var directoryWasCreated = false;
                var directory = ""; 

                if (Job.Profile.SaveDialog.SetDirectory)
                {
                    directory = FileUtil.MakeValidFolderName(Job.TokenReplacer.ReplaceTokens(Job.Profile.SaveDialog.Folder));

                    try
                    {
                        if (!Directory.Exists(directory))
                        {
                            _logger.Trace("The preselected save dialog folder must be created");
                            Directory.CreateDirectory(directory);
                            directoryWasCreated = true; //flag to delete the directory, if it stays empty
                            _logger.Info("Created folder for savedialog: " + directory);
                        }

                        saveFileDialog.RestoreDirectory = true;
                        saveFileDialog.InitialDirectory = directory;
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn("Exception while creating folder for save dialog \"" + directory 
                            + "\".\r\nSave dialog will be opened with default save location.\r\n" + ex.Message);
                    } 
                }

                //saveFileDialog.FileOk += SaveFileDialog_FileOk; 

                Cancel = !LaunchSaveFileDialog(saveFileDialog);

                if (Cancel)
                {
                    WorkflowStep = WorkflowStep.AbortedByUser;
                    //Delete Directory if it was just created and is empty
                    if (directoryWasCreated && (Directory.GetFiles(directory).Length < 1))
                    {
                        Directory.Delete(directory);
                        _logger.Info("Deleted created directory \"" + directory +"\" , because it was empty.");
                    }
                }
            }
        }

        /// <summary>
        /// Sets the job's filenametemplate and extension by savefiledialog.
        /// Recursive call of the savefile dialog if filename (+path) is to long.
        /// </summary>
        /// <param name="saveFileDialog">saveFileDialiog</param>
        /// <returns>false if user cancels savefiledialog</returns>
        private bool LaunchSaveFileDialog(SaveFileDialog saveFileDialog)
        {
            string tmpFile = Job.ComposeOutputFilename();

            saveFileDialog.FileName = tmpFile;

            var result = TopMostHelper.ShowDialogTopMost(saveFileDialog, !Job.Profile.SkipPrintDialog);

            if (result != DialogResult.OK)
            {
                _logger.Warn("Cancelled the save dialog. No PDF will be created.");
                WorkflowStep = WorkflowStep.AbortedByUser;
                return false;
            }

            Job.Profile.OutputFormat = (OutputFormat)saveFileDialog.FilterIndex - 1;

            try {
                string outputFile = saveFileDialog.FileName;
                if (!OutputFormatHelper.HasValidExtension(outputFile, Job.Profile.OutputFormat))
                    outputFile = OutputFormatHelper.AddValidExtension(outputFile, Job.Profile.OutputFormat);

                Job.OutputFilenameTemplate = outputFile;
                return true;
            }
            catch (PathTooLongException)
            {
                _logger.Error("Filename (+ path) from savefile dialog is too long.");
                string message = _translationHelper.GetTranslation("InteractiveWorkflow", "SelectedPathTooLong", "The total length of path and filename is too long.\r\nPlease use a shorter name.");
                string caption = _translationHelper.GetTranslation("InteractiveWorkflow", "SelectDestination", "Select destination");
                MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Warning);
                return LaunchSaveFileDialog(saveFileDialog);
            }
        }

        /*private static void SaveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var sfd = (SaveFileDialog)sender;
            //string file = sfd.FileName;
            string ext = ((OutputFormat)sfd.FilterIndex - 1).ToString();
            
            string file = Path.ChangeExtension(Path.GetFullPath(sfd.FileName), ext);

            if (File.Exists(file))
            {
                try
                {
                    var dr = MessageBox.Show(Path.GetFileName(file) + @"does already exist."
                        + @"Do You want to replace it?", @"Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    //Translate me!!!!!!!!
                    if (dr != DialogResult.Yes)
                        e.Cancel = true;
                }
                catch (Exception ex)
                {
                    //_logger.Error(ex);

                }
            }
        }//*/

        protected override bool QueryEncryptionPasswords()
        {
            Job.Passwords.PdfOwnerPassword = Job.Profile.PdfSettings.Security.OwnerPassword;
            Job.Passwords.PdfUserPassword = Job.Profile.PdfSettings.Security.UserPassword;

            bool askOwnerPw = string.IsNullOrEmpty(Job.Profile.PdfSettings.Security.OwnerPassword);
            bool askUserPw = Job.Profile.PdfSettings.Security.RequireUserPassword &&
                             string.IsNullOrEmpty(Job.Profile.PdfSettings.Security.UserPassword);

            if (askOwnerPw || askUserPw) //overwrite values with value from form
            {
                var pwWindow = new EncryptionPasswordsWindow(EncryptionPasswordMiddleButton.Skip, askOwnerPw, askUserPw);

                pwWindow.ShowDialogTopMost();

                if (pwWindow.Response == EncryptionPasswordResponse.OK)
                {
                    if (askOwnerPw)
                        Job.Passwords.PdfOwnerPassword = pwWindow.OwnerPassword;
                    if (askUserPw)
                        Job.Passwords.PdfUserPassword = pwWindow.UserPassword;
                }
                else if (pwWindow.Response == EncryptionPasswordResponse.Skip)
                {
                    Job.Profile.PdfSettings.Security.Enabled = false;
                    _logger.Info("User skipped encryption password dialog. Encryption disabled.");
                    return true;
                }
                else
                {
                    Cancel = true;
                    _logger.Warn("Cancelled the PDF password dialog. No PDF will be created.");
                    WorkflowStep = WorkflowStep.AbortedByUser;
                    return false;
                }
            }

            return true;
        }

        protected override bool QuerySignaturePassword()
        {
            if (!string.IsNullOrEmpty(Job.Profile.PdfSettings.Signature.SignaturePassword))
            {
                Job.Passwords.PdfSignaturePassword = Job.Profile.PdfSettings.Signature.SignaturePassword;
                return true;
            }

            var passwordWindow = new SignaturePasswordWindow(PasswordMiddleButton.Skip, Job.Profile.PdfSettings.Signature.CertificateFile);

            TopMostHelper.ShowDialogTopMost(passwordWindow, true);

            if (passwordWindow.SignaturePasswordViewModel.Result == SignaturePasswordResult.StorePassword)
            {
                Job.Passwords.PdfSignaturePassword = passwordWindow.Password;
                return true;
            }
            if (passwordWindow.SignaturePasswordViewModel.Result == SignaturePasswordResult.Skip)
            {
                Job.Profile.PdfSettings.Signature.Enabled = false;
                _logger.Info("User skipped Signature Password. Signing disabled.");
                return true;
            }

            Cancel = true;
            _logger.Warn("Cancelled the signature password dialog. No PDF will be created.");
            WorkflowStep = WorkflowStep.AbortedByUser;
            return false;
        }

        protected override bool QueryEmailSmtpPassword()
        {
            if (!string.IsNullOrEmpty(Job.Profile.EmailSmtp.Password))
            {
                Job.Passwords.SmtpPassword = Job.Profile.EmailSmtp.Password;
                return true;
            }

            var pwWindow = new SmtpPasswordWindow(SmtpPasswordMiddleButton.Skip, Job.Profile.EmailSmtp.Address, Job.Profile.EmailSmtp.Recipients);
            
            pwWindow.ShowDialogTopMost();
            
            if (pwWindow.Response == SmtpPasswordResponse.OK)
            {
                Job.Passwords.SmtpPassword = pwWindow.SmtpPassword;
                return true;
            }
            if (pwWindow.Response == SmtpPasswordResponse.Skip)
            {
                Job.Profile.EmailSmtp.Enabled = false;
                _logger.Info("User skipped Smtp Password. Smtp Email disabled.");
                return true;
            }
            Cancel = true;
            _logger.Warn("Cancelled the SMTP dialog. No PDF will be created.");
            WorkflowStep = WorkflowStep.AbortedByUser;
            return false;
        }

        protected override bool QueryFtpPassword()
        {
            if (!string.IsNullOrEmpty(Job.Profile.Ftp.Password))
            {
                Job.Passwords.FtpPassword = Job.Profile.Ftp.Password;
                return true;
            }

            var pwWindow = new FtpPasswordWindow(FtpPasswordMiddleButton.Skip);
            pwWindow.ShowDialogTopMost();

            if (pwWindow.Response == FtpPasswordResponse.OK)
            {
                Job.Passwords.FtpPassword = pwWindow.FtpPassword;
                return true;
            }
            if (pwWindow.Response == FtpPasswordResponse.Skip)
            {
                Job.Profile.PdfSettings.Signature.Enabled = false;
                _logger.Info("User skipped ftp password. Ftp upload disabled.");
                return true;
            }
            Cancel = true;
            _logger.Warn("Cancelled the FTP password dialog. No PDF will be created.");
            WorkflowStep = WorkflowStep.AbortedByUser;
            return false;
        }

        protected override void NotifyUserAboutFailedJob()
        {
            string errorText;

            switch (Job.ErrorType)
            {
                case JobError.Ghostscript:
                    errorText = _translationHelper.GetTranslation("InteractiveWorkflow", "GhostscriptError", "Internal Ghostscript error");
                    break;
                default:
                    errorText = _translationHelper.GetTranslation("InteractiveWorkflow", "UnknownError", "Unkown internal error");
                    break;
            }

            string caption = _translationHelper.GetTranslation("InteractiveWorkflow", "Error", "Error");
            string opener = _translationHelper.GetFormattedTranslation("InteractiveWorkflow", "ErrorWhileConverting", "PDFCreator was not able to convert the document, because an error occured:\r\n{0}\r\n\r\nYou can find additional information in the log file.", errorText);
            
            MessageWindow.ShowTopMost(opener, caption, MessageWindowButtons.OK, MessageWindowIcon.Error);
        }

        protected override bool EvaluateActionResult(ActionResult actionResult)
        {
            if (actionResult.Success)
                return true;
            string caption = _translationHelper.GetTranslation("InteractiveWorkflow", "Error", "Error");
            string opener = _translationHelper.GetTranslation("InteractiveWorkflow", "AnErrorOccured", "An error occured:");
            string errorText = ErrorCodeInterpreter.GetErrorText(actionResult[0], true);
            MessageWindow.ShowTopMost(opener + "\r\n" + errorText, caption, MessageWindowButtons.OK, MessageWindowIcon.Error);

            return actionResult.Success;
        }

        protected override void RetypeSmtpPassword(object sender, QueryPasswordEventArgs e)
        {
            _logger.Debug("Launched E-mail password Form");
            var pwWindow = new SmtpPasswordWindow(SmtpPasswordMiddleButton.None, Job.Profile.EmailSmtp.Address, Job.Profile.EmailSmtp.Recipients);
            pwWindow.SmtpPassword = Job.Passwords.SmtpPassword;
            pwWindow.Message = _translationHelper.GetTranslation("InteractiveWorkflow", "RetypeSmtpPwMessage",
                "Could not authenticate at server.\r\nPlease check your password and verify that you have a working internet connection.");

            if (pwWindow.ShowDialogTopMost() == SmtpPasswordResponse.OK)
            {
                Job.Passwords.SmtpPassword = pwWindow.SmtpPassword;
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                _logger.Warn("Cancelled the SMTP dialog. No PDF will be created.");
                WorkflowStep = WorkflowStep.AbortedByUser;
            }
        }

        protected override void RecommendPdfArchitect(object sender, EventArgs e)
        {
            _logger.Info("Recommend PDF Architect");
            RecommendPdfArchitectWindow.ShowDialogTopMost(true);
        }

        private void job_OnActionAdded(object sender, ActionAddedEventArgs e)
        {
           // code for AttachMe here
            /*if (e.Action is AttachMeAction)
            {
                var attachMe = e.Action as AttachMeAction;
                attachMe.OnAttachmentCreated += attachMe_OnAttachmentCreated;
            }*/
        }
    }
}