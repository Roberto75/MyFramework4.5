using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Pdf
{
    public class WordToPdf
    {


        //restiruisce il path del file PDF
        public FileInfo convert(FileInfo word, DirectoryInfo outputFolder)
        {

            if (!word.Exists)
            {
                throw new ApplicationException("File non trovato: " + word.FullName);
            }


            pdfforge.PDFCreator.Core.Ghostscript.GhostscriptVersion gsVersion = new pdfforge.PDFCreator.Core.Ghostscript.GhostscriptDiscovery().GetBestGhostscriptInstance("9.14");

            if (gsVersion == null)
            {
                throw new ApplicationException("Stampante Ghostscript non trovata");
            }


            pdfforge.PDFCreator.Core.Settings.ConversionProfile profile = new pdfforge.PDFCreator.Core.Settings.ConversionProfile();
            profile.OpenViewer = false;
            profile.OutputFormat = pdfforge.PDFCreator.Core.Settings.Enums.OutputFormat.Pdf;
            profile.PdfSettings.ColorModel = pdfforge.PDFCreator.Core.Settings.Enums.ColorModel.Rgb;
            //profile.JpegSettings.Color = pdfforge.PDFCreator.Core.Settings.Enums.JpegColor.Color24Bit;
            //profile.JpegSettings.Dpi = 
            

            int i = 1;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[" + i + "]");
            sb.AppendLine("SessionId=1");
            sb.AppendLine("WinStation=Console");
            sb.AppendLine("UserName=SampleUser1234");
            sb.AppendLine("ClientComputer=\\PC1");
            sb.AppendLine("SpoolFileName=" + word.FullName);
            sb.AppendLine("PrinterName=PDFCreator");
            sb.AppendLine("JobId=1");
            sb.AppendLine("DocumentTitle=Title");
            sb.AppendLine("");


            string tempFolder = Path.GetTempPath();
            string fileInf = Path.Combine(tempFolder, word.Name.Replace(word.Extension, ".inf"));
            File.WriteAllText(fileInf, sb.ToString(), Encoding.GetEncoding("Unicode"));

            pdfforge.PDFCreator.Core.Jobs.JobInfo jobInfo = new pdfforge.PDFCreator.Core.Jobs.JobInfo(fileInf);

            pdfforge.PDFCreator.Core.Jobs.JobTranslations jobTranslations = new pdfforge.PDFCreator.Core.Jobs.JobTranslations();
            jobTranslations.EmailSignature = "\r\n\r\nCreated with PDFCreator";

            pdfforge.PDFCreator.Core.Jobs.GhostscriptJob job = null;
            try
            {
                job = new pdfforge.PDFCreator.Core.Jobs.GhostscriptJob(jobInfo, profile, jobTranslations);
                job.AutoCleanUp = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }

            job.OutputFilenameTemplate = outputFolder.FullName + word.Name;

            job.RunJob();

            if (job.OutputFiles.Count > 1)
            {
                throw new ApplicationException("job.OutputFiles.Count > 1");
            }

            foreach (string s in job.OutputFiles)
            {
                Debug.WriteLine("OutputFile: " + s);
            }

            return new FileInfo(job.OutputFiles[0]);
        }

    }
}
