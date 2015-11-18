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
    class TiffDeviceTest
    {
        private TestHelper _th;
        
        private TiffDevice _tiffDevice;
        private Collection<string> _parameterStrings;
        private GhostscriptVersion _ghostscriptVersion;
        private const string TestFile = "testfile.tiff";

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("TiffDeviceTest");
            
            _tiffDevice = new TiffDevice();
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Tif);
            _tiffDevice.Job = _th.Job;

            _tiffDevice.Job.OutputFiles.Add(TestFile);
            _tiffDevice.Profile = new ConversionProfile();

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
            _parameterStrings = new Collection<string>(_tiffDevice.GetGhostScriptParameters(_ghostscriptVersion));
            DevicesGeneralTests.CheckDefaultParameters(_parameterStrings);
        }

        [Test]
        public void CheckDeviceSpecificDefaultParameters()
        {
            DevicesGeneralTests.CheckDefaultParameters(_parameterStrings);

            Assert.Contains("-sCompression=lzw", _parameterStrings, "Missing default device parameter.");
            Assert.Contains("-dTextAlphaBits=4", _parameterStrings, "Missing default device parameter.");
            Assert.Contains("-dGraphicsAlphaBits=4", _parameterStrings, "Missing default device parameter.");
            string dpiParameter = "-r" + _tiffDevice.Profile.TiffSettings.Dpi;
            Assert.Contains(dpiParameter, _parameterStrings, "Missing default  device parameter.");

            var outputFileParameter = _parameterStrings.First(x => x.StartsWith("-sOutputFile="));
            Assert.IsNotNull(outputFileParameter, "Missing -sOutputFile parameter.");
            Assert.IsTrue(outputFileParameter.EndsWith(".tif", true, null), "Outputfile does not end with .tif");
        }

        [Test]
        public void ParametersTest_BlackWhite_5Dpi()
        {
            _tiffDevice.Profile.TiffSettings.Color = TiffColor.BlackWhite;
            _tiffDevice.Profile.TiffSettings.Dpi = 5;

            _parameterStrings = new Collection<string>(_tiffDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=tiffg4", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_Color12Bit_2400Dpi()
        {
            _tiffDevice.Profile.TiffSettings.Color = TiffColor.Color12Bit;
            _tiffDevice.Profile.TiffSettings.Dpi = 2400;

            _parameterStrings = new Collection<string>(_tiffDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=tiff12nc", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_Color24Bit_4Dpi()
        {
            _tiffDevice.Profile.TiffSettings.Color = TiffColor.Color24Bit;
            _tiffDevice.Profile.TiffSettings.Dpi = 4;

            _parameterStrings = new Collection<string>(_tiffDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=tiff24nc", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_Grey8Bit_2401Dpi()
        {
            _tiffDevice.Profile.TiffSettings.Color = TiffColor.Gray8Bit;
            _tiffDevice.Profile.TiffSettings.Dpi = 2401;

            _parameterStrings = new Collection<string>(_tiffDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sDEVICE=tiffgray", _parameterStrings, "Missing parameter.");
        }
    }
}
