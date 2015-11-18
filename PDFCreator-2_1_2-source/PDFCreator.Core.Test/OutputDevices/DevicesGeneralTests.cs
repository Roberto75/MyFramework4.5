using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using SystemInterface.IO;
using SystemWrapper.IO;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Ghostscript;
using PDFCreator.TestUtilities;
using pdfforge.PDFCreator.Core.Ghostscript.OutputDevices;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using Rhino.Mocks;

namespace PDFCreator.Core.Test.OutputDevices
{
    [TestFixture]
    class DevicesGeneralTests
    {
        private TestHelper _th;

        private OutputDevice _outputDevice;
        private Collection<string> _parameterStrings;

        private GhostscriptVersion _ghostscriptVersion;
        
        /// <summary>
        /// Filename for temporary outputfile based on JobTempFileName (output.pdf)
        /// </summary>
        private string[] _singleTempOutputfile;

        /// <summary>
        /// Filenames for temporary outputfiles based on JobTempFileName (output.png, output2.png, output3.png)
        /// </summary>
        private string[] _multipleTempOutputFiles;

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("DevicesGeneralTests");

            _outputDevice = new PdfDevice();
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);

            _singleTempOutputfile = new[] { _th.Job.JobTempFileName + ".pdf" };
            _multipleTempOutputFiles = new[] { _th.Job.JobTempFileName + "1.png"
                , _th.Job.JobTempFileName + "2.png"
                , _th.Job.JobTempFileName + "3.png" };
            
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = new ConversionProfile();

            _ghostscriptVersion = new GhostscriptDiscovery().GetBestGhostscriptInstance(TestHelper.MinGhostscriptVersion);
        }

        [TearDown]
        public void CleanUp()
        {
            _th.CleanUp();
        }
        
        public static void CheckDefaultParameters(Collection<string> parameters)
        {
            Assert.Contains("gs", parameters, "Missing default parameter.");
            Assert.AreEqual(0, parameters.IndexOf("gs"), "gs is not the first parameter.");

            Assert.IsNotNull(parameters.First(x => x.StartsWith("-I")), "Missing -I GhostscriptLib Parameter");
            Assert.IsNotNull(parameters.First(x => x.StartsWith("-sFONTPATH=")), "Missing -sFONTPATH= Parameter");

            Assert.Contains("-dNOPAUSE", parameters, "Missing default parameter.");
            Assert.Contains("-dBATCH", parameters, "Missing default parameter.");

            Assert.Contains("-f", parameters, "missing default parameter.");

            var metadataFile = parameters.FirstOrDefault(x => x.EndsWith("metadata.mtd"));
            Assert.IsNotNull(metadataFile, "Missing metadata file.");
            Assert.AreEqual(parameters.Count-1, parameters.IndexOf(metadataFile), "Metadata file is not the last parameter");
        }

        private string RemoveExtension(string filePath)
        {
            var extensionLength = Path.GetExtension(filePath).Length;
            var length = filePath.Length;
            return filePath.Substring(0, length - extensionLength - 1);
        }

        [Test]
        public void CheckDeviceIndependentDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_outputDevice.GetGhostScriptParameters(_ghostscriptVersion));
            CheckDefaultParameters(_parameterStrings);
        }

        [Test]
        public void ParametersTest_CoverPage()
        {
            _outputDevice.Profile.CoverPage.Enabled = true;
            const string coverFile = "CoverFile.pdf";
            _outputDevice.Profile.CoverPage.File = coverFile;

            _parameterStrings = new Collection<string>(_outputDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains(coverFile, _parameterStrings, "Missing parameter.");
            var fIndex = _parameterStrings.IndexOf("-f");
            Assert.Less(fIndex, _parameterStrings.IndexOf(coverFile), "CoverFile not behind -f parameter.");

            _outputDevice.Profile.CoverPage.Enabled = false;
            _parameterStrings = new Collection<string>(_outputDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.AreEqual(-1, _parameterStrings.IndexOf(coverFile), "Falsely set CoverFile.");
        }

        [Test]
        public void ParametersTest_AttachmentPage()
        {
            _outputDevice.Profile.AttachmentPage.Enabled = true;
            const string attachmentFile = "AttachmentFile.pdf";
            _outputDevice.Profile.AttachmentPage.File = attachmentFile;

            _parameterStrings = new Collection<string>(_outputDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains(attachmentFile, _parameterStrings, "Missing parameter.");
            var fIndex = _parameterStrings.IndexOf("-f");
            Assert.Less(fIndex, _parameterStrings.IndexOf(attachmentFile), "AttachmentFile not behind -f parameter.");

            _outputDevice.Profile.AttachmentPage.Enabled = false;
            _parameterStrings = new Collection<string>(_outputDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.AreEqual(-1, _parameterStrings.IndexOf(attachmentFile), "Falsely set AttachmentFile.");
        }

        [Test]
        public void ParametersTest_CoverAndAttachmentPage()
        {
            _outputDevice.Profile.CoverPage.Enabled = true;
            const string coverFile = "CoverFile.pdf";
            _outputDevice.Profile.CoverPage.File = coverFile;
            _outputDevice.Profile.AttachmentPage.Enabled = true;
            const string attachmentFile = "AttachmentFile.pdf";
            _outputDevice.Profile.AttachmentPage.File = attachmentFile;

            _parameterStrings = new Collection<string>(_outputDevice.GetGhostScriptParameters(_ghostscriptVersion));

            var coverIndex = _parameterStrings.IndexOf(coverFile);
            var attachmentIndex = _parameterStrings.IndexOf(attachmentFile);

            Assert.LessOrEqual(coverIndex - attachmentIndex, -2, "No further (file)parameter between cover and attachment file.");
        }

        [Test]
        public void ParametersTest_Stamping()
        {
            _outputDevice.Profile.Stamping.Enabled = true;
            _parameterStrings = new Collection<string>(_outputDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.IsNotNull(_parameterStrings.FirstOrDefault(x => x.EndsWith(".stm")), "Missing stamp file.");
        }

        [Test]
        public void Test_MakeValidFileExtension_Pdf()
        {
            var file = _outputDevice.MakeValidExtension("testfile.nonsense", OutputFormat.Pdf);
            Assert.AreEqual("testfile.pdf", file);
        }

        [Test]
        public void Test_MakeValidFileExtension_PdfA1B()
        {
            var file = _outputDevice.MakeValidExtension("testfile.nonsense", OutputFormat.PdfA1B);
            Assert.AreEqual("testfile.pdf", file);
        }

        [Test]
        public void Test_MakeValidFileExtension_PdfA2B()
        {
            var file = _outputDevice.MakeValidExtension("testfile.nonsense", OutputFormat.PdfA2B);
            Assert.AreEqual("testfile.pdf", file);
        }

        [Test]
        public void Test_MakeValidFileExtension_PdfX()
        {
            var file = _outputDevice.MakeValidExtension("testfile.nonsense", OutputFormat.PdfX);
            Assert.AreEqual("testfile.pdf", file);
        }

        [Test]
        public void Test_MakeValidFileExtension_Jpeg()
        {
            var file = _outputDevice.MakeValidExtension("testfile.nonsense", OutputFormat.Jpeg);
            Assert.AreEqual("testfile.jpg", file);
        }

        [Test]
        public void Test_MakeValidFileExtension_Png()
        {
            var file = _outputDevice.MakeValidExtension("testfile.nonsense", OutputFormat.Png);
            Assert.AreEqual("testfile.png", file);
        }

        [Test]
        public void Test_MakeValidFileExtension_Tiff()
        {
            var file = _outputDevice.MakeValidExtension("testfile.nonsense", OutputFormat.Tif);
            Assert.AreEqual("testfile.tif", file);
        }

        [Test]
        public void Test_SingleFileDevice_CollectOutputFiles()
        {
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);

            var directoryWrapStub = MockRepository.GenerateStub<IDirectory>();
            directoryWrapStub.Stub(x => x.GetFiles(_th.Job.JobTempOutputFolder)).Return(_singleTempOutputfile);

            _outputDevice = new PngDevice(new FileWrap(), directoryWrapStub);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;

            _outputDevice.CollectTemporaryOutputFiles();

            Assert.AreEqual(_singleTempOutputfile, _outputDevice.TempOutputFiles, "Wrong outputfiles.");
        }

        [Test]
        public void Test_SingleFileDevice_MoveOutputFiles_CopyingIsSuccessful()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();
          
            _outputDevice = new PdfDevice(fileWrapStub);
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.TempOutputFiles = _singleTempOutputfile;
            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            Assert.AreEqual(1, _outputDevice.Job.OutputFiles.Count, "Wrong number of outputfiles.");
            Assert.AreEqual(filenameTemplate, _outputDevice.Job.OutputFiles[0], "Outputfile is not the template in job.");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Once());
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Once()); //Delete must only be called once
        }

        [Test]
        public void Test_SingleFileDevice_MoveOutputFiles_FirstAttemptToCopyFailsSecondIsSuccessful_OutputfileMustHaveAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything))
                .Throw(new IOException("IoException"))
                .Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);

            _outputDevice = new PdfDevice(fileWrapStub);
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.TempOutputFiles = _singleTempOutputfile;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith("_2.pdf", true, CultureInfo.CurrentCulture), "Failure in file appendix");
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Once()); //Delete must only be called once
        }

        [Test]
        public void Test_SingleFileDevice_MoveOutputFiles_TwoAttemptsToCopyFail_ThrowDeviceException()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();
            fileWrapStub.Stub(x => x.Copy("ignore", "ignore2", true)).IgnoreArguments().Throw(new IOException("")); //Deny all attempts to copy

            _outputDevice = new PdfDevice(fileWrapStub);
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.TempOutputFiles = _singleTempOutputfile;

            _outputDevice.CollectTemporaryOutputFiles();

            Assert.Throws<DeviceException>(() => _outputDevice.MoveOutputFiles());
            fileWrapStub.AssertWasCalled(x => x.Copy("","",true), options => options.IgnoreArguments().Repeat.Twice()); //DeviceException should be thrown after second denied copy call
            fileWrapStub.AssertWasNotCalled(x => x.Delete("ignore"), options => options.IgnoreArguments()); //Delete never gets called
        }

        [Test]
        public void Test_SingleFileDevice_AutosaveWithUniqueFilename_MoveOutputFiles_FileExists_OutputfileMustHaveAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);

            _outputDevice = new PdfDevice(fileWrapStub);
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = true;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _singleTempOutputfile;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith("_2.pdf", true, CultureInfo.CurrentCulture), "Failure in file appendix");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore", false), options => options.IgnoreArguments().Repeat.Once()); //Copy must only be called once
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Once()); //Delete must only be called once
        }

        [Test]
        public void Test_SingleFileDevice_AutosaveWithUniqueFilename_MoveOutputFiles_FileExists_FirstAttemptToCopyFails_FileExists_OutputfileHasContinuedAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Throw(new IOException("IoException")).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file after first attempt to copy has failed.
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);
            _outputDevice = new PdfDevice(fileWrapStub);
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = true;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _singleTempOutputfile;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith("_3.pdf", true, CultureInfo.CurrentCulture), "Failure in file appendix.");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore", false), options => options.IgnoreArguments().Repeat.Twice()); //Copy must only be called once
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Once()); //Delete must only be called once
        }

        [Test]
        public void Test_SingleFileDevice_UniqueFilenameWithoutAutosave_NoAppedixForExistingOutputfile()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);

            _outputDevice = new PdfDevice(fileWrapStub);
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = false;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _singleTempOutputfile;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Outputfile is not based on template from job.");
            Assert.IsFalse(_outputDevice.Job.OutputFiles[0].EndsWith("_2.pdf", true, CultureInfo.CurrentCulture), "Outputfile has an unwanted appendix.");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore", false), options => options.IgnoreArguments().Repeat.Once()); //Copy must only be called once
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Once()); //Delete must only be called once
        }

        [Test]
        public void Test_MultiFileDevice_CollectOutputFiles()
        {
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Png);
            
            var directoryWrapStub = MockRepository.GenerateStub<IDirectory>();      
            directoryWrapStub.Stub(x => x.GetFiles(_th.Job.JobTempOutputFolder)).Return(_multipleTempOutputFiles);
            
            _outputDevice = new PngDevice(new FileWrap(), directoryWrapStub);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;

            _outputDevice.CollectTemporaryOutputFiles();

            Assert.AreEqual(_multipleTempOutputFiles, _outputDevice.TempOutputFiles, "Wrong outputfiles.");
        }

        [Test]
        public void Test_MultipleFileDevice_MoveOutputFiles_CopyingIsSuccessful()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Png);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;
            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            Assert.AreEqual(3, _outputDevice.Job.OutputFiles.Count, "Wrong number of outputfiles.");
            Assert.AreEqual(filenameTemplate.ToLower(), _outputDevice.Job.OutputFiles[0].ToLower(), "First outputfile is not the template in job.");
            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3.png", true, CultureInfo.CurrentCulture), "Failure in appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
        }

        [Test]
        public void Test_MultipleFileDevice_MoveOutputFiles_FirstAttemptToCopyFailsSecondIsSuccessful_FirstOutputfileMustHaveAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything))
                .Throw(new IOException("IoException"))
                .Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);

            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Png);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;
            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "first outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith("_2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of first outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3.png", true, CultureInfo.CurrentCulture), "Failure in appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(4)); //Four times, once for each file and one for "failed" attempt
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
        }

        [Test]
        public void Test_MultipleFileDevice_MoveOutputFiles_FirstAttemptToCopyThirdFileFailsSecondIsSuccessful_ThirdOutputfileMustHaveAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything))
                .Repeat.Twice(); //Copying for first two files is successful
            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything))
                .Throw(new IOException("IoException"))
                .Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);

            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Png);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;
            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            Assert.AreEqual(filenameTemplate.ToLower(), _outputDevice.Job.OutputFiles[0].ToLower(), "First outputfile is not the template in job.");
            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3_2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(4)); //Four times, once for each file and one for "failed" attempt
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Four times, once for each file one for "failed" attempt
        }

        [Test]
        public void Test_MutipleFileDevice_MoveOutputFiles_TwoAttemptsToCopyFirstFileFail_ThrowDeviceException()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();
            fileWrapStub.Stub(x => x.Copy("ignore", "ignore2", true)).IgnoreArguments().Throw(new IOException("")); //Deny all attempts to copy

            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Png);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;

            Assert.Throws<DeviceException>(() => _outputDevice.MoveOutputFiles());
            fileWrapStub.AssertWasCalled(x => x.Copy("", "", true), options => options.IgnoreArguments().Repeat.Twice()); //DeviceException should be thrown after second denied copy call
            fileWrapStub.AssertWasNotCalled(x => x.Delete("ignore"), options => options.IgnoreArguments()); //Delete never gets called
        }

        [Test]
        public void Test_MultiFileDevice_AutosaveWithUniqueFilename_MoveOutputFiles_FirstFileExists_FirstOutputfileMustHaveAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);

            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = true;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "first outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith("_2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of first outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3.png", true, CultureInfo.CurrentCulture), "Failure in appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
        }

        [Test]
        public void Test_MultiFileDevice_AutosaveWithUniqueFilename_MoveOutputFiles_ThirdFileExists_ThirdOutputfileMustHaveAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(false).Repeat.Twice(); //Simulate existing file in third request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(false);

            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = true;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "first outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith(".png", true, CultureInfo.CurrentCulture), "Failure in appendix of first outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3_2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
        }
    
        [Test]
        public void Test_MultiFileDevice_AutosaveWithUniqueFilename_MoveOutputFiles_FirstFileExists_FirstAttemptToCopyFails_FileExists_FirstOutputfileHasContinuedAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Throw(new IOException("IoException")).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file after first attempt to copy has failed.
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);
            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = true;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "first outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith("_3.png", true, CultureInfo.CurrentCulture), "Failure in continued appendix of first outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3.png", true, CultureInfo.CurrentCulture), "Failure in appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(4)); //Four times, once for each file and once for failed attempt
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
        }

        [Test]
        public void Test_MultiFileDevice_AutosaveWithUniqueFilename_MoveOutputFiles_ThirdFileExists_FirstAttemptToCopyFails_FileExists_ThirdOutputfileHasContinuedAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Repeat.Twice();
            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Throw(new IOException("IoException")).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(false).Repeat.Twice();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(true).Repeat.Twice();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);
            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = true;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "first outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith(".png", true, CultureInfo.CurrentCulture), "Failure in appendix of first outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3_3.png", true, CultureInfo.CurrentCulture), "Failure in continued appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(4)); //Four times, once for each file and once for failed attempt
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
        }

        [Test]
        public void Test_MultiFileDevice_UniqueFilenameWithoutAutosave_MoveOutputFiles_FirstFileExists_FirstOutputfileHasNoAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once(); //Simulate existing file in first request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).IgnoreArguments().Return(false);

            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = false;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "first outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith(".png", true, CultureInfo.CurrentCulture), "Failure in appendix of first outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3.png", true, CultureInfo.CurrentCulture), "Failure in appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
        }

        [Test]
        public void Test_MultiFileDevice_UniqueFilenameWithoutAutosave_MoveOutputFiles_ThirdFileExists_ThirdOutputfileHasNoAppendix()
        {
            var fileWrapStub = MockRepository.GenerateStub<IFile>();

            fileWrapStub.Stub(x => x.Copy(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<Boolean>.Is.Anything)).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(false).Repeat.Twice(); //Simulate existing file in third request for unique filename
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(true).Repeat.Once();
            fileWrapStub.Stub(x => x.Exists(Arg<String>.Is.Anything)).Return(false);

            _outputDevice = new PngDevice(fileWrapStub, new DirectoryWrap());
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _outputDevice.Job = _th.Job;
            _outputDevice.Profile = _th.Profile;
            _outputDevice.Profile.AutoSave.Enabled = false;
            _outputDevice.Profile.AutoSave.EnsureUniqueFilenames = true;
            _outputDevice.TempOutputFiles = _multipleTempOutputFiles;

            var filenameTemplate = _th.Job.OutputFilenameTemplate; //Save it, because it can change in MoveOutputFiles 

            _outputDevice.MoveOutputFiles();

            var templateWithoutExtension = RemoveExtension(filenameTemplate);
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "first outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[0].EndsWith(".png", true, CultureInfo.CurrentCulture), "Failure in appendix of first outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Second outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[1].EndsWith("2.png", true, CultureInfo.CurrentCulture), "Failure in appendix of second outputfile");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].StartsWith(templateWithoutExtension, true, CultureInfo.CurrentCulture), "Third outputfile is not based on template from job.");
            Assert.IsTrue(_outputDevice.Job.OutputFiles[2].EndsWith("3.png", true, CultureInfo.CurrentCulture), "Failure in appendix of third outputfile");
            fileWrapStub.AssertWasCalled(x => x.Copy("ignore", "ignore2", true), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
            fileWrapStub.AssertWasCalled(x => x.Delete("ignore"), options => options.IgnoreArguments().Repeat.Times(3)); //Three times, once for each file
        }
  
    }
}
