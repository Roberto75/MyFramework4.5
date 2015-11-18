using System.Collections.Generic;
using System.IO;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace pdfforge.PDFCreator.Core.Ghostscript.OutputDevices
{
    /// <summary>
    ///     Extends OutputDevice to create PNG files
    /// </summary>
    public class TiffDevice : OutputDevice
    {
        protected override void AddDeviceSpecificParameters(IList<string> parameters)
        {
            switch (Profile.TiffSettings.Color)
            {
                case TiffColor.BlackWhite:
                    parameters.Add("-sDEVICE=tiffg4"); // set back to tifflzw
                    break;
                case TiffColor.Gray8Bit:
                    parameters.Add("-sDEVICE=tiffgray");
                    break;
                case TiffColor.Color12Bit:
                    parameters.Add("-sDEVICE=tiff12nc");
                    break;
                case TiffColor.Color24Bit:
                default:
                    parameters.Add("-sDEVICE=tiff24nc");
                    break;
            }
            parameters.Add("-sCompression=lzw");
            parameters.Add("-r" + Profile.TiffSettings.Dpi);
            parameters.Add("-dTextAlphaBits=4");
            parameters.Add("-dGraphicsAlphaBits=4");
        }

        protected override void SetDeviceSpecificOutputFile(IList<string> parameters)
        {
            parameters.Add("-sOutputFile=" + Path.Combine(Job.JobTempOutputFolder, Job.JobTempFileName + Path.GetExtension(Job.OutputFilenameTemplate)));
                //%d for multiple Pages
        }
    }
}