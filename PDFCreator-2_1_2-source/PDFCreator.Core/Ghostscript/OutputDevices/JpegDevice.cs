using System;
using System.Collections.Generic;
using System.IO;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.Core.Ghostscript.OutputDevices
{
    /// <summary>
    ///     Extends OutputDevice to create PNG files
    /// </summary>
    public class JpegDevice : MultiFileDevice
    {
        protected override void AddDeviceSpecificParameters(IList<string> parameters)
        {
            switch (Profile.JpegSettings.Color)
            {
                case JpegColor.Gray8Bit:
                    parameters.Add("-sDEVICE=jpeggray");
                    break;
                default:
                    parameters.Add("-sDEVICE=jpeg");
                    break;
            }
            parameters.Add("-dJPEGQ=" + Profile.JpegSettings.Quality);
            parameters.Add("-r" + Profile.JpegSettings.Dpi);
            parameters.Add("-dTextAlphaBits=4");
            parameters.Add("-dGraphicsAlphaBits=4");
        }

        protected override void SetDeviceSpecificOutputFile(IList<string> parameters)
        {
            parameters.Add("-sOutputFile=" + Path.Combine(Job.JobTempOutputFolder, Job.JobTempFileName + "%d" + Path.GetExtension(Job.OutputFilenameTemplate)));
                //%d for multiple Pages
        }
    }
}