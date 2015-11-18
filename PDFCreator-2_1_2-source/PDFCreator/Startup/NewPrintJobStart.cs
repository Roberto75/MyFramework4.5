﻿using System.IO;
using NLog;
using pdfforge.PDFCreator.Core.Jobs;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Utilities.IO;

namespace pdfforge.PDFCreator.Startup
{
    public class NewPrintJobStart : MaybePipedStart
    {
        public string NewJobInfoFile { get; private set; }
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public NewPrintJobStart(string newJobInfoFile)
        {
            NewJobInfoFile = newJobInfoFile;
        }

        internal override string ComposePipeMessage()
        {
            EnsureJobFileIsInSpoolPath();
            
            return "NewJob|" + NewJobInfoFile;
        }

        internal override bool StartApplication()
        {
            if (string.IsNullOrEmpty(NewJobInfoFile) || !File.Exists(NewJobInfoFile))
            {
                _logger.Error("No file in InfoDataFile argument or file does not exist.");
                return false;
            }
            
            EnsureJobFileIsInSpoolPath();
            
            _logger.Debug("Adding new job");
            JobInfoQueue.Instance.Add(NewJobInfoFile);

            return true;
        }

        private void EnsureJobFileIsInSpoolPath()
        {
            // Move to spool folder, if the correct spool folder could not be determined for some reason
            if (!Path.GetFullPath(NewJobInfoFile).StartsWith(JobInfoQueue.Instance.SpoolFolder))
            {
                _logger.Debug(
                    "JobInfo file from printer is not in our spool folder ({0}) - we'll move it there",
                    JobInfoQueue.Instance.SpoolFolder);
                NewJobInfoFile = MoveSpoolFile(NewJobInfoFile, JobInfoQueue.Instance.SpoolFolder,
                    SettingsHelper.Settings.ApplicationSettings);
            }
        }

        private string MoveSpoolFile(string infFile, string spoolFolder, ApplicationSettings applicationSettings)
        {
            var ji = new JobInfo(infFile, applicationSettings.TitleReplacement);

            string jobName = Path.GetFileNameWithoutExtension(infFile);
            string jobFolder = Path.Combine(spoolFolder, jobName);

            jobFolder = new UniqueDirectory(jobFolder).MakeUniqueDirectory();
            Directory.CreateDirectory(jobFolder);

            foreach (var sourceFile in ji.SourceFiles)
            {
                string targetFile = Path.Combine(jobFolder, Path.GetFileName(sourceFile.Filename));
                File.Move(sourceFile.Filename, targetFile);
                sourceFile.Filename = Path.GetFileName(sourceFile.Filename);
            }

            var newInfFile = Path.Combine(jobFolder, jobName + ".inf");
            ji.SaveInf(newInfFile);

            File.Delete(infFile);

            return newInfFile;
        }
    }
}
