using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class TestPhantomjs
    {
        [TestMethod]
        public void TestMethod1()
        {
            My.Phantomjs.Phantomjs phantom = new My.Phantomjs.Phantomjs("c:\\PortableApps\\phantomjs-1.9.8-windows\\phantomjs.exe" 
                , "C:\\Develop.NET\\EarlyWarning\\public\\Implementazione\\Ver2\\WebPortal\\WebPortal\\Content\\Images");


            My.Phantomjs.Models.Task task = new My.Phantomjs.Models.Task ();
            task.TaskType = My.Phantomjs.Phantomjs.EnumTaskType.Rasterize;
            task.fileName = "report01.png";
            task.Url = new Uri ("http://localhost/EarlyWarning/Report/top10VulnerableProduct");

            phantom.process(task);


        }
    }
}
