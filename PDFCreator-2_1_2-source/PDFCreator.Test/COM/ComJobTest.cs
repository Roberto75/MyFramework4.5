using System;
using System.Runtime.InteropServices;
using NUnit.Framework;
using pdfforge.PDFCreator;
using pdfforge.PDFCreator.COM;

namespace PDFCreator.Test.COM
{
    /* REWROTE THE TESTS */
    [TestFixture]
    class ComJobTest
    {
        private Queue _queue;

        private void CreateTestPages(int n)
        {
            for (int i = 0; i < n; i++)
            {
                JobInfoQueue.Instance.AddTestPage();
            }
        }

        [SetUp]
        public void SetUp()
        {
            _queue = new Queue();
            _queue.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            _queue.ReleaseCom();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTo_IfFilenameEmpty_ThrowsArgumentException()
        {
            CreateTestPages(1);
            
            const string filename = "";
            PrintJob comJob = _queue.NextJob;

            comJob.ConvertTo(filename);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTo_IfFilenameHasIllegalChars_ThrowsArgumentException()
        {
            CreateTestPages(1);

            const string filename = "testpage>testpage.pdf";
            PrintJob comJob = _queue.NextJob;

            comJob.ConvertTo(filename);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertTo_IfFilenameHasIllegalExtension_ThrowsArgumentException()
        {
            CreateTestPages(1);

            const string filename = "testpage\testpage.adfsd";
            var comJob = _queue.NextJob;

            comJob.ConvertTo(filename);
        }

        [Test]
        [ExpectedException(typeof(COMException))]
        public void ConvertTo_IfFilenameDirectoryNotExisting_ThrowsCOMException()
        {
            CreateTestPages(1);

            const string filename = "basdeead\\aokdeaad.pdf";
            var comJob = _queue.NextJob;

            comJob.ConvertTo(filename);
        }

        [Test]
        [ExpectedException(typeof(COMException))]
        public void ProfileSettings_IfEmptyPropertyname_ThrowsCOMException()
        {
            CreateTestPages(1);

            var comJob = _queue.NextJob;
            comJob.SetProfileSetting("","True");
        }

        [Test]
        [ExpectedException(typeof(COMException))]
        public void ProfileSettings_IfNotExistingPropertyname_ThrowsCOMException()
        {
            CreateTestPages(1);

            var comJob = _queue.NextJob;
            comJob.SetProfileSetting("NotExisting", "True");
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void SetProfileSettings_IfEmptyValue_ThrowsCOMException()
        {
            CreateTestPages(1);

            var comJob = _queue.NextJob;
            comJob.SetProfileSetting("PdfSettings.Security.Enabled", "");
        }

        [Test]
        [ExpectedException(typeof (FormatException))]
        public void SetProfileSettings_IfInappropriateValue_ThrowsCOMException()
        {
            CreateTestPages(1);

            var comJob = _queue.NextJob;
            comJob.SetProfileSetting("PdfSettings.Security.Enabled","3");
        }

        [Test]
        [ExpectedException(typeof(COMException))]
        public void GetProfileSettings_IfEmptyPropertyname_ThrowsCOMException()
        {
            CreateTestPages(1);

            var comJob = _queue.NextJob;
            comJob.GetProfileSetting("");
        }

        [Test]
        [ExpectedException(typeof(COMException))]
        public void GetProfileSettings_IfInvalidPropertyname_ThrowsCOMException()
        {
            CreateTestPages(1);

            var comJob = _queue.NextJob;
            comJob.GetProfileSetting("asdioajsd");
        }
    }
}
