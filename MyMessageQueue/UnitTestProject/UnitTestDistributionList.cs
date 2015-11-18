using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using My.MessageQueue.Models;
using System.Diagnostics;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestDistributionList
    {

        private My.MessageQueue.DistributionListManager manager = new My.MessageQueue.DistributionListManager("DefaultConnection");

        [TestMethod]
        public void Create()
        {
            DistributionList list = new DistributionList();

            list.name = "Lista di ditribuzione CERT";
            list.Members.Add(new Member("Roberto Rutigliano", "roberto.rutigliano@techub.it"));
            list.Members.Add(new Member("Marco Cinque", "CINQUE17@posteitaliane.it"));


            bool esito = false;

            try
            {
                manager.openConnection();

                manager.createDistributionList(list);

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
        public void GetDistributionList()
        {
            DistributionList list = null;

            try
            {
                manager.openConnection();

                list = manager.getDistributionList(3);
              
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                manager.closeConnection();
            }

            if (list  == null)
            {
                Assert.Fail();
            }

        }







    }
}
