                                                                           
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using SystemInterface.IO;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace pdfforge.PDFCreator.Core.Ghostscript.OutputDevices
{
    /// <summary>
    ///     Extends OutputDevice to create PDF files
    /// </summary>
    public class PdfDevice : OutputDevice
    {
        const int DpiMin = 4;
        const int DpiMax = 2400;

        public PdfDevice(){}

        public PdfDevice(IFile file) : base(file){}

        protected override void AddDeviceSpecificParameters(IList<string> parameters)
        {   
            parameters.Add("-sDEVICE=pdfwrite");
            parameters.Add("-dCompatibilityLevel=1.5");
            parameters.Add("-dPDFSETTINGS=/default");
            parameters.Add("-dEmbedAllFonts=true");

            if (!(Profile.OutputFormat.Equals(OutputFormat.PdfA2B) || Profile.OutputFormat.Equals(OutputFormat.PdfA1B))
                && Profile.PdfSettings.FastWebView)
            {
                parameters.Add("-dFastWebView=true");
            }

            SetPageOrientation(parameters, DistillerDictonaries);
            SetColorSchemeParameters(parameters);

            //ColorSheme must be defined before adding def files of PdfA/X
            if (Profile.OutputFormat == OutputFormat.PdfX)
                SetPdfXParameters(parameters);
            else if ((Profile.OutputFormat == OutputFormat.PdfA1B) 
                || (Profile.OutputFormat == OutputFormat.PdfA2B))
                    SetPdfAParameters(parameters);
            
            GrayAndColorImagesCompressionAndResample(parameters, DistillerDictonaries);
            MonoImagesCompression(parameters);
        }

        private void SetPdfAParameters(IList<string> parameters)
        {
            var shortenedTempPath = PathHelper.GetShortPathName(Job.JobTempFolder);

            if (Profile.OutputFormat == OutputFormat.PdfA1B)
                parameters.Add("-dPDFA=1");
            else
                parameters.Add("-dPDFA=2");
            //parameters.Add("-dNOOUTERSAVE"); //Set in pdf-A example, but is not documented in the distiller parameters

            Logger.Debug("Shortened Temppath from\r\n\"" + Job.JobTempFolder + "\"\r\nto\r\n\"" + shortenedTempPath + "\"");

            //Add ICC profile
            string iccFile = Path.Combine(shortenedTempPath, "profile.icc");
            //Set ICC Profile according to the color model 
            switch (Profile.PdfSettings.ColorModel)
            {
                case ColorModel.Cmyk:
                    File.WriteAllBytes(iccFile, CoreResources.WebCoatedFOGRA28);
                    break;
                case ColorModel.Gray:
                    File.WriteAllBytes(iccFile, CoreResources.ISOcoated_v2_grey1c_bas);
                    break;
                default:
                case ColorModel.Rgb:
                    File.WriteAllBytes(iccFile, CoreResources.eciRGB_v2);
                    break;
            }

            parameters.Add("-sOutputICCProfile=\"" + iccFile + "\"");

            string defFile = Path.Combine(Job.JobTempFolder, "pdfa_def.ps");
            var sb = new StringBuilder(CoreResources.PdfaDefinition);
            sb.Replace("[ICC_PROFILE]", "(" + EncodeGhostscriptParametersOctal(iccFile.Replace('\\', '/')) + ")");
            File.WriteAllText(defFile, sb.ToString());
            parameters.Add(defFile);
        }

        protected override void SetDeviceSpecificOutputFile(IList<string> parameters)
        {
            parameters.Add("-sOutputFile=" + Path.Combine(Job.JobTempOutputFolder, Job.JobTempFileName + Path.GetExtension(Job.OutputFilenameTemplate)));
        }

        private void SetPdfXParameters(IList<string> parameters)
        {
            var shortenedTempPath = PathHelper.GetShortPathName(Job.JobTempFolder);

            parameters.Add("-dPDFX");

            Logger.Debug("Shortened Temppath from\r\n\"" + Job.JobTempFolder + "\"\r\nto\r\n\"" + shortenedTempPath + "\"");

            //Add ICC profile
            string iccFile = Path.Combine(shortenedTempPath, "profile.icc");
            File.WriteAllBytes(iccFile, CoreResources.ISOcoated_v2_300_eci);
            parameters.Add("-sOutputICCProfile=\"" + iccFile + "\"");
            //parameters.Add("-dNOOUTERSAVE"); //Set in pdf-X example, but is not documented in the distiller parameters

            string defFile = Path.Combine(shortenedTempPath, "pdfx_def.ps");
            var sb = new StringBuilder(CoreResources.PdfxDefinition);
            sb.Replace("%/ICCProfile (ISO Coated sb.icc)", "/ICCProfile (" + EncodeGhostscriptParametersOctal(iccFile.Replace('\\', '/')) + ")");
            File.WriteAllText(defFile, sb.ToString());
            parameters.Add(defFile);
        }

        private void GrayAndColorImagesCompressionAndResample(IList<string> parameters,
            IList<string> distillerDictonaries)
        {
            if (!Profile.PdfSettings.CompressColorAndGray.Enabled)
            {
                parameters.Add("-dAutoFilterColorImages=false");
                parameters.Add("-dAutoFilterGrayImages=false");
                parameters.Add("-dEncodeColorImages=false");
                parameters.Add("-dEncodeGrayImages=false");
                return;
            }

            #region compress parameters

            switch (Profile.PdfSettings.CompressColorAndGray.Compression)
            {
                case CompressionColorAndGray.JpegMaximum:
                    parameters.Add("-dAutoFilterColorImages=false");
                    parameters.Add("-dAutoFilterGrayImages=false");
                    parameters.Add("-dEncodeColorImages=true");
                    parameters.Add("-dEncodeGrayImages=true");
                    parameters.Add("-dColorImageFilter=/DCTEncode");
                    parameters.Add("-dGrayImageFilter=/DCTEncode");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /ColorImageDict <</QFactor 2.4 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /GrayImageDict <</QFactor 2.4 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    break;

                case CompressionColorAndGray.JpegHigh:
                    parameters.Add("-dAutoFilterColorImages=false");
                    parameters.Add("-dAutoFilterGrayImages=false");
                    parameters.Add("-dEncodeColorImages=true");
                    parameters.Add("-dEncodeGrayImages=true");
                    parameters.Add("-dColorImageFilter=/DCTEncode");
                    parameters.Add("-dGrayImageFilter=/DCTEncode");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /ColorImageDict <</QFactor 1.3 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /GrayImageDict <</QFactor 1.3 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    break;

                case CompressionColorAndGray.JpegMedium:
                    parameters.Add("-dAutoFilterColorImages=false");
                    parameters.Add("-dAutoFilterGrayImages=false");
                    parameters.Add("-dEncodeColorImages=true");
                    parameters.Add("-dEncodeGrayImages=true");
                    parameters.Add("-dColorImageFilter=/DCTEncode");
                    parameters.Add("-dGrayImageFilter=/DCTEncode");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /ColorImageDict <</QFactor 0.76 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /GrayImageDict <</QFactor 0.76 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    break;

                case CompressionColorAndGray.JpegLow:
                    parameters.Add("-dAutoFilterColorImages=false");
                    parameters.Add("-dAutoFilterGrayImages=false");
                    parameters.Add("-dEncodeColorImages=true");
                    parameters.Add("-dEncodeGrayImages=true");
                    parameters.Add("-dColorImageFilter=/DCTEncode");
                    parameters.Add("-dGrayImageFilter=/DCTEncode");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /ColorImageDict <</QFactor 0.40 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /GrayImageDict <</QFactor 0.40 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    break;

                case CompressionColorAndGray.JpegMinimum:
                    parameters.Add("-dAutoFilterColorImages=false");
                    parameters.Add("-dAutoFilterGrayImages=false");
                    parameters.Add("-dEncodeColorImages=true");
                    parameters.Add("-dEncodeGrayImages=true");
                    parameters.Add("-dColorImageFilter=/DCTEncode");
                    parameters.Add("-dGrayImageFilter=/DCTEncode");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /ColorImageDict <</QFactor 0.15 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    distillerDictonaries.Add(
                        ".setpdfwrite << /GrayImageDict <</QFactor 0.15 /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    break;

                case CompressionColorAndGray.Zip:
                    parameters.Add("-dAutoFilterColorImages=false");
                    parameters.Add("-dAutoFilterGrayImages=false");
                    parameters.Add("-dEncodeColorImages=true");
                    parameters.Add("-dEncodeGrayImages=true");
                    parameters.Add("-dColorImageFilter=/FlateEncode");
                    parameters.Add("-dGrayImageFilter=/FlateEncode");
                    break;

                case CompressionColorAndGray.JpegManual:
                    parameters.Add("-dAutoFilterColorImages=false");
                    parameters.Add("-dAutoFilterGrayImages=false");
                    parameters.Add("-dEncodeColorImages=true");
                    parameters.Add("-dEncodeGrayImages=true");
                    parameters.Add("-dColorImageFilter=/DCTEncode");
                    parameters.Add("-dGrayImageFilter=/DCTEncode");
                    distillerDictonaries.Add(".setpdfwrite << /ColorImageDict <</QFactor " +
                                                Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor.ToString(CultureInfo.InvariantCulture) +
                                                " /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    distillerDictonaries.Add(".setpdfwrite << /GrayImageDict <</QFactor " +
                                                Profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor.ToString(CultureInfo.InvariantCulture) +
                                                " /Blend 1 /HSample [2 1 1 2] /VSample [2 1 1 2]>> >> setdistillerparams");
                    break;
                case CompressionColorAndGray.Automatic:
                default:
                    parameters.Add("-dAutoFilterColorImages=true");
                    parameters.Add("-dAutoFilterGrayImages=true");
                    parameters.Add("-dEncodeColorImages=true");
                    parameters.Add("-dEncodeGrayImages=true");
                    parameters.Add("-dColorImageAutoFilterStrategy=/JPEG");
                    parameters.Add("-dGrayImageAutoFilterStrategy=/JPEG");
                    parameters.Add("-dColorImageFilter=/DCTEncode");
                    parameters.Add("-dGrayImageFilter=/DCTEncode");
                    break;
                } //close switch

                #endregion

            #region resample parameters

            if (Profile.PdfSettings.CompressColorAndGray.Compression == CompressionColorAndGray.Automatic)
                return;

            if (Profile.PdfSettings.CompressColorAndGray.Resampling)
            {
                if (Profile.PdfSettings.CompressColorAndGray.Dpi < DpiMin)
                    Profile.PdfSettings.CompressColorAndGray.Dpi = DpiMin;
                else if (Profile.PdfSettings.CompressColorAndGray.Dpi > DpiMax)
                    Profile.PdfSettings.CompressColorAndGray.Dpi = DpiMax;

                parameters.Add("-dDownsampleColorImages=true");
                parameters.Add("-dColorImageResolution=" + Profile.PdfSettings.CompressColorAndGray.Dpi);
                parameters.Add("-dDownsampleGrayImages=true");
                parameters.Add("-dGrayImageResolution=" + Profile.PdfSettings.CompressColorAndGray.Dpi);
            }

            #endregion
        }

        private void MonoImagesCompression(IList<string> parameters)
        {
            if (!Profile.PdfSettings.CompressMonochrome.Enabled)
            {
                parameters.Add("-dEncodeMonoImages=false");
                return;
            }

            switch (Profile.PdfSettings.CompressMonochrome.Compression)
            {
                case CompressionMonochrome.CcittFaxEncoding:
                    parameters.Add("-dEncodeMonoImages=true");
                    parameters.Add("-dMonoImageFilter=/CCITTFaxEncode");
                    break;
                case CompressionMonochrome.RunLengthEncoding:
                    parameters.Add("-dEncodeMonoImages=true");
                    parameters.Add("-dMonoImageFilter=/RunLengthEncode");
                    break;
                case CompressionMonochrome.Zip:
                default:
                    parameters.Add("-dEncodeMonoImages=true");
                    parameters.Add("-dMonoImageFilter=/FlateEncode");
                    break;
            }

            if (Profile.PdfSettings.CompressMonochrome.Resampling)
            {
                if (Profile.PdfSettings.CompressMonochrome.Dpi < DpiMin)
                    Profile.PdfSettings.CompressMonochrome.Dpi = DpiMin;
                else if (Profile.PdfSettings.CompressMonochrome.Dpi > DpiMax)
                    Profile.PdfSettings.CompressMonochrome.Dpi = DpiMax;

                parameters.Add("-dDownsampleMonoImages=true");
                parameters.Add("-dMonoImageDownsampleType=/Bicubic");
                parameters.Add("-dMonoImageResolution=" + Profile.PdfSettings.CompressMonochrome.Dpi);
            }
        }

        private void SetColorSchemeParameters(IList<string> parameters)
        {
            //PDF/X only supports CMYK Colors
            if (Profile.OutputFormat == OutputFormat.PdfX)
                if (Profile.PdfSettings.ColorModel == ColorModel.Rgb)
                    Profile.PdfSettings.ColorModel = ColorModel.Cmyk;
            
            switch (Profile.PdfSettings.ColorModel)
            {
                case ColorModel.Cmyk:
                    parameters.Add("-sColorConversionStrategy=CMYK"); //Executes to execute the actual conversion to CMYK
                    parameters.Add("-dProcessColorModel=/DeviceCMYK");
                    break;
                case ColorModel.Gray:
                    parameters.Add("-sColorConversionStrategy=Gray"); //Executes the actual conversion to Gray
                    parameters.Add("-dProcessColorModel=/DeviceGray");
                    break;
                case ColorModel.Rgb:
                default:
                    if ((Profile.OutputFormat == OutputFormat.PdfA1B) || (Profile.OutputFormat == OutputFormat.PdfA2B))
                        parameters.Add("-sColorConversionStrategy=/UseDeviceIndependentColor"); 
                    else    
                        parameters.Add("-sColorConversionStrategy=RGB"); 
                    parameters.Add("-dProcessColorModel=/DeviceRGB");
                    parameters.Add("-dConvertCMYKImagesToRGB=true");
                    break;
            }
        }

        private void SetPageOrientation(IList<string> parameters, IList<string> distillerDictonaries)
        {   
            switch (Profile.PdfSettings.PageOrientation)
            {
                case PageOrientation.Landscape:
                    parameters.Add("-dAutoRotatePages=/None");
                    distillerDictonaries.Add("<</Orientation 3>> setpagedevice");
                    break;
                case PageOrientation.Automatic: 
                    parameters.Add("-dAutoRotatePages=/PageByPage");
                    parameters.Add("-dParseDSCComments=false"); //necessary for automatic rotation
                    break;
                //case  PageOrientation.Portrait:
                default:
                    parameters.Add("-dAutoRotatePages=/None");
                    distillerDictonaries.Add("<</Orientation 0>> setpagedevice");
                    break;
            }
        }
    }
}