
using System.IO;
using iTextSharp.text.pdf;
using NUnit.Framework;
using PDFCreator.TestUtilities;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Workflow;

namespace PDFCreator.Test
{
    [TestFixture]
    [Category("LongRunning")]
    class ConversionWorkflowTest
    {
        private TestHelper _th;
        private PdfCreatorSettings _settings;

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("ConversionWorklowTest");
        }

        [TearDown]
        public void CleanUp()
        {
            if (_th != null)
                _th.CleanUp();
        }

        [Test]
        public void TitleTemplateTest()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);
            _th.JobInfo.Metadata.PrintJobName = "Title from Job";
            _th.JobInfo.Metadata.PrintJobAuthor = "Author from Job";

            _th.Job.Profile.AutoSave.Enabled = true;

            _settings = new PdfCreatorSettings(null);
            _settings.ConversionProfiles.Add(_th.Job.Profile);
            _settings.ConversionProfiles[0].AutoSave.EnsureUniqueFilenames = false;
            _settings.ConversionProfiles[0].AutoSave.TargetDirectory = _th.TmpTestFolder;
            _settings.ConversionProfiles[0].FileNameTemplate = "AutoSaveTestOutput";
            _settings.ConversionProfiles[0].OutputFormat = OutputFormat.Pdf;
            _settings.ConversionProfiles[0].TitleTemplate = "<PrintJobName> by <PrintJobAuthor>";

            var autoSaveWorkflow = new AutoSaveWorkflow(_th.Job, _settings);

            autoSaveWorkflow.RunWorkflow();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);
            Assert.AreEqual("Title from Job by Author from Job", pdf.Info["Title"], "Wrong title in PDF Metadata");
        }

        [Test]
        public void AuthorTemplateTest()
        {
            _th.GenerateGsJob(PSfiles.PDFCreatorTestpage, OutputFormat.Pdf);
            _th.JobInfo.Metadata.PrintJobAuthor = "Author from Job";

            _th.Job.Profile.AutoSave.Enabled = true;

            _settings = new PdfCreatorSettings(null);
            _settings.ConversionProfiles.Add(_th.Job.Profile);
            _settings.ConversionProfiles[0].AutoSave.EnsureUniqueFilenames = false;
            _settings.ConversionProfiles[0].AutoSave.TargetDirectory = _th.TmpTestFolder;
            _settings.ConversionProfiles[0].FileNameTemplate = "AutoSaveTestOutput";
            _settings.ConversionProfiles[0].OutputFormat = OutputFormat.Pdf;
            _settings.ConversionProfiles[0].AuthorTemplate = "<PrintJobAuthor> + some text";

            var autoSaveWorkflow = new AutoSaveWorkflow(_th.Job, _settings);

            autoSaveWorkflow.RunWorkflow();

            var pdf = new PdfReader(_th.Job.OutputFiles[0]);
            Assert.AreEqual("Author from Job + some text", pdf.Info["Author"], "Wrong author in PDF Metadata");
        }
    }
}
