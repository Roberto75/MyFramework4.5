using System;
using System.IO;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Core.Ghostscript.OutputDevices
{
    public abstract class MultiFileDevice : OutputDevice
    {
        protected MultiFileDevice()
        { }
        
        protected MultiFileDevice(IFile file, IDirectory directory) : base(file, directory)
        { }

        public override void MoveOutputFiles()
        {   
            string outputDir = Path.GetDirectoryName(Job.OutputFilenameTemplate) ?? "";
            string filenameBase = Path.GetFileNameWithoutExtension(Job.OutputFilenameTemplate) ?? "output";
            string outfilebody = Path.Combine(outputDir, filenameBase);

            Job.OutputFiles = new string[TempOutputFiles.Count]; //reserve space

            foreach (string tempoutputfile in TempOutputFiles)
            {
                string extension = Path.GetExtension(TempOutputFiles[0]);

                string tempFileBase = Path.GetFileNameWithoutExtension(tempoutputfile) ?? "output";
                string num = tempFileBase.Replace(Job.JobTempFileName, "");

                int numValue;
                if (int.TryParse(num, out numValue))
                {
                    int numDigits = (int)Math.Floor(Math.Log10(TempOutputFiles.Count) + 1);
                    num = numValue.ToString("D" + numDigits);
                }

                string outputfile;
                if (num == "1")
                    outputfile = outfilebody + extension;
                else
                    outputfile = outfilebody + num + extension;

                var uniqueFileNameContinuance = false;
                if (Job.Profile.AutoSave.Enabled && Job.Profile.AutoSave.EnsureUniqueFilenames)
                {
                    var oldTemplate = outputfile;
                    outputfile = EnsureUniqueFilename(outputfile, false);
                    uniqueFileNameContinuance = oldTemplate != Job.OutputFilenameTemplate;
                }

                if (!CopyFile(tempoutputfile, outputfile))
                {
                    outputfile = EnsureUniqueFilename(outputfile, uniqueFileNameContinuance);

                    if (!CopyFile(tempoutputfile, outputfile))
                        //Throw exception affter second attempt to copy failes.
                        throw new DeviceException("Error while copying to target file in second attempt. Process gets canceled.", 2);
                }
                DeleteFile(tempoutputfile);
                Job.OutputFiles[Convert.ToInt32(num) - 1] = outputfile;
           }
        }
    }
}
