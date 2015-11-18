using NUnit.Framework;
using pdfforge.PDFCreator.Startup;

namespace PDFCreator.Test.Startup
{
    [TestFixture]
    public class AppStartFactoryTest
    {
        [Test]
        public void Called_WithEmptyCommandLine_ReturnsMainWindowStartUp()
        {
            string[] args = {};

            AppStartFactory appStartFactory = new AppStartFactory();
            var appStart = appStartFactory.CreateApplicationStart(args);

            Assert.IsAssignableFrom<MainWindowStart>(appStart);
        }

        [Test]
        public void Called_WithPrintFileParameter_ReturnsMainWindowStartUp()
        {
            string[] args = {"/PrintFile=C:\\Test.txt"};

            AppStartFactory appStartFactory = new AppStartFactory();
            var appStart = appStartFactory.CreateApplicationStart(args);

            Assert.IsAssignableFrom<PrintFileStart>(appStart);
        }

        [Test]
        public void Called_WithManagePrintJobs_ReturnsMainWindowStartUp()
        {
            string[] args = {"/ManagePrintJobs"};

            AppStartFactory appStartFactory = new AppStartFactory();
            var appStart = appStartFactory.CreateApplicationStart(args);

            MaybePipedStart maybePipedStart = appStart as MaybePipedStart;

            Assert.IsTrue(maybePipedStart.StartManagePrintJobs);
        }

        [Test]
        public void Called_WithJobInfo_ReturnsNewPrintJobStart()
        {
            string jobFile = @"C:\test.inf";
            string[] args = { string.Format("/InfoDataFile={0}", jobFile) };

            AppStartFactory appStartFactory = new AppStartFactory();
            var appStart = appStartFactory.CreateApplicationStart(args);

            Assert.IsAssignableFrom<NewPrintJobStart>(appStart);
            Assert.AreEqual(jobFile, ((NewPrintJobStart)appStart).NewJobInfoFile);
        }

        [Test]
        public void Called_WithJobInfoAndManagePrintJobs_ReturnsNewPrintJobStartWithNewPrintJobStart()
        {
            string jobFile = @"C:\test.inf";
            string[] args = { string.Format("/InfoDataFile={0}", jobFile), "/ManagePrintJobs" };

            AppStartFactory appStartFactory = new AppStartFactory();
            var appStart = appStartFactory.CreateApplicationStart(args);

            Assert.IsAssignableFrom<NewPrintJobStart>(appStart);
            Assert.IsTrue(((NewPrintJobStart)appStart).StartManagePrintJobs);
        }
    }
}
