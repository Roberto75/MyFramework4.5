using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfResize
{
    public class MyPdf
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private string _tempFileInf;

        private List<string> _outputFiles = new List<string>();

        private DirectoryInfo _sourceFolder;
        private DirectoryInfo _destinationFolder;

        private long _sizeMin;

        public void test()
        {

            logger.Info("");
            logger.Info(String.Format("PDF Resize ver. {0}", this.GetType().Assembly.GetName().Version.ToString()));

            pdfforge.PDFCreator.Core.Ghostscript.GhostscriptVersion gsVersion = new pdfforge.PDFCreator.Core.Ghostscript.GhostscriptDiscovery().GetBestGhostscriptInstance("9.14");

            init();

            pdfforge.PDFCreator.Core.Settings.ConversionProfile profile = new pdfforge.PDFCreator.Core.Settings.ConversionProfile();
            profile.OpenViewer = false;
            profile.OutputFormat = pdfforge.PDFCreator.Core.Settings.Enums.OutputFormat.Pdf;

            //COLOR AND GRAY
            profile.PdfSettings.CompressColorAndGray.Enabled = true;
            profile.PdfSettings.CompressColorAndGray.Compression = pdfforge.PDFCreator.Core.Settings.Enums.CompressionColorAndGray.JpegManual;
            profile.PdfSettings.CompressColorAndGray.JpegCompressionFactor = 25; //Define a custom compression factor (requires JpegManual as method)
            profile.PdfSettings.CompressColorAndGray.Resampling = false; // If true, the images will be resampled to a maximum resolution   
            profile.PdfSettings.CompressColorAndGray.Dpi = 8; //Images will be resampled to this maximum resolution of the images, if resampling is enabled
            
            //Monochrome
            profile.PdfSettings.CompressMonochrome.Enabled = true;
            profile.PdfSettings.CompressMonochrome.Compression = pdfforge.PDFCreator.Core.Settings.Enums.CompressionMonochrome.Zip;
            profile.PdfSettings.CompressMonochrome.Resampling = false;
            profile.PdfSettings.CompressMonochrome.Dpi = 24; // Images will be resampled to this maximum resolution of the images, if resampling is enabled

            //TIFF
            profile.TiffSettings.Color = pdfforge.PDFCreator.Core.Settings.Enums.TiffColor.Color24Bit;
            profile.TiffSettings.Dpi = 150;

            //PNG
            profile.PngSettings.Color = pdfforge.PDFCreator.Core.Settings.Enums.PngColor.Color24Bit;
            profile.PngSettings.Dpi = 150;

            //JPG
            profile.JpegSettings.Color = pdfforge.PDFCreator.Core.Settings.Enums.JpegColor.Color24Bit;
            profile.JpegSettings.Dpi = 150;
            //            profile.JpegSettings.Quality = 



            FileInfo[] pdfToProcess = _sourceFolder.GetFiles("*.pdf");
            FileInfo resizeFi;

            logger.Info(String.Format("Sono stati trovati {0:N0} files pdf", pdfToProcess.Length));

            string esito;
            decimal percentuale;
            string temp;

            for (int j = 0; j < pdfToProcess.Length; j++)
            {
                Debug.WriteLine(String.Format("{0:N0}/{1:N0} \t {2}", j + 1, pdfToProcess.Length, pdfToProcess[j].FullName));

                if (pdfToProcess[j].Length < _sizeMin)
                {
                
                    temp = String.Format("{0:D3}/{1:N0} \t {2} - skipped size < : {3:###,###} < {4:###,###} ", j + 1, pdfToProcess.Length, pdfToProcess[j].Name, pdfToProcess[j].Length, _sizeMin);
                    logger.Info(temp);
                    continue;
                }

                try
                {
                    resizeFi = resizePdf(pdfToProcess[j], profile);

                    if (pdfToProcess[j].Length > resizeFi.Length)
                    {
                        esito = "Ok";
                    }
                    else
                    {
                        esito = "Ko";
                    }

                    //almeno un mega 1048576
                    if (resizeFi.Length < 1048576)
                    {
                        esito = "Too small";
                    }


                    percentuale = resizeFi.Length * 100 / pdfToProcess[j].Length;

                    temp = String.Format("{0:D3}/{1:N0} \t {2} - {4:N0} --> {5:N0} -  [{3}] ", j + 1, pdfToProcess.Length, pdfToProcess[j].Name, esito, pdfToProcess[j].Length, resizeFi.Length);
                    temp += String.Format(" - {0}%", percentuale);

                    logger.Info(temp);
                }
                catch (Exception ex)
                {
                    logger.Error("Exception: " + ex.Message);
                }
            }
        }




        private void GenerateInfFileWithPsFiles()
        {

            string tempFolder = Path.GetTempPath();
            string TmpTestFolder = tempFolder;


            string TmpInfFile = Path.Combine(TmpTestFolder, "psFiles.inf");


            FileInfo fi = new FileInfo("../../../../pdf04.pdf");
            if (!fi.Exists)
            {
                throw new ApplicationException("File non trovato");

            }


            // TmpPsFiles = GeneratePSFileList(psFiles, TmpTestFolder);
            List<string> TmpPsFiles = new List<string>();
            TmpPsFiles.Add(fi.FullName);


            StringBuilder sb = new StringBuilder();

            for (int i = 1; i <= TmpPsFiles.Count; i++)
            {
                sb.AppendLine("[" + i + "]");
                sb.AppendLine("SessionId=1");
                sb.AppendLine("WinStation=Console");
                sb.AppendLine("UserName=SampleUser1234");
                sb.AppendLine("ClientComputer=\\PC1");
                sb.AppendLine("SpoolFileName=" + TmpPsFiles[i - 1]);
                sb.AppendLine("PrinterName=PDFCreator");
                sb.AppendLine("JobId=1");
                sb.AppendLine("DocumentTitle=Title");
                sb.AppendLine("");
            }

            File.WriteAllText(TmpInfFile, sb.ToString(), Encoding.GetEncoding("Unicode"));

            _tempFileInf = TmpInfFile;
        }



        private void init()
        {
            string temp;
            temp = System.Configuration.ConfigurationManager.AppSettings["source.folder"];
            _sourceFolder = new DirectoryInfo(temp);
            if (!_sourceFolder.Exists)
            {
                throw new ArgumentException("Editare il file App.config, source.folder non trovata: " + _sourceFolder.FullName);
            }
            logger.Info(String.Format("Source Folder: {0}", _sourceFolder.FullName));


            temp = System.Configuration.ConfigurationManager.AppSettings["destination.folder"];
            _destinationFolder = new DirectoryInfo(temp);
            if (!_destinationFolder.Exists)
            {
                throw new ArgumentException("Editare il file App.config, destination.folder non trovata: " + _destinationFolder.FullName);
            }
            logger.Info(String.Format("Destination Folder: {0}", _destinationFolder.FullName));

            if (_sourceFolder.FullName == _destinationFolder.FullName)
            {
                throw new ArgumentException("Editare il file App.config, source.folder deve essere differente da destination.folder");
            }


            temp = System.Configuration.ConfigurationManager.AppSettings["source.file.size.min"];
            _sizeMin = long.Parse(temp);

          //  logger.Info(String.Format("Source file min size (bytes): {0}", _sizeMin.ToString("###,###")));
            logger.Info(String.Format("Source file min size (bytes): {0:###,###}", _sizeMin));

        }





        private FileInfo resizePdf(FileInfo soucePdf, pdfforge.PDFCreator.Core.Settings.ConversionProfile profile)
        {
            int i = 1;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[" + i + "]");
            sb.AppendLine("SessionId=1");
            sb.AppendLine("WinStation=Console");
            sb.AppendLine("UserName=SampleUser1234");
            sb.AppendLine("ClientComputer=\\PC1");
            sb.AppendLine("SpoolFileName=" + soucePdf.FullName);
            sb.AppendLine("PrinterName=PDFCreator");
            sb.AppendLine("JobId=1");
            sb.AppendLine("DocumentTitle=Title");
            sb.AppendLine("");


            string tempFolder = Path.GetTempPath();
            string fileInf = Path.Combine(tempFolder, soucePdf.Name.Replace(".pdf", ".inf"));
            File.WriteAllText(fileInf, sb.ToString(), Encoding.GetEncoding("Unicode"));

            pdfforge.PDFCreator.Core.Jobs.JobInfo jobInfo = new pdfforge.PDFCreator.Core.Jobs.JobInfo(fileInf);

            pdfforge.PDFCreator.Core.Jobs.JobTranslations jobTranslations = new pdfforge.PDFCreator.Core.Jobs.JobTranslations();
            jobTranslations.EmailSignature = "\r\n\r\nCreated with PDFCreator";

            pdfforge.PDFCreator.Core.Jobs.GhostscriptJob job = new pdfforge.PDFCreator.Core.Jobs.GhostscriptJob(jobInfo, profile, jobTranslations);
            job.AutoCleanUp = false;
            //job.
            Debug.WriteLine(job.JobTempOutputFolder);
            Debug.WriteLine(job.JobTempFolder);


            job.OutputFilenameTemplate = _destinationFolder.FullName + soucePdf.Name;

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








        //private void _log(string message)
        //{
        //    Console.WriteLine(message);
        //    Debug.WriteLine(message);
        //    logger.Debug(message);

        //}


    }



}
