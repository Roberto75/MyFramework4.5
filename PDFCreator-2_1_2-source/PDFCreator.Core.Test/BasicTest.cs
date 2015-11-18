using System.IO;
using iTextSharp.text.pdf;
using NUnit.Framework;
using PDFCreator.TestUtilities;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace PDFCreator.Core.Test
{
    [TestFixture]
    [Category("LongRunning")]
    public class BasicTest
    {
        private TestHelper _th;

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("BasicTest");
        }
        
        [TearDown]
        public void CleanUp()
        {
            if (_th != null)
                _th.CleanUp();
        }

        private void CheckOutputFiles()
        {
            foreach (string file in _th.Job.OutputFiles)
            {
                Assert.IsTrue(File.Exists(file), "Output file '" + file + "' does not exist!");
                FileInfo fi = new FileInfo(file);
                Assert.IsTrue(fi.Length > 0, "Output file '" + file + "' is empty!");
            }
        }

        [Test]
        public void TestSinglePagePdf()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            _th.RunGsJob();

            CheckOutputFiles();
            
            var pdf = new PdfReader(_th.Job.OutputFiles[0]);
            Assert.AreEqual(1, pdf.NumberOfPages, "Number of output pages is incorrect");   
        }

        [Test]
        public void TestManyPagesFromOnePsFile_toPdf()
        {
            _th.GenerateGsJob(PSfiles.ElevenTextPages, OutputFormat.Pdf);

            _th.RunGsJob();

            CheckOutputFiles();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);

            Assert.AreEqual(11, pdf.NumberOfPages, "Number of output pages is incorrect");
        }

        [Test]
        public void TestManyPagesBy128PsFiles_toPdf()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            var sourceFile = _th.JobInfo.SourceFiles[0];
            for(var i =0; i<127; i++)
            {
                _th.JobInfo.SourceFiles.Add(sourceFile);
            }
            _th.JobInfo.SaveInf();

            _th.RunGsJob();

            CheckOutputFiles();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);
            Assert.AreEqual(128, pdf.NumberOfPages, "Number of output pages is incorrect");
        }

        [Test]
        public void TestManyPagesBy129PsFiles_toPdf()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            var sourceFile = _th.JobInfo.SourceFiles[0];
            for (var i = 0; i < 128; i++)
            {
                _th.JobInfo.SourceFiles.Add(sourceFile);
            }
            _th.JobInfo.SaveInf();

            _th.RunGsJob();

            CheckOutputFiles();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);
            Assert.AreEqual(129, pdf.NumberOfPages, "Number of output pages is incorrect");
        }

        /*
        [Test]
        public void TestManyPagesBy3000PsFiles_toPdf()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);

            var sourceFile = _th.JobInfo.SourceFiles[0];
            for (var i = 0; i < 3000; i++)
            {
                _th.JobInfo.SourceFiles.Add(sourceFile);
            }
            _th.JobInfo.SaveInf();

            _th.RunGsJob();

            CheckOutputFiles();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);
            Assert.AreEqual(3000, pdf.NumberOfPages, "Number of output pages is incorrect");
        }
        */

        [Test]
        public void Test_DefaultPdfVersionIs9_15()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);
            _th.RunGsJob();

            CheckOutputFiles();

            var pdfReader = new PdfReader(_th.Job.OutputFiles[0]);
            Assert.AreEqual('5', pdfReader.PdfVersion);
        }

        [Test]
        public void TestSinglePagePng()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Png);

            _th.RunGsJob();
            
            CheckOutputFiles();

            Assert.AreEqual(1, _th.Job.OutputFiles.Count, "Number of output pages is incorrect");
        }

        [Test]
        public void TestManyPagesPng()
        {
            _th.GenerateGsJob(PSfiles.ElevenTextPages, OutputFormat.Png);

            _th.RunGsJob();

            CheckOutputFiles();

            Assert.AreEqual(11, _th.Job.OutputFiles.Count, "Number of output pages is incorrect");
        }

        [Test]
        public void TestSinglePageTif()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Tif);

            _th.RunGsJob();

            CheckOutputFiles();

            Assert.AreEqual(1, _th.Job.OutputFiles.Count, "Number of output pages is incorrect");
        }

        [Test]
        public void TestManyPagesTif()
        {
            _th.GenerateGsJob(PSfiles.ElevenTextPages, OutputFormat.Tif);

            _th.RunGsJob();

            CheckOutputFiles();

            Assert.AreEqual(1, _th.Job.OutputFiles.Count, "Number of output pages is incorrect");
        }

        [Test]
        public void TestSinglePageJpeg()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Jpeg);

            _th.RunGsJob();

            CheckOutputFiles();

            Assert.AreEqual(1, _th.Job.OutputFiles.Count, "Number of output pages is incorrect");
        }

        [Test]
        public void TestManyPagesJpeg()
        {
            _th.GenerateGsJob(PSfiles.ElevenTextPages, OutputFormat.Jpeg);

            _th.RunGsJob();

            CheckOutputFiles();

            Assert.AreEqual(11, _th.Job.OutputFiles.Count, "Number of output pages is incorrect");
        }

        [Test]
        public void TestSpecialCharactersPdf()
        {
            _th.GenerateGsJob(PSfiles.ElevenTextPages, OutputFormat.Pdf);
            
            _th.SetFilenameTemplate("Täßt#Filênám€ 他们她们它们 вѣдѣ вѣди.pdf");
            
            _th.RunGsJob();

            CheckOutputFiles();

            Assert.AreEqual(1, _th.Job.OutputFiles.Count, "Number of output pages is incorrect");
        }

        [Test]
        public void TestSpecialCharactersJpeg()
        {
            _th.GenerateGsJob(PSfiles.ElevenTextPages, OutputFormat.Jpeg);
            _th.SetFilenameTemplate("Täßt#Filênám€ 他们她们它们 вѣдѣ вѣди.jpg");
            _th.RunGsJob();

            CheckOutputFiles();

            Assert.AreEqual(11, _th.Job.OutputFiles.Count, "Number of output pages is incorrect");
        }
    }
}
