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
    class PngDeviceTest
    {
        private TestHelper _th;

        private PngDevice _pngDevice;
        private Collection<string> _parameterStrings;
        private GhostscriptVersion _ghostscriptVersion;
        private const string TestFile = "testfile.jpg";

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("PngDeviceTest");

            _pngDevice = new PngDevice();
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Png);
            _pngDevice.Job = _th.Job;

            _pngDevice.Job.OutputFiles.Add(TestFile);
            _pngDevice.Profile = new ConversionProfile();

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
            _parameterStrings = new Collection<string>(_pngDevice.GetGhostScriptParameters(_ghostscriptVersion));
            DevicesGeneralTests.CheckDefaultParameters(_parameterStrings);
        }

        [Test]
        public void CheckDeviceSpecificDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_pngDevice.GetGhostScriptParameters(_ghostscriptVersion));
            
            string dpiParameter = "-r" + _pngDevice.Profile.PngSettings.Dpi;
            Assert.Contains(dpiParameter, _parameterStrings, "Missing default device parameter.");

            Assert.Contains("-dTextAlphaBits=4", _parameterStrings, "Missing default device parameter.");
            Assert.Contains("-dGraphicsAlphaBits=4", _parameterStrings, "Missing default device parameter.");

            var outputFileParameter = _parameterStrings.First(x => x.StartsWith("-sOutputFile="));
            Assert.IsNotNull(outputFileParameter, "Missing -sOutputFile parameter.");
            Assert.IsTrue(outputFileParameter.EndsWith("%d.png", true, null), "Outputfile does not end with %d.png");
        }

        [Test]
        public void ParametersTest_ColorBlackWhite_4Dpi()
        {
            _pngDevice.Profile.PngSettings.Color = PngColor.BlackWhite;
            _pngDevice.Profile.PngSettings.Dpi = 4;

            _parameterStrings = new Collection<string>(_pngDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=pngmonod", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_Color24Bit_5Dpi()
        {
            _pngDevice.Profile.PngSettings.Color = PngColor.Color24Bit;
            _pngDevice.Profile.PngSettings.Dpi = 5;

            _parameterStrings = new Collection<string>(_pngDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=png16m", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_Color32BitTransp_800Dpi()
        {
            _pngDevice.Profile.PngSettings.Color = PngColor.Color32BitTransp;
            _pngDevice.Profile.PngSettings.Dpi = 800;

            _parameterStrings = new Collection<string>(_pngDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=pngalpha", _parameterStrings, "Missing parameter.");
        }
        [Test]
        public void ParametersTest_Color4Bit_1600Dpi()
        {
            _pngDevice.Profile.PngSettings.Color = PngColor.Color4Bit;
            _pngDevice.Profile.PngSettings.Dpi = 1600;

            _parameterStrings = new Collection<string>(_pngDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=png16", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_Color8Bit_2400Dpi()
        {
            _pngDevice.Profile.PngSettings.Color = PngColor.Color8Bit;
            _pngDevice.Profile.PngSettings.Dpi = 2400;

            _parameterStrings = new Collection<string>(_pngDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=png256", _parameterStrings, "Missing parameter.");
        }
        
        [Test]
        public void ParametersTest_ColorGray8Bit_2401Dpi()
        {
            _pngDevice.Profile.PngSettings.Color = PngColor.Gray8Bit;
            _pngDevice.Profile.PngSettings.Dpi = 2401;

            _parameterStrings = new Collection<string>(_pngDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=pnggray", _parameterStrings, "Missing parameter.");
        }
    }
}
