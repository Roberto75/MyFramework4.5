using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using pdfforge.PDFCreator.Core.Actions;
using pdfforge.PDFCreator.Core.Ghostscript;
using pdfforge.PDFCreator.Core.Ghostscript.OutputDevices;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Tokens;
using pdfforge.PDFProcessing;

namespace pdfforge.PDFCreator.Core.Jobs
{
    /// <summary>
    ///     The GhostscriptJob class processes a Job with Ghostscript.
    /// </summary>
    public class GhostscriptJob : AbstractJob
    {
        public const string MinGsVersion = "9.14";
        private readonly GhostScript _ghostScript;
        
        private readonly StringBuilder _ghostscriptOutput = new StringBuilder();

        private string GhostscriptOutput
        {
            get { return _ghostscriptOutput.ToString(); }
        }

        /// <summary>
        ///     Create a new GhostscriptJob instance
        /// </summary>
        /// <param name="jobInfo">jobInfo of the job to convert</param>
        /// <param name="profile">Profile that determines the conversion process</param>
        public GhostscriptJob(IJobInfo jobInfo, ConversionProfile profile, JobTranslations jobTranslations)
        {
            OutputFiles = new List<string>();
            JobActions = new List<IAction>();

            var gsVersion = new GhostscriptDiscovery().GetBestGhostscriptInstance(MinGsVersion);

            if (gsVersion == null)
            {
                Logger.Error("No valid Ghostscript version found.");
                throw new InvalidOperationException("No valid Ghostscript version found.");
            }

            Logger.Debug("Ghostscript Version: " + gsVersion.Version + " loaded from " + gsVersion.ExePath);
            _ghostScript = new GhostScript(gsVersion);

            JobTempFolder = Path.Combine(Path.Combine(Path.GetTempPath(), "PDFCreator.net"),
                "Job_" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
            JobTempOutputFolder = Path.Combine(JobTempFolder, "tempoutput");
            Directory.CreateDirectory(JobTempFolder);
            Directory.CreateDirectory(JobTempOutputFolder);

            Passwords = new JobPasswords();
            JobTranslations = jobTranslations;

            AutoCleanUp = true;

            JobInfo = jobInfo;

            Profile = profile;
            TokenReplacer = GetTokenReplacer(); //important for testing without workflow
        }

        public override void InitMetadata()
        {
            JobInfo.Metadata.Author = Profile.AuthorTemplate;
            JobInfo.Metadata.Title = Profile.TitleTemplate;
            ApplyMetadata();
        }

        public override void ApplyMetadata()
        {
            TokenReplacer = GetTokenReplacer();
            JobInfo.Metadata.Author = TokenReplacer.GetToken("Author").GetValue();
            JobInfo.Metadata.Title = TokenReplacer.GetToken("Title").GetValue();           
        }

        /// <summary>
        /// Creates a TokenReplacer based on the job data.
        /// </summary>
        /// <returns>The configured TokenReplacer</returns>
        private TokenReplacer GetTokenReplacer()
        {
            var tokenReplacer = new TokenReplacer();

            string titleFilename = "";
            string titleFolder = "";

            if (FileUtil.IsValidRootedPath(JobInfo.SourceFiles[0].DocumentTitle))
            {
                titleFilename = Path.GetFileNameWithoutExtension(JobInfo.SourceFiles[0].DocumentTitle);
                titleFolder = Path.GetDirectoryName(JobInfo.SourceFiles[0].DocumentTitle);
            }
            tokenReplacer.AddStringToken("ClientComputer", JobInfo.SourceFiles[0].ClientComputer);
            tokenReplacer.AddStringToken("ComputerName", Environment.MachineName);
            tokenReplacer.AddNumberToken("Counter", JobInfo.SourceFiles[0].JobCounter);
            tokenReplacer.AddDateToken("DateTime", DateTime.Now);
            tokenReplacer.AddStringToken("InputFilename", titleFilename);
            tokenReplacer.AddStringToken("InputFilePath", titleFolder);
            tokenReplacer.AddNumberToken("JobId", JobInfo.SourceFiles[0].JobId);
            tokenReplacer.AddListToken("OutputFilenames", new List<string>());
                //Set "real" values after converting (in AbstractJob.RunJob())
            tokenReplacer.AddStringToken("OutputFilePath", "");
                //Set "real" values after converting (in AbstractJob.RunJob()) 
            tokenReplacer.AddStringToken("PrinterName", JobInfo.SourceFiles[0].PrinterName);
            tokenReplacer.AddNumberToken("SessionId", JobInfo.SourceFiles[0].SessionId);
            tokenReplacer.AddStringToken("Username", Environment.UserName);
            tokenReplacer.AddToken(new EnvironmentToken("Environment"));
            tokenReplacer.AddStringToken("PrintJobAuthor", JobInfo.Metadata.PrintJobAuthor);
            tokenReplacer.AddStringToken("PrintJobName", JobInfo.Metadata.PrintJobName);
            tokenReplacer.AddStringToken("Author", tokenReplacer.ReplaceTokens(JobInfo.Metadata.Author));
            tokenReplacer.AddStringToken("Title", tokenReplacer.ReplaceTokens(JobInfo.Metadata.Title));

            return tokenReplacer;
        }

        /// <summary>
        ///     Determines the number of pages through all source files
        /// </summary>
        /// <returns>the number of pages in all source files</returns>
        protected override int CountPagesInSourceFiles()
        {
            int count = 0;

            foreach (SourceFileInfo file in JobInfo.SourceFiles)
            {
                count += CountPages(file.Filename);
            }

            return count;
        }

        /// <summary>
        ///     Determines the number of pages in a single source file
        /// </summary>
        /// <param name="fileName">full path of the PS file to process</param>
        /// <returns>The number of pages in the given source file</returns>
        private int CountPages(string fileName)
        {
            int count = 0;

            try
            {
                using (FileStream fs = File.OpenRead(fileName))
                {
                    var sr = new StreamReader(fs);

                    while (sr.Peek() >= 0)
                    {
                        var readLine = sr.ReadLine();
                        if (readLine != null && readLine.Contains("%%Page:"))
                            count++;
                    }
                }
            }
            catch
            {
                Logger.Warn("Error while retrieving page count");
            }

            return count;
        }

        /// <summary>
        ///     Apply all Actions according to the configuration
        /// </summary>
        protected virtual void SetUpActions()
        {
            // it does not work yet... 
            /*if (Profile.Stamping.Enable)
            {
                StampPdfAction stamp = new StampPdfAction(Profile.Stamping.StampText, Profile.Stamping.FontName);
                JobActions.Add(stamp);
            }*/
            
            if (Profile.Scripting.Enabled)
            {
                AddAction(new ScriptAction());
                Logger.Trace("Script-Action added");
            }

            if (Profile.OpenViewer)
            {
                var defaultViewerAction = new DefaultViewerAction(true);
                defaultViewerAction.RecommendPdfArchitect += OnRecommendPdfArchitect;
                AddAction(defaultViewerAction);

                Logger.Trace("Viewer-Action added");
            }

            if (Profile.Printing.Enabled)
            {
                AddAction(new PrintingAction(_ghostScript));
                Logger.Trace("Print-Action added");
            }

            if (Profile.EmailSmtp.Enabled)
            {
                var smtpMailAction = new SmtpMailAction(JobTranslations.EmailSignature);
                smtpMailAction.QueryRetypeSmtpPassword += OnRetypeSmtpPassword;
                AddAction(smtpMailAction);
                Logger.Trace("SMTP-Mail-Action added");
            }

            if (Profile.EmailClient.Enabled)
            {
                var eMailClientAction = new EMailClientAction(JobTranslations.EmailSignature);
                AddAction(eMailClientAction);
                Logger.Trace("EMail-Client-Action added");
            }

            if (Profile.Ftp.Enabled)
            {
                var ftpAction = new FtpAction();
                AddAction(ftpAction);
                Logger.Trace("Ftp-Action added");
            }

            /*if (Profile.AttachMe.Enable)
            {
                var attachMeAction = new AttachMeAction();
                AddAction(attachMeAction);
                Logger.Trace("Attach.Me added");
            }*/
        }

        /// <summary>
        ///     Run the Job
        /// </summary>
        protected override bool RunJobWork()
        {
            try
            {
                if (String.IsNullOrEmpty(Thread.CurrentThread.Name))
                    Thread.CurrentThread.Name = "JobWorker";
            }
            catch (InvalidOperationException)
            {
            }

            try
            {
                _ghostScript.Output += Ghostscript_Output;
                OnJobCompleted += (sender, args) => _ghostScript.Output -= Ghostscript_Output;

                OutputFiles.Clear();

                Logger.Trace("Setting up actions");
                SetUpActions();

                Logger.Debug("Starting Ghostscript Job");

                OutputDevice device;
                switch (Profile.OutputFormat)
                {
                    case OutputFormat.PdfA1B:
                    case OutputFormat.PdfA2B:
                    case OutputFormat.PdfX:
                    case OutputFormat.Pdf:
                        device = new PdfDevice();
                        break;

                    case OutputFormat.Png:
                        device = new PngDevice();
                        break;
                    case OutputFormat.Jpeg:
                        device = new JpegDevice();
                        break;
                    case OutputFormat.Tif:
                        device = new TiffDevice();
                        break;

                    default:
                        throw new Exception("Illegal OutputFormat specified");
                }

                device.Profile = Profile;
                device.Job = this;

                Logger.Trace("Output format is: {0}", Profile.OutputFormat.ToString());

                _ghostScript.Output += Ghostscript_Logging;
                var success = _ghostScript.Run(device);
                _ghostScript.Output -= Ghostscript_Logging;

                Logger.Trace("Finished Ghostscript execution");

                if (!success)
                {
                    string errorMessage = ExtractGhostscriptErrors(GhostscriptOutput);
                    Logger.Error("Ghostscript execution failed: " + errorMessage);
                    ErrorMessage = errorMessage;

                    return false;
                }

                ProcessOutput(device);

                Logger.Trace("Moving output files to final location");
                device.MoveOutputFiles();

                Logger.Trace("Finished Ghostscript Job");
                return true;
            }
            catch (ProcessingException)
            {
                throw;
            }
            catch (DeviceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("There was an error while converting the Job {0}: {1}", JobInfo.InfFile, ex);
                throw;
            }
        }

        public static string ExtractGhostscriptErrors(string ghostscriptOutput)
        {
            var lines = ghostscriptOutput.Split('\n');

            var sb = new StringBuilder();

            foreach (var line in lines)
            {
                if (line.StartsWith("GPL Ghostscript"))
                    continue;

                if (line.StartsWith("Copyright (C)"))
                    continue;

                if (line.StartsWith("This software comes with NO WARRANTY"))
                    continue;

                if (line.StartsWith("Loading"))
                    continue;

                if (line.StartsWith("%%"))
                    continue;

                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Process Ghostscript output to detect the progress
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void Ghostscript_Output(object sender, OutputEventArgs e)
        {
            string output = e.Output;

            const string pageMarker = "[Page: ";
            if (output.Contains("[LastPage]"))
            {
                ReportProgress(100);
            }
            else if (output.Contains(pageMarker))
            {
                int start = output.LastIndexOf(pageMarker, StringComparison.Ordinal);
                int end = output.IndexOf("]", start, StringComparison.Ordinal);
                if ((start >= 0) && (end > start))
                {
                    start += pageMarker.Length;
                    string page = output.Substring(start, end - start);
                    int pageNumber;
                    if (Int32.TryParse(page, out pageNumber))
                    {
                        ReportProgress((pageNumber*100)/PageCount);
                    }
                }
            }
        }

        /// <summary>
        ///     Process Ghostscript output to provide logging
        /// </summary>
        /// <param name="sender">Sending object</param>
        /// <param name="e">Event Arguments</param>
        private void Ghostscript_Logging(object sender, OutputEventArgs e)
        {
            _ghostscriptOutput.Append(e.Output);

            Logger.Debug(e.Output.TrimEnd( '\r', '\n' ));
        }

        public override event EventHandler<QueryPasswordEventArgs> OnRetypeSmtpPassword;
        public override event EventHandler OnRecommendPdfArchitect;

        private void ProcessOutput(OutputDevice device)
        {
            if (!PDFProcessor.ProcessingRequired(Profile))
                return;

            PDFProcessor.ProcessPDF(device.TempOutputFiles[0], Profile, Passwords);
        }
    }
}