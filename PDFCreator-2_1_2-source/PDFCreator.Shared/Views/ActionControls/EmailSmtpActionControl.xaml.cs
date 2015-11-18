﻿using System.IO;
using System.Text;
using System.Windows;
using NLog;
using pdfforge.PDFCreator.Core.Actions;
using pdfforge.PDFCreator.Core.Jobs;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.ViewModels;
using pdfforge.PDFCreator.Shared.ViewModels.UserControls;

namespace pdfforge.PDFCreator.Shared.Views.ActionControls
{
    public partial class EmailSmtpActionControl : ActionControl
    {
        public EmailSmtpActionControl()
        {
            InitializeComponent();

            DisplayName = TranslationHelper.Instance.GetTranslation("SmtpEmailActionSettings", "DisplayName", "Send e-mail over SMTP");
            Description = TranslationHelper.Instance.GetTranslation("SmtpEmailActionSettings", "Description",
                    "The SMTP e-mail action allows to directly send files via e-mail without further user interaction. Notice: This action is intended for advanced users and requires careful attention as it can silently send the converted documents via e-mail to the configured recipients.");

            TranslationHelper.Instance.TranslatorInstance.Translate(this);
        }

        public override bool IsActionEnabled
        {
            get
            {
                if (CurrentProfile == null)
                    return false;
                return CurrentProfile.EmailSmtp.Enabled;
            }
            set { CurrentProfile.EmailSmtp.Enabled = value; }
        }

        private EmailSmtp EmailSmtpSettings
        {
            get
            {
                if (DataContext == null)
                    return null;
                return ((ActionsTabViewModel) DataContext).CurrentProfile.EmailSmtp;
            }
        }

        private string Password
        {
            get
            {
                if (DataContext == null)
                    return null;
                return ((ActionsTabViewModel)DataContext).CurrentProfile.EmailSmtp.Password;
            }
            set { ((ActionsTabViewModel)DataContext).CurrentProfile.EmailSmtp.Password = value; }
        }

        private void EditMailTextButton_OnClick(object sender, RoutedEventArgs e)
        {
            var setEmailTextForm = new EditEmailTextWindow(EmailSmtpSettings.AddSignature);
            setEmailTextForm.Subject = EmailSmtpSettings.Subject;
            setEmailTextForm.Body = EmailSmtpSettings.Content;

            if (setEmailTextForm.ShowDialog() == true)
            {
                EmailSmtpSettings.Subject = setEmailTextForm.Subject;
                EmailSmtpSettings.Content = setEmailTextForm.Body;
                EmailSmtpSettings.AddSignature = setEmailTextForm.AddSignature;
            }
        }

        private void SetPasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pwWindow = new SmtpPasswordWindow(SmtpPasswordMiddleButton.Remove);
            pwWindow.SmtpPassword = Password;

            pwWindow.ShowDialogTopMost();

            if (pwWindow.Response == SmtpPasswordResponse.OK)
            {
                Password = pwWindow.SmtpPassword;
            }
            else if (pwWindow.Response == SmtpPasswordResponse.Remove)
            {
                Password = "";
            }
        }

        private void SendTestMailButton_OnClick(object sender, RoutedEventArgs e)
        {
            var smtpMailAction = new SmtpMailAction(MailSignatureHelper.ComposeMailSignature(CurrentProfile.EmailSmtp));

            var currentProfile = ((ActionsTabViewModel)DataContext).CurrentProfile.Copy();

            #region check profile
            var result = smtpMailAction.Check(currentProfile);
            if (!result.Success)
            {
                var caption = TranslationHelper.Instance.GetTranslation("SmtpEmailActionSettings", "SendTestMail", "Send test mail");
                var message = ErrorCodeInterpreter.GetFirstErrorText(result, true);
                MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Error);
                return;
            }
            #endregion

            #region create job
            string tempFolder = Path.GetTempPath();
            string tmpTestFolder = Path.Combine(tempFolder, "PdfCreatorTest\\SendSmtpTestmail");
            Directory.CreateDirectory(tmpTestFolder);
            string tmpInfFile = Path.Combine(tmpTestFolder, "SmtpTest.inf");

            var sb = new StringBuilder();
            sb.AppendLine("[1]");
            sb.AppendLine("SessionId=1");
            sb.AppendLine("WinStation=Console");
            sb.AppendLine("UserName=SampleUser1234");
            sb.AppendLine("ClientComputer=\\PC1");
            sb.AppendLine("PrinterName=PDFCreator");
            sb.AppendLine("JobId=1");
            sb.AppendLine("DocumentTitle=SmtpTest");
            sb.AppendLine("");

            File.WriteAllText(tmpInfFile, sb.ToString(), Encoding.GetEncoding("Unicode"));

            JobTranslations jobTranslations = new JobTranslations();
            jobTranslations.EmailSignature = MailSignatureHelper.ComposeMailSignature(true);

            var job = new GhostscriptJob(new JobInfo(tmpInfFile), new ConversionProfile(), jobTranslations);

            job.Profile = currentProfile;
            #endregion

            #region add password
            if (string.IsNullOrEmpty(Password))
            {
                var pwWindow = new SmtpPasswordWindow(SmtpPasswordMiddleButton.None, currentProfile.EmailSmtp.Address, currentProfile.EmailSmtp.Recipients);
                if (pwWindow.ShowDialogTopMost() != SmtpPasswordResponse.OK)
                {
                    Directory.Delete(tmpTestFolder, true);
                    return;
                }
                job.Passwords.SmtpPassword = pwWindow.SmtpPassword;
            }
            else
            {
                job.Passwords.SmtpPassword = Password;
            }
            #endregion

            #region add testfile
            string testFile = (Path.Combine(tmpTestFolder, "testfile.txt"));
            File.WriteAllText(testFile, @"PDFCreator", Encoding.GetEncoding("Unicode"));
            job.OutputFiles.Add(testFile);
            #endregion

            LogManager.GetCurrentClassLogger().Info("Send test mail over smtp.");
            result = smtpMailAction.ProcessJob(job);
            Directory.Delete(tmpTestFolder, true);

            if (!result.Success)
            {
                var caption = TranslationHelper.Instance.GetTranslation("SmtpEmailActionSettings", "SendTestMail",
                    "Send test mail");
                var message = ErrorCodeInterpreter.GetFirstErrorText(result, true);
                MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Error);
            }
            else
            {
                var caption = TranslationHelper.Instance.GetTranslation("SmtpEmailActionSettings", "SendTestMail", "Send test mail");
                var message = TranslationHelper.Instance.GetFormattedTranslation("SmtpEmailActionSettings", "TestMailSent",
                    "Test mail sent to {0}.", job.Profile.EmailSmtp.Recipients);
                MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Info);
            }
        }
    }
}
