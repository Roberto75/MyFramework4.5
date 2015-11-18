using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using My.MessageQueue.Models;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestAttachment
    {
        [TestMethod]
        public void GetAttachment()
        {

            My.MessageQueue.MessaggeQueueManager manager = new My.MessageQueue.MessaggeQueueManager("DefaultConnection");

            try
            {
                manager.openConnection();


               Attachment attachment = manager.getAttachment(1);

               if (attachment == null) {
                   Assert.Fail();
               }


               manager.saveAttachment(attachment, @"C:\Temp\" + "Copy of " + attachment.name);


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                manager.closeConnection();
            }


        }
    }
}
