using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using pdfforge.PDFCreator.Core.Ghostscript;
using PDFCreator.TestUtilities;
using pdfforge.PDFCreator.Core.Ghostscript.OutputDevices;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace PDFCreator.Core.Test.OutputDevices
{
    class PdfDeviceTest
    {
        private TestHelper _th;

        private PdfDevice _pdfDevice;
        private Collection<string> _parameterStrings;
        private GhostscriptVersion _ghostscriptVersion;
        private const string TestFile = "testfile.pdf";

        [SetUp]
        public void SetUp()
        {
            _th = new TestHelper("PdfDeviceTest");

            _pdfDevice = new PdfDevice();
            _th.GenerateGsJob(PSfiles.ThreePDFCreatorTestpages, OutputFormat.Pdf);
            _pdfDevice.Job = _th.Job;

            _pdfDevice.Job.OutputFiles.Add(TestFile);
            _pdfDevice.Profile = new ConversionProfile();

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
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            DevicesGeneralTests.CheckDefaultParameters(_parameterStrings);
        }

        [Test]
        public void CheckDeviceSpecificDefaultParameters()
        {
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            
            Assert.Contains("-sDEVICE=pdfwrite", _parameterStrings, "Missing default device parameter.");
            Assert.Contains("-dCompatibilityLevel=1.5", _parameterStrings, "Missing CompatibilityLevel 1.5");
            Assert.Contains("-dPDFSETTINGS=/default", _parameterStrings, "Missing default device parameter.");
            Assert.Contains("-dEmbedAllFonts=true", _parameterStrings, "Missing default device parameter.");

            var outputFileParameter = _parameterStrings.First(x => x.StartsWith("-sOutputFile="));
            Assert.IsNotNull(outputFileParameter, "Missing -sOutputFile parameter.");
            Assert.IsTrue(outputFileParameter.EndsWith(".pdf", true, null), "Outputfile does not end with .pdf");
        }

        [Test]
        public void ParametersTest_PdfX()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dPDFX", _parameterStrings, "Missing parameter.");
            var iccProfile = _parameterStrings.FirstOrDefault(x => x.StartsWith("-sOutputICCProfile=\""));
            Assert.IsNotNull(iccProfile, "Missing Parameter for ICC Profile.");            
            var defFile = _parameterStrings.FirstOrDefault(x => x.EndsWith("pdfx_def.ps"));
            Assert.IsNotNull(defFile, "Missing DefFile.");
        }

        [Test]
        public void ParametersTest_PdfX_DefinitonFile_Behind_ProcessColorModel()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            
            var defFile = _parameterStrings.FirstOrDefault(x => x.EndsWith("pdfx_def.ps"));
            var defFileIndex = _parameterStrings.IndexOf(defFile);

            var processColorModel = _parameterStrings.FirstOrDefault(x => x.StartsWith("-dProcessColorModel"));
            var processColorModelIndex = _parameterStrings.IndexOf(processColorModel);

            Assert.Greater(defFileIndex, processColorModelIndex);
        }

        [Test]
        public void ParametersTest_PdfX_DefinitonFile_Behind_ColorConversionStrategyIndex()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            var defFile = _parameterStrings.FirstOrDefault(x => x.EndsWith("pdfx_def.ps"));
            var defFileIndex = _parameterStrings.IndexOf(defFile);

            var sColorConversionStrategy = _parameterStrings.FirstOrDefault(x => x.StartsWith("-sColorConversionStrategy"));
            var sColorConversionStrategyIndex = _parameterStrings.IndexOf(sColorConversionStrategy);

            Assert.Greater(defFileIndex, sColorConversionStrategyIndex);
        }

        [Test]
        public void ParametersTest_PdfX_DefinitonFile_Before_GrayImageAutoFilterStrategy()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            var grayImageAutoFilterStrategy = _parameterStrings.FirstOrDefault(x => x.StartsWith("-dGrayImageAutoFilterStrategy"));
            var grayImageAutoFilterStrategyIndex = _parameterStrings.IndexOf(grayImageAutoFilterStrategy);
            
            var defFile = _parameterStrings.FirstOrDefault(x => x.EndsWith("pdfx_def.ps"));
            var defFileIndex = _parameterStrings.IndexOf(defFile);

            Assert.Greater(grayImageAutoFilterStrategyIndex, defFileIndex);
        }

        [Test]
        public void ParametersTest_PdfA1b()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA1B;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dPDFA=1", _parameterStrings, "Missing parameter.");
            var iccProfile = _parameterStrings.FirstOrDefault(x => x.StartsWith("-sOutputICCProfile=\""));
            Assert.IsNotNull(iccProfile, "Missing Parameter for ICC Profile.");
            var defFile = _parameterStrings.FirstOrDefault(x => x.EndsWith("pdfa_def.ps"));
            Assert.IsNotNull(defFile, "Missing DefFile.");
        }

        [Test]
        public void ParametersTest_PdfA2b()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA2B;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dPDFA=2", _parameterStrings, "Missing parameter.");
            var iccProfile = _parameterStrings.FirstOrDefault(x => x.StartsWith("-sOutputICCProfile=\""));
            Assert.IsNotNull(iccProfile, "Missing Parameter for ICC Profile.");
            var defFile = _parameterStrings.FirstOrDefault(x => x.EndsWith("pdfa_def.ps"));
            Assert.IsNotNull(defFile, "Missing DefFile.");
        }

        [Test]
        public void ParametersTest_FastWebView()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.FastWebView = true;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.Contains("-dFastWebView=true", _parameterStrings, "Missing parameter.");

            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.FastWebView = false;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.False(_parameterStrings.Contains("-dFastWebView=true"), "Fast web view parameter falsely set.");

            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA1B;
            _pdfDevice.Profile.PdfSettings.FastWebView = true;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.False(_parameterStrings.Contains("-dFastWebView=true"), "PdfA-1b must not contain fast web view parameter.");

            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA2B;
            _pdfDevice.Profile.PdfSettings.FastWebView = true;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.False(_parameterStrings.Contains("-dFastWebView=true"), "PdfA-2b must not contain fast web view parameter.");
        }

        [Test]
        public void ParametersTest_PageOrientation_Landscape()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.PageOrientation = PageOrientation.Landscape;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.Contains("-dAutoRotatePages=/None", _parameterStrings, "Missing parameter.");
            
            Assert.Contains("-c", _parameterStrings, "Missing parameter.");
            Assert.Contains("<</Orientation 3>> setpagedevice", _parameterStrings, "Missing parameter.");
            var cIndex = _parameterStrings.IndexOf("-c");
            Assert.Less(cIndex, _parameterStrings.IndexOf("<</Orientation 3>> setpagedevice"), "Destiller parameter before -c");
        }

        [Test]
        public void ParametersTest_PageOrientation_Automatic()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA2B;
            _pdfDevice.Profile.PdfSettings.PageOrientation = PageOrientation.Automatic;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.Contains("-dAutoRotatePages=/PageByPage", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dParseDSCComments=false", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_PageOrientation_Portrait()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;
            _pdfDevice.Profile.PdfSettings.PageOrientation = PageOrientation.Portrait;
            
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            
            Assert.Contains("-dAutoRotatePages=/None", _parameterStrings, "Missing parameter.");

            Assert.Contains("-c", _parameterStrings, "Missing parameter.");
            Assert.Contains("<</Orientation 0>> setpagedevice", _parameterStrings, "Missing parameter.");
            var cIndex = _parameterStrings.IndexOf("-c");
            Assert.Less(cIndex, _parameterStrings.IndexOf("<</Orientation 0>> setpagedevice"), "Destiller parameter before -c");
        }

        [Test]
        public void ParametersTest_ColorSchemes_Cmyk()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.ColorModel = ColorModel.Cmyk;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.Contains("-sColorConversionStrategy=CMYK", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dProcessColorModel=/DeviceCMYK", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_ColorSchemes_Gray()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.ColorModel = ColorModel.Gray;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.Contains("-sColorConversionStrategy=Gray", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dProcessColorModel=/DeviceGray", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_ColorSchemes_Rgb()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.ColorModel = ColorModel.Rgb;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.Contains("-sColorConversionStrategy=RGB", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dProcessColorModel=/DeviceRGB", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dConvertCMYKImagesToRGB=true", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_ColorSchemes_Rgb_PdfA_1b_RequiresDeviceIndependentColor()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA1B;
            _pdfDevice.Profile.PdfSettings.ColorModel = ColorModel.Rgb;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.Contains("-sColorConversionStrategy=/UseDeviceIndependentColor", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dProcessColorModel=/DeviceRGB", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dConvertCMYKImagesToRGB=true", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_ColorSchemes_Rgb_PdfA_2b_RequiresDeviceIndependentColor()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA2B;
            _pdfDevice.Profile.PdfSettings.ColorModel = ColorModel.Rgb;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.Contains("-sColorConversionStrategy=/UseDeviceIndependentColor", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dProcessColorModel=/DeviceRGB", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dConvertCMYKImagesToRGB=true", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_PdfX_RgbBecomesCmyk()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;
            _pdfDevice.Profile.PdfSettings.ColorModel = ColorModel.Rgb;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-sColorConversionStrategy=CMYK", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dProcessColorModel=/DeviceCMYK", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionDisabled()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = false;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dAutoFilterColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=false", _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionJpegMaximum_ResamplingDisabled()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegMaximum;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = false;
            
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            
            Assert.Contains("-dAutoFilterColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-c", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /ColorImageDict <</QFactor 2.4 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /GrayImageDict <</QFactor 2.4 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            var cIndex = _parameterStrings.IndexOf("-c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /ColorImageDict <</QFactor 2.4 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /GrayImageDict <</QFactor 2.4 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionJpegHigh_Resampling3Dpi_RaisedTo4()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA2B;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegHigh;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Dpi = 3;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dAutoFilterColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-c", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /ColorImageDict <</QFactor 1.3 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /GrayImageDict <</QFactor 1.3 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            var cIndex = _parameterStrings.IndexOf("-c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /ColorImageDict <</QFactor 1.3 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /GrayImageDict <</QFactor 1.3 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");

            Assert.Contains("-dDownsampleColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageResolution=" + 4, _parameterStrings, "Missing parameter.");
            Assert.Contains("-dDownsampleGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageResolution=" + 4, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionJpegMedium_Resampling4Dpi()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegMedium;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Dpi = 4;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dAutoFilterColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-c", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /ColorImageDict <</QFactor 0.76 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /GrayImageDict <</QFactor 0.76 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            var cIndex = _parameterStrings.IndexOf("-c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /ColorImageDict <</QFactor 0.76 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /GrayImageDict <</QFactor 0.76 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");

            Assert.Contains("-dDownsampleColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageResolution=" + 4, _parameterStrings, "Missing parameter.");
            Assert.Contains("-dDownsampleGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageResolution=" + 4, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionJpegLow_Resampling150Dpi()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegLow;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Dpi = 150;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            
            Assert.Contains("-dAutoFilterColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-c", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /ColorImageDict <</QFactor 0.40 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /GrayImageDict <</QFactor 0.40 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            var cIndex = _parameterStrings.IndexOf("-c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /ColorImageDict <</QFactor 0.40 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /GrayImageDict <</QFactor 0.40 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");

            Assert.Contains("-dDownsampleColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageResolution=" + 150, _parameterStrings, "Missing parameter.");
            Assert.Contains("-dDownsampleGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageResolution=" + 150, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionJpegMinimum_Resampling1200Dpi()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA2B;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegMinimum;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Dpi = 1200;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dAutoFilterColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-c", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /ColorImageDict <</QFactor 0.15 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /GrayImageDict <</QFactor 0.15 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            var cIndex = _parameterStrings.IndexOf("-c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /ColorImageDict <</QFactor 0.15 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /GrayImageDict <</QFactor 0.15 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");

            Assert.Contains("-dDownsampleColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageResolution=" + 1200, _parameterStrings, "Missing parameter.");
            Assert.Contains("-dDownsampleGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageResolution=" + 1200, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionJpegZip_Resampling2400Dpi()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.Zip;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Dpi = 2400;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dAutoFilterColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageFilter=/FlateEncode", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageFilter=/FlateEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-dDownsampleColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageResolution=" + 2400, _parameterStrings, "Missing parameter.");
            Assert.Contains("-dDownsampleGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageResolution=" + 2400, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionJpegManual_Resampling2401Dpi_LoweredTo2400Dpi()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.JpegManual;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor = 1.23;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Dpi = 2401;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            
            Assert.Contains("-dAutoFilterColorImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=false", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-c", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /ColorImageDict <</QFactor " +
                                                1.23.ToString(CultureInfo.InvariantCulture) +
                                                " /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            Assert.Contains(".setpdfwrite << /GrayImageDict <</QFactor " +
                                                1.23.ToString(CultureInfo.InvariantCulture) +
                                                " /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams", _parameterStrings, "Missing parameter.");
            var cIndex = _parameterStrings.IndexOf("-c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /ColorImageDict <</QFactor " +
                                                1.23.ToString(CultureInfo.InvariantCulture) +
                                                " /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");
            Assert.Less(cIndex, _parameterStrings.IndexOf(".setpdfwrite << /GrayImageDict <</QFactor " +
                                                1.23.ToString(CultureInfo.InvariantCulture) +
                                                " /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams"), "Destiller parameter before -c");

            Assert.Contains("-dDownsampleColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageResolution=" + 2400, _parameterStrings, "Missing parameter.");
            Assert.Contains("-dDownsampleGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageResolution=" + 2400, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_GrayAndColorImagesCompressionJpegAutomatic_Resampling300Dpi_ResamplingGetsDisabled()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA2B;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Compression = CompressionColorAndGray.Automatic;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Dpi = 300;

            _pdfDevice.Profile.PdfSettings.CompressColorAndGray.Resampling = true;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dAutoFilterColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dAutoFilterGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeColorImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dEncodeGrayImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageAutoFilterStrategy=/JPEG", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageAutoFilterStrategy=/JPEG", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dColorImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dGrayImageFilter=/DCTEncode", _parameterStrings, "Missing parameter.");

            Assert.IsFalse(_parameterStrings.Contains("-dDownsampleColorImages=true"), "Falsely set downsample parameter for colored images");
            Assert.IsFalse(_parameterStrings.Contains("-dDownsampleGrayImages=true"), "Falsely set downsample parameter for gray images");
        }

        [Test]
        public void ParametersTest_MonoImagesCompressionDisabled_ResamplingEnabled_ResamplingGetsBlocked()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Enabled = false;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Dpi = 123;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dEncodeMonoImages=false", _parameterStrings, "Missing parameter.");

            Assert.IsFalse(_parameterStrings.Contains("-dDownsampleMonoImages=true"), "Falsely set resample parameter");
            Assert.IsFalse(_parameterStrings.Contains("-dMonoImageDownsampleType=/Bicubic"), "Falsely set resample parameter");
            Assert.IsFalse(_parameterStrings.Contains("-dMonoImageResolution=" + 123), "Falsely set resample parameter");
        }

        [Test]
        public void ParametersTest_MonoImagesCcittFaxEncoding_ResamplingDisabled()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfA2B;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.CcittFaxEncoding;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Resampling = false;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Dpi = 123;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dEncodeMonoImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dMonoImageFilter=/CCITTFaxEncode", _parameterStrings, "Missing parameter.");

            Assert.IsFalse(_parameterStrings.Contains("-dDownsampleMonoImages=true"), "Falsely set resample parameter");
            Assert.IsFalse(_parameterStrings.Contains("-dMonoImageDownsampleType=/Bicubic"), "Falsely set resample parameter");
            Assert.IsFalse(_parameterStrings.Contains("-dMonoImageResolution=" + 123), "Falsely set resample parameter");
        }

        [Test]
        public void ParametersTest_MonoImagesRunLengthEncoding_Resampling300Dpi()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.PdfX;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.RunLengthEncoding;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Dpi = 300;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dEncodeMonoImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dMonoImageFilter=/RunLengthEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-dMonoImageDownsampleType=/Bicubic", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dMonoImageResolution=" + 300, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_MonoImagesZipResampling3Dpi_RaisedTo4()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.Zip;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Dpi = 3;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dEncodeMonoImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dMonoImageFilter=/FlateEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-dMonoImageDownsampleType=/Bicubic", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dMonoImageResolution=" + 4, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_MonoImagesZipResampling2401Dpi_LoweredTo2400()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Enabled = true;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Compression = CompressionMonochrome.Zip;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Resampling = true;
            _pdfDevice.Profile.PdfSettings.CompressMonochrome.Dpi = 2401;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains("-dEncodeMonoImages=true", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dMonoImageFilter=/FlateEncode", _parameterStrings, "Missing parameter.");

            Assert.Contains("-dMonoImageDownsampleType=/Bicubic", _parameterStrings, "Missing parameter.");
            Assert.Contains("-dMonoImageResolution=" + 2400, _parameterStrings, "Missing parameter.");
        }

        [Test]
        public void ParametersTest_CoverPage()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.CoverPage.Enabled = true;
            const string coverFile = "CoverFile.pdf";
            _pdfDevice.Profile.CoverPage.File = coverFile;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains(coverFile, _parameterStrings, "Missing parameter.");
            var fIndex = _parameterStrings.IndexOf("-f");
            Assert.Less(fIndex, _parameterStrings.IndexOf(coverFile), "CoverFile not behind -f parameter.");

            _pdfDevice.Profile.CoverPage.Enabled = false;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.AreEqual(-1, _parameterStrings.IndexOf(coverFile), "Falsely set CoverFile.");
        }

        [Test]
        public void ParametersTest_AttachmentPage()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.AttachmentPage.Enabled = true;
            const string attachmentFile = "AttachmentFile.pdf";
            _pdfDevice.Profile.AttachmentPage.File = attachmentFile;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            Assert.Contains(attachmentFile, _parameterStrings, "Missing parameter.");
            var fIndex = _parameterStrings.IndexOf("-f");
            Assert.Less(fIndex, _parameterStrings.IndexOf(attachmentFile), "AttachmentFile not behind -f parameter.");

            _pdfDevice.Profile.AttachmentPage.Enabled = false;
            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));
            Assert.AreEqual(-1, _parameterStrings.IndexOf(attachmentFile), "Falsely set AttachmentFile.");
        }

        [Test]
        public void ParametersTest_CoverAndAttachmentPage()
        {
            _pdfDevice.Profile.OutputFormat = OutputFormat.Pdf;
            _pdfDevice.Profile.CoverPage.Enabled = true;
            const string coverFile = "CoverFile.pdf";
            _pdfDevice.Profile.CoverPage.File = coverFile;
            _pdfDevice.Profile.AttachmentPage.Enabled = true;
            const string attachmentFile = "AttachmentFile.pdf";
            _pdfDevice.Profile.AttachmentPage.File = attachmentFile;

            _parameterStrings = new Collection<string>(_pdfDevice.GetGhostScriptParameters(_ghostscriptVersion));

            var coverIndex = _parameterStrings.IndexOf(coverFile);
            var attachmentIndex = _parameterStrings.IndexOf(attachmentFile);

            Assert.LessOrEqual(coverIndex - attachmentIndex, -2, "No further (file)parameter between cover and attachment file.");
        }

    }
}
