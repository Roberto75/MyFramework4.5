using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Ghostscript;
using pdfforge.PDFCreator.Core.Ghostscript.OutputDevices;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using PDFCreator.TestUtilities;

namespace PDFCreator.Core.Test.OutputDevices
{
    class JpegDeviceTest
    {
        private TestHelper _th;

        private JpegDevice _jpegDevice;
        private Collection<string> _parameterStrings;
        private GhostscriptVersion _ghostscriptVersion;
        private const string TestFile = "testfile.jpg";

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("JpegDeviceTest");

            _jpegDevice = new JpegDevice();
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Jpeg);
            _jpegDevice.Job = _th.Job;

            _jpegDevice.Job.OutputFiles.Add(TestFile);
            _jpegDevice.Profile = new ConversionProfile();

            _ghostscriptVersion = new GhostscriptDiscovery().GetBestGhostscriptInstance(TestHelper.MinGhostscriptVersion);
        }

        [TearDown]
        public void CleanUp()
        {
            _th.CleanUp();
        }

        [Test]
        public void CheckDeviceIndependentDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));
            DevicesGeneralTests.CheckDefaultParameters(_parameterStrings);
        }

        [Test]
        public void CheckDeviceSpecificDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            string jpegQualityParameter = "-dJPEGQ=" + _jpegDevice.Profile.JpegSettings.Quality;
            Assert.Contains(jpegQualityParameter, _parameterStrings, "Missing default device parameter.");

            string dpiParameter = "-r" + _jpegDevice.Profile.JpegSettings.Dpi;
            Assert.Contains(dpiParameter, _parameterStrings, "Missing default device parameter.");

            Assert.Contains("-dTextAlphaBits=4", _parameterStrings, "Missing default device parameter.");
            Assert.Contains("-dGraphicsAlphaBits=4", _parameterStrings, "Missing default device parameter.");

            var outputFileParameter = _parameterStrings.First(x => x.StartsWith("-sOutputFile="));
            Assert.IsNotNull(outputFileParameter, "Missing -sOutputFile parameter.");
            Assert.IsTrue(outputFileParameter.EndsWith("%d.jpeg", true, null), "Outputfile does not end with %d.jpeg");
        }

        [Test]
        public void ParametersTest_Color24Bit_Quality4_4Dpi()
        {
            _jpegDevice.Profile.JpegSettings.Color = JpegColor.Color24Bit;
            _jpegDevice.Profile.JpegSettings.Quality = 4;
            _jpegDevice.Profile.JpegSettings.Dpi = 4;

            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=jpeg", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_Color24Bit_Quality5_5Dpi()
        {
            _jpegDevice.Profile.JpegSettings.Color = JpegColor.Color24Bit;
            _jpegDevice.Profile.JpegSettings.Quality = 5;
            _jpegDevice.Profile.JpegSettings.Dpi = 5;

            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=jpeg", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_ColorGray_Quality100_2400Dpi()
        {
            _jpegDevice.Profile.JpegSettings.Color = JpegColor.Gray8Bit;
            _jpegDevice.Profile.JpegSettings.Quality = 100;
            _jpegDevice.Profile.JpegSettings.Dpi = 2400;

            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=jpeggray", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_ColorGray_Quality101_2401Dpi()
        {
            _jpegDevice.Profile.JpegSettings.Color = JpegColor.Gray8Bit;
            _jpegDevice.Profile.JpegSettings.Quality = 101;
            _jpegDevice.Profile.JpegSettings.Dpi = 2401;

            _parameterStrings = new Collection<string>(_jpegDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=jpeggray", _parameterStrings, "Missing parameter.");
        }
    }
}
