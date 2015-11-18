using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestEmail
    {
        private My.MessageQueue.MessaggeQueueManager manager = new My.MessageQueue.MessaggeQueueManager("DefaultConnection");


        [TestMethod]
        public void InsertEmail()
        {
            My.MessageQueue.Models.Email mail = new My.MessageQueue.Models.Email();

            mail.To = "roberto.rutigliano@techub.it";

            mail.distributionListId = 3;

            mail.Subject = "My Message Queue";
            mail.Body = "TEST";
            mail.SendType = My.MessageQueue.Models.MessageBase.EnumSendType.Async;

            mail.Attachments.Add(new My.MessageQueue.Models.Attachment(@"C:\temp\whatsnew.xml"));
            mail.Attachments.Add(new My.MessageQueue.Models.Attachment(@"C:\temp\pdf01.pdf"));
            // mail.Attachments.Add(new My.MessageQueue.Models.Attachment(@"C:\temp\pdf02.pdf"));

            bool esito = false;

            try
            {
                manager.openConnection();

                manager.insert(mail);

                esito = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                esito = false;
            }
            finally
            {
                manager.closeConnection();
            }

            if (esito == false)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void DeleteEmail()
        {
            bool esito = false;

            try
            {
                manager.openConnection();

                esito = manager.delete(3);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                esito = false;
            }
            finally
            {
                manager.closeConnection();
            }


            if (esito == false)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void Sendmail()
        {

            My.MessageQueue.MailManager mail = new My.MessageQueue.MailManager("DefaultConnection");

            bool esito = false;

            try
            {
                mail.openConnection();

                esito = mail.send(6);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                esito = false;
            }
            finally
            {
                mail.closeConnection();
            }


            if (esito == false)
            {
                Assert.Fail();
            }

        }

    }
}
