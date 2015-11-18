using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Threading;
using ImapX;
using NUnit.Framework;
using PDFCreator.TestUtilities;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace PDFCreator.Core.Test.Actions
{
    [TestFixture]
    [Category("LongRunning")]
    class SmtpEmailTest
    {
        private TestHelper _th;

        private readonly string _mailServer = ParameterHelper.GetPassword("smtp_server");
        private readonly string _userName = ParameterHelper.GetPassword("smtp_user");
        private readonly string _eMailAddress = ParameterHelper.GetPassword("smtp_email");
        private readonly string _smtpPassword = ParameterHelper.GetPassword("smtp_password");
        private readonly int _smtpPort = int.Parse(ParameterHelper.GetPassword("smtp_port"));

        private readonly string _imapServer = ParameterHelper.GetPassword("imap_server");
        private readonly string _imapUserName = ParameterHelper.GetPassword("imap_user");
        private readonly string _imapPassword = ParameterHelper.GetPassword("imap_password");
        private readonly int _imapPort = int.Parse(ParameterHelper.GetPassword("imap_port"));

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("SmtpEmailTest");
        }

        [TearDown]
        public void CleanUp()
        {
            _th.CleanUp();
        }

        private void InitTest()
        {
            _th.Profile.EmailSmtp.Enabled = true;
            _th.Profile.EmailSmtp.UserName = _userName;
            _th.Profile.EmailSmtp.Address = _eMailAddress;
            _th.Profile.EmailSmtp.Server = _mailServer;
            _th.Profile.EmailSmtp.Port = _smtpPort;
            _th.Profile.EmailSmtp.Ssl = true;
            _th.Profile.EmailSmtp.Recipients = _eMailAddress;
        }

        [Test]
        public void OnePdfFileAttached() 
        {
            InitTest();
            _th.Profile.EmailSmtp.Subject = "One Pdf-File attached";
            _th.Profile.EmailSmtp.Content = "The following files are attached: <OutputFilenames:\r\n>";
            _th.Profile.EmailSmtp.AddSignature = false;

            _th.Profile.OutputFormat = OutputFormat.Pdf;

            TestSmtpEmail("");
        }

        [Test]
        public void ThreeJpegFilesAttached()
        {
            InitTest();
            _th.Profile.EmailSmtp.Subject = "Three Jpeg-Files attached";
            _th.Profile.EmailSmtp.Content = "The following files are attached: <OutputFilenames:\r\n>";
            _th.Profile.EmailSmtp.AddSignature = false;

            _th.Profile.OutputFormat = OutputFormat.Jpeg;

            TestSmtpEmail("");
        }

        private ImapClient CreateImapClient()
        {
            return new ImapClient(_imapServer, _imapPort, true, SslProtocols.Tls);
        }
        
        private void TestSmtpEmail(string outputFilenameTemplate)
        {
            #region Delete Mails in Inbox
            var imapClient = CreateImapClient();
            Assert.IsTrue(imapClient.Connection(), "Could not connect to Imap");
            Assert.IsTrue(imapClient.LogIn(_imapUserName, _imapPassword), "Could no login to Imap");
            foreach (Message msg in imapClient.Folders["INBOX"].Messages)
                imapClient.Folders["INBOX"].DeleteMessage(msg);
            imapClient.LogOut();
            imapClient.Disconnect();
            #endregion

            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, _th.Profile.OutputFormat);
            _th.Job.Passwords.SmtpPassword = _smtpPassword;
            if (!String.IsNullOrEmpty(outputFilenameTemplate))
            {
                _th.SetFilenameTemplate(outputFilenameTemplate + Path.GetExtension(_th.Job.OutputFilenameTemplate));
            }
            _th.RunGsJob();

            #region Wait for new Mail in Inbox
            imapClient = CreateImapClient();
            Assert.IsTrue(imapClient.Connection(), "Could not connect to Imap");
            Assert.IsTrue(imapClient.LogIn(_imapUserName, _imapPassword), "Could no login to Imap-Server");
            int i = 0;
            while ((imapClient.Folders["INBOX"].Messages.Count == 0))
            {
                Thread.Sleep(2000);
                imapClient = CreateImapClient();
                Assert.IsTrue(imapClient.Connection(), "Could not connect to Imap");
                Assert.IsTrue(imapClient.LogIn(_imapUserName, _imapPassword), "Could no login to Imap-Server");
                Assert.Less(i++, 60, "Mail did not arrive within 2 Minutes");
            }
            #endregion

            Message mail = imapClient.Folders["INBOX"].Messages[0];
            
            mail.Process();
    
            Assert.Less(mail.From.Count, 2, "Email has more than one sender");
            Assert.AreEqual(_th.Profile.EmailSmtp.Address, mail.From[0].Address, "Wrong sender");
                    
            Assert.AreEqual(_th.Job.TokenReplacer.ReplaceTokens(_th.Profile.EmailSmtp.Subject), mail.Subject.Replace("\n ", ""), "Incorrect mail-subject");
                
            string mailContent = mail.TextBody.ContentStream;
            if (mail.TextBody.ContentTransferEncoding == "base64")
                mailContent = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(mailContent));

            Assert.AreEqual(_th.Job.TokenReplacer.ReplaceTokens(_th.Profile.EmailSmtp.Content),
                mailContent.Replace("=\r\n", "").Replace("=0A", "\n").Replace("=0D", "\r"), "Incorrect mail-content");
                    
            List<string> recipientsList = new List<string>(_th.Profile.EmailSmtp.Recipients.Trim().Replace(";", ",").Split(','));
            List<string> recipientsMail = new List<string>(mail.To[0].Address.Trim().Replace("\n", "").Split(','));
            Assert.AreEqual(recipientsList, recipientsMail, "Not all the recipients were added in mail");

            Assert.AreEqual(_th.Job.OutputFiles.Count, mail.Attachments.Count, "Incorrect number of attached files");
            for (int j = 0; j < _th.Job.OutputFiles.Count; j++)
            {
                Assert.AreEqual(mail.Attachments[j].FileData, File.ReadAllBytes(_th.Job.OutputFiles[j]), "Data of " + _th.Job.OutputFiles[j] + " has changed");
            }
            for (int j = 0; j < _th.Job.OutputFiles.Count; j++)
            {
                Assert.AreEqual(mail.Attachments[j].FileName, Path.GetFileName(_th.Job.OutputFiles[j]), "Name of " + _th.Job.OutputFiles[j] + " has changed. Data was identical.");
            }

               
            imapClient.LogOut();
            imapClient.Disconnect();
        }
    }
}