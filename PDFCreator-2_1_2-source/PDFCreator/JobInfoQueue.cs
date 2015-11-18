﻿using System;
using System.Collections.Generic;
using System.Text;
using pdfforge.PDFCreator.Core.Jobs;
using System.IO;
using System.Resources;
using System.Diagnostics;
using pdfforge.PDFCreator.Core;
using NLog;
using pdfforge.PDFCreator.Threading;
using pdfforge.PDFCreator.Helper;
using Microsoft.Win32;
using pdfforge.PDFCreator.Assistants;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Communication;

namespace pdfforge.PDFCreator
{
    /// <summary>
    /// The JobInfoQueue manages the pending JobInfos that are waiting to be converted
    /// </summary>
    public class JobInfoQueue : IJobInfoQueue
    {
        public IList<IJobInfo> JobInfos { get; private set; }

        private readonly HashSet<string> _jobFileSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public event EventHandler<NewJobInfoEventArgs> OnNewJobInfo;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static JobInfoQueue _instance;

        /// <summary>
        /// The singleton JobInfoQueue instance
        /// </summary>
        public static JobInfoQueue Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new JobInfoQueue();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Get the number ob items in the JobInfo Queue
        /// </summary>
        public int Count
        {
            get { return JobInfos.Count; }
        }

        /// <summary>
        /// Get the next pending job. If this is null, the queue is empty
        /// </summary>
        public IJobInfo NextJob
        {
            get
            {
                if (IsEmpty)
                    return null;

                return JobInfos[0];
            }
        }

        /// <summary>
        /// Determines if the Queue is emtpy
        /// </summary>
        /// <returns>true, if the Queue is empty</returns>
        public bool IsEmpty
        {
            get { return JobInfos.Count == 0; }
        }

        /// <summary>
        /// Gets the folder in which the print jobs will be stored
        /// </summary>
        public string SpoolFolder
        {
            get { return Path.Combine(Path.GetTempPath(), @"PDFCreator\Spool"); }
        }

        private JobInfoQueue()
        {
            JobInfos = new List<IJobInfo>();
            _logger.Debug("Spool folder is '{0}'", SpoolFolder);
        }

        public void FindSpooledJobs()
        {
            FindSpooledJobs(SpoolFolder);
        }

        private void FindSpooledJobs(string folder)
        {
            _logger.Debug("Looking for spooled jobs in '{0}'", folder);
            
            if (!Directory.Exists(folder))
                return;

            _logger.Debug("Searching for spooled jobs");
            foreach (string f in Directory.GetFiles(folder, "*.inf", SearchOption.AllDirectories))
            {
                try
                {
                    _logger.Debug("Found job info: " + f);
                    Add(f);
                }
                catch (Exception ex)
                {
                    _logger.WarnException(string.Format("There was an error while adding the print job '{0}': ", f), ex);
                }
            }
        }

        /// <summary>
        /// Initialize the Instance of the JobInfoQueue. This can be called to ensure that the instance exists.
        /// </summary>
        public static void Init()
        {
            if (_instance != null)
                _instance = new JobInfoQueue();
        }

        /// <summary>
        /// Reads a JobInfo from the given file and adds it to the end of the JobInfo Queue
        /// </summary>
        /// <param name="jobInfo">The JobInfo file to read and add</param>
        public void Add(string jobInfo)
        {
            var ji = new JobInfo(jobInfo, SettingsHelper.Settings.ApplicationSettings.TitleReplacement);
            if (ji.SourceFiles.Count > 0)
                Add(ji);
        }

        /// <summary>
        /// Appends an item to the end of the JobInfo Queue
        /// </summary>
        /// <param name="jobInfo">The JobInfo to add</param>
        public void Add(IJobInfo jobInfo)
        {
            string jobFile = Path.GetFullPath(jobInfo.InfFile);
            _logger.Debug("New JobInfo: " + jobFile);
            _logger.Debug("DocumentTitle: " + jobInfo.SourceFiles[0].DocumentTitle);
            _logger.Debug("ClientComputer: " + jobInfo.SourceFiles[0].ClientComputer);
            _logger.Debug("SessionId: " + jobInfo.SourceFiles[0].SessionId);
            _logger.Debug("PrinterName: " + jobInfo.SourceFiles[0].PrinterName);
            _logger.Debug("JobCounter: " + jobInfo.SourceFiles[0].JobCounter);
            _logger.Debug("JobId: " + jobInfo.SourceFiles[0].JobId);

            if (_jobFileSet.Contains(jobFile))
                return;

            _logger.Debug("Added JobInfo: " + jobFile);
            JobInfos.Add(jobInfo);
            _jobFileSet.Add(jobFile);

            if (OnNewJobInfo != null)
            {
                OnNewJobInfo(null, new NewJobInfoEventArgs(jobInfo));
            }
        }

        /// <summary>
        /// Removes a JobInfo from the Queue
        /// </summary>
        /// <param name="jobInfo">The JobInfo to remove</param>
        /// <returns>true, if successful</returns>
        public bool Remove(IJobInfo jobInfo)
        {
            return Remove(jobInfo, false);
        }

        /// <summary>
        /// Removes a JobInfo from the Queue
        /// </summary>
        /// <param name="jobInfo">The JobInfo to remove</param>
        /// <param name="deleteFiles">If true, the inf and source files will be deleted</param>
        /// <returns>true, if successful</returns>
        public bool Remove(IJobInfo jobInfo, bool deleteFiles)
        {
            _jobFileSet.Remove(jobInfo.InfFile);

            if (deleteFiles)
                jobInfo.Delete(deleteFiles);

            return JobInfos.Remove(jobInfo);
        }

        /// <summary>
        /// Add event handler to the pipe server. This allows the Queue to listen for new messages and catch new jobs to place them in the queue
        /// </summary>
        /// <param name="pipeServer">The PipeServer object to use</param>
        public void AddEventHandler(PipeServer pipeServer)
        {
            pipeServer.OnNewMessage += pipeServer_OnNewMessage;
        }

        private void pipeServer_OnNewMessage(object sender, MessageEventArgs e)
        {
            _logger.Debug("New Message received: " + e.Message);
            if (e.Message.StartsWith("NewJob|", StringComparison.InvariantCultureIgnoreCase))
            {
                string file = e.Message.Substring(7);
                try
                {
                    _logger.Debug("NewJob found: " + file);
                    if (File.Exists(file))
                    {
                        _logger.Trace("The given file exists");
                        var jobInfo = new JobInfo(file, SettingsHelper.Settings.ApplicationSettings.TitleReplacement);
                        Add(jobInfo);
                    }
                }
                catch
                {
                }
            }
            else if (e.Message.StartsWith("ShowMain|", StringComparison.InvariantCultureIgnoreCase))
            {
                ThreadManager.Instance.StartMainWindowThread();
            }
        }

        public void AddTestPage()
        {
            var rm = new ResourceManager(typeof (Properties.Resources));
            var sb = new StringBuilder(rm.GetString("TestPage"));
            // HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion -> ProductName

            sb.Replace("[INFOTITLE]", "PDFCreator " + UpdateAssistant.CurrentVersion);
            sb.Replace("[INFODATE]", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            sb.Replace("[INFOAUTHORS]", "pdfforge");
            sb.Replace("[INFOHOMEPAGE]", Urls.PdfforgeWebsiteUrl);
            sb.Replace("[INFOPDFCREATOR]", "PDFCreator " + UpdateAssistant.CurrentVersion);

            sb.Replace("[INFOCOMPUTER]", Environment.MachineName);
            sb.Replace("[INFOWINDOWS]", OsHelper.GetWindowsVersion());
            sb.Replace("[INFO64BIT]", OsHelper.Is64BitOperatingSystem.ToString());

            string tempPath = Path.Combine(SpoolFolder, Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            File.WriteAllText(Path.Combine(tempPath, "testpage.ps"), sb.ToString());

            sb = new StringBuilder();

            sb.AppendLine("[0]");
            sb.AppendLine("SessionId=" + Process.GetCurrentProcess().SessionId);
            sb.AppendLine("WinStation=Console");
            sb.AppendLine("UserName=" + Environment.UserName);
            sb.AppendLine("ClientComputer=" + Environment.MachineName);
            sb.AppendLine("SpoolFileName=testpage.ps");
            sb.AppendLine("PrinterName=PDFCreator");
            sb.AppendLine("JobId=1");
            sb.AppendLine("DocumentTitle=PDFCreator Testpage");
            sb.AppendLine("");

            string infFile = Path.Combine(tempPath, "testpage.inf");
            File.WriteAllText(infFile, sb.ToString(), Encoding.Unicode);

            var jobInfo = new JobInfo(infFile, SettingsHelper.Settings.ApplicationSettings.TitleReplacement);
            Add(jobInfo);
        }

        /// <summary>
        /// Removes all files and subdirectories (including all files contained in them) from the given temporary folders, that are older than one day.
        /// Use with caution!
        /// </summary>
        public void CleanTempFiles()
        {
            TimeSpan ts = TimeSpan.FromDays(1);
            CleanFolder(SpoolFolder, ts);
            CleanFolder(Path.Combine(Path.GetTempPath(), "PDFCreator.net"), ts);
        }

        /// <summary>
        /// Removes all files and subdirectories (including all files contained in them) from the given folder, that are older than given TimeSpan.
        /// Use with caution!
        /// </summary>
        /// <param name="folder">The folder to clean up</param>
        /// <param name="minAge">Minimum minAge of the files in order to be deleted</param>
        private void CleanFolder(string folder, TimeSpan minAge)
        {
            if (!Directory.Exists(folder))
                return;

            try
            {
                var folders = new DirectoryInfo(folder).GetDirectories();
                foreach (var dir in folders)
                {
                    if ((DateTime.UtcNow - dir.CreationTimeUtc > minAge) || FileUtil.DirectoryIsEmpty(dir.FullName))
                    {
                        Directory.Delete(dir.FullName, true);
                    }
                }

                var files = new DirectoryInfo(folder).GetFiles();
                foreach (var file in files)
                {
                    if (DateTime.UtcNow - file.CreationTimeUtc > minAge)
                    {
                        File.Delete(file.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Debug("Exception while cleaning up: " + ex);
            }
        }

        public bool SpoolFolderIsAccessible()
        {
            try
            {
                if (!Directory.Exists(SpoolFolder))
                {
                    Directory.CreateDirectory(SpoolFolder);
                }

                Directory.GetAccessControl(SpoolFolder);

                foreach (var directory in Directory.EnumerateDirectories(SpoolFolder, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        _logger.Debug("Checking directory " + directory);
                        Directory.GetAccessControl(directory);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.Info("Exception while checking spool folder: " + ex);
                    }
                }

                /*
                foreach (var file in Directory.EnumerateFiles(SpoolFolder, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        _logger.Debug("Checking file " + file);
                        File.GetAccessControl(file);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.Info("Exception while checking spool folder: " + ex);
                    }
                }*/
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.Debug("The spool folder seems to be broken: " + ex);
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// EventArgs class that contains the new JobInfo
    /// </summary>
    public class NewJobInfoEventArgs : EventArgs
    {
        public IJobInfo JobInfo { get; private set; }

        public NewJobInfoEventArgs(IJobInfo jobInfo)
        {
            JobInfo = jobInfo;
        }
    }
}