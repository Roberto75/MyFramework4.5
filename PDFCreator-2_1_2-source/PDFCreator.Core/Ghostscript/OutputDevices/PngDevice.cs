using System.Collections.Generic;
using System.IO;
using SystemInterface.IO;
using pdfforge.PDFCreator.Core.Settings.Enums;

namespace pdfforge.PDFCreator.Core.Ghostscript.OutputDevices
{
    /// <summary>
    ///     Extends OutputDevice to create PNG files
    /// </summary>
    public class PngDevice : MultiFileDevice
    {
        public PngDevice()
        {}

        public PngDevice(IFile file, IDirectory directory) : base(file, directory)
        {}

        protected override void AddDeviceSpecificParameters(IList<string> parameters)
        {
            switch (Profile.PngSettings.Color)
            {
                case PngColor.BlackWhite:
                    parameters.Add("-sDEVICE=pngmonod");
                    break;
                case PngColor.Color24Bit:
                    parameters.Add("-sDEVICE=png16m");
                    break;
                case PngColor.Color32BitTransp:
                    parameters.Add("-sDEVICE=pngalpha");
                    break;
                case PngColor.Color4Bit:
                    parameters.Add("-sDEVICE=png16");
                    break;
                case PngColor.Color8Bit:
                    parameters.Add("-sDEVICE=png256");
                    break;
                case PngColor.Gray8Bit:
                    parameters.Add("-sDEVICE=pnggray");
                    break;
            }
            parameters.Add("-r" + Profile.PngSettings.Dpi);
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