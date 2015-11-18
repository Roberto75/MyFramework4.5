﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using NLog;
using pdfforge.DataStorage.Storage;
using pdfforge.PDFCreator.Core.Settings;
using pdfforge.PDFCreator.Core.Settings.Enums;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Licensing;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.ViewModels;
using pdfforge.PDFCreator.Shared.Views;
using pdfforge.PDFCreator.Threading;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.Utilities.Threading;
using pdfforge.PDFCreator.Views;

namespace pdfforge.PDFCreator.Assistants
{
    class UpdateAssistant
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly TranslationHelper TranslationHelper = TranslationHelper.Instance;
        private static UpdateAssistant _instance;

        public static Version CurrentVersion
        {
            get { return VersionHelper.GetCurrentApplicationVersion(); }
        }
        
        /// <summary>
        /// Get instance of UpdateManager.
        /// Has to be initialized
        /// </summary>
        public static UpdateAssistant Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UpdateAssistant();
                }
                return _instance;
            }
        }
        
        public delegate void VersionEventHandler(Object sender, UpdateEventArgs eventArgs);
        
        /// <summary>
        /// Event - launched if new version is available
        /// </summary>
        public event VersionEventHandler NewVersionEvent;
        
        /// <summary>
        /// Event - launched if no new version is available
        /// </summary>
        public event VersionEventHandler NoNewVersionEvent;
        
        /// <summary>
        /// Event - launched if error occurs in updateProcedure
        /// </summary>
        public event VersionEventHandler ErrorEvent;
        
        /// <summary>
        /// Event - launched as last step in the UpdateProcedure (Thread)
        /// (even if CheckNecessity skips the UpdateCheck)
        /// </summary>
        public event VersionEventHandler FinishedEvent;

        /// <summary>
        /// onlineVersion of the last UpdateProcedure
        /// </summary>
        public static ApplicationVersion OnlineVersion { get; private set; }
        
        /// <summary>
        /// Current time for the next automated updateCheck
        /// </summary>
        public static DateTime NextUpdate { get; private set; }
        
        private static UpdateInterval _updateInterval = UpdateInterval.Never;

        /// <summary>
        /// Flag to report, if UpdateProcedure(Thread) is already running
        /// </summary>
        public bool UpdateProcedureIsRunning { get; private set; }
        
        private bool _checkNecessity = true;

        /// <summary>
        /// Initialize with NextUpdate and UpdateInterval from Settings
        /// </summary>
        /// <param name="appSettings">Current ApplicationSettings</param>
        /// <param name="appProperties">Current ApplicationProperties</param>
        public void Initialize(ApplicationSettings appSettings, ApplicationProperties appProperties)
        {
            NextUpdate = appProperties.NextUpdate;
            _updateInterval = appSettings.UpdateInterval;
            _logger.Debug("UpdateManager initialised");
        }

        /// <summary>
        /// Initialize with NextUpdate and UpdateInterval from Settings
        /// </summary>
        /// <param name="settings">Current PdfCreatorSettings</param>
        public void Initialize(PdfCreatorSettings settings)
        {
            NextUpdate = settings.ApplicationProperties.NextUpdate; 
            _updateInterval = settings.ApplicationSettings.UpdateInterval;
            _logger.Debug("UpdateManager initialised");
        }

        /// <summary>
        /// Launch UpdateProcedure in separate Thread.
        /// UpdateManager must be initialized!
        /// Downloads update-info.txt and compares the recent (online) version to the current version
        /// and launches assigned events.
        /// Specific Events must be set in advance.
        /// Can only be launched once at the time, reported in UpdateProcedureIsRunning flag.
        /// Resets the UpdateManager afterwards.
        /// </summary>
        /// <param name="checkNecessity"></param>
        public void UpdateProcedure(bool checkNecessity)
        {
            _checkNecessity = checkNecessity;

            var thread = new SynchronizedThread(UpdateThread);
            thread.Name = "UpdateThread";
            thread.SetApartmentState(ApartmentState.STA);
            ThreadManager.Instance.StartSynchronizedThread(thread);
        }

        private void UpdateThread()
        {
            _logger.Debug("Launched UpdateThread");
            var eventArgs = new UpdateEventArgs();
            if (UpdateProcedureIsRunning)
            {
                _logger.Debug("UpdateThread is already running");
                return;
            }
            UpdateProcedureIsRunning = true;

            try
            {
                Update(eventArgs);
            }
            catch(Exception e)
            {
                _logger.Error("Exception in UpdateProcedure:\r\n" + e.Message);
                if (ErrorEvent != null)
                {
                    _logger.Debug("Launch ErrorEvent");
                    ErrorEvent(this, eventArgs);
                }
            }

            if (FinishedEvent != null)
            {
                _logger.Debug("Launch FinishedEvent");
                FinishedEvent(this, eventArgs);
            }

            UpdateProcedureIsRunning = false;

            _instance = null;
        }

        private void Update(UpdateEventArgs eventArgs)
        {
            if (_checkNecessity)
            {
                if (_updateInterval == UpdateInterval.Never)
                {
                    _logger.Debug("Automatic UpdateCheck is disabled");
                    return;
                }

                if (DateTime.Compare(DateTime.Now, NextUpdate) < 0)
                {
                    _logger.Debug("Update period did not pass. Next Update is set to: " + NextUpdate);
                    return;
                }
                _logger.Debug("Update period has passed (set to: " + NextUpdate +")");
            }
            
            _logger.Debug("Start UpdateCheck");

            Version thisVersion = CurrentVersion;
            OnlineVersion = GetOnlineVersion();
            if (OnlineVersion == null)
            {
                _logger.Error("OnlineVersion not available");
                return;
            }

            if (thisVersion.CompareTo(OnlineVersion.Version) < 0)
            {
                _logger.Info("New OnlineVersion available");
                if (NewVersionEvent != null)
                {
                    _logger.Debug("Launch NewVersionEvent");
                    NewVersionEvent(this, eventArgs);
                }
            }
            else
            {
                eventArgs.SetNewUpdateTime = true;
                if (NoNewVersionEvent != null)
                {
                    _logger.Debug("Launch NoNewVersionEvent");
                    NoNewVersionEvent(this, eventArgs);
                }
            }

            if (eventArgs.SetNewUpdateTime)
                SetNewUpdateTime();
            else
                NextUpdate = DateTime.Now.AddHours(2);
            _logger.Info("NextUpdate dated to " + NextUpdate);
        }

        private void SetNewUpdateTime()
        {   
            switch(_updateInterval)
            {
                default:
                    NextUpdate = DateTime.Now.AddDays(7);
                    break;
                case UpdateInterval.Daily:
                    NextUpdate = DateTime.Now.AddDays(1);
                    break;
                case UpdateInterval.Weekly:
                    NextUpdate = DateTime.Now.AddDays(7);
                    break;
                case UpdateInterval.Monthly:
                    NextUpdate = DateTime.Now.AddMonths(1);
                    break;
                case UpdateInterval.Never:
                    NextUpdate = DateTime.Now.AddYears(10);
                    break;
            }
        }

        private ApplicationVersion GetOnlineVersion()
        {
            _logger.Debug("Get online Version");

            string sectionName = "PDFCreator";
            string url = Urls.PdfCreatorUpdateInfoUrl;

            if (new LicenseValidator().IsValidLicense)
            {
                sectionName = "PDFCreatorPlus";
                url = Urls.PdfCreatorPlusUpdateInfoUrl;
            }

            using (var webClient = new WebClient())
            {
                string contents = webClient.DownloadString(url);
                using (var stream = CreateStreamFromString(contents))
                {
                    _logger.Debug("Loading update-info.txt");
                    DataStorage.Data data = DataStorage.Data.CreateDataStorage();
                    var iniStorage = new IniStorage();
                    iniStorage.SetData(data);
                    iniStorage.ReadData(stream, true);

                    var onlineVersion = new Version(iniStorage.Data.GetValue(sectionName + "\\Version"));
                    string downloadUrl = iniStorage.Data.GetValue(sectionName + "\\DownloadUrl");
                    string fileHash = iniStorage.Data.GetValue(sectionName + "\\FileHash");
                    _logger.Info("Online Version: " + onlineVersion);

                    return new ApplicationVersion(onlineVersion, downloadUrl, fileHash);
                }
            }
        }

        private Stream CreateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static void LaunchNewUpdateForm(Object sender, UpdateEventArgs eventArgs)
        {
            string caption = TranslationHelper.GetFormattedTranslation("UpdateManager", "PDFCreatorUpdate",
                "PDFCreator Update");
            string message = TranslationHelper.GetFormattedTranslation("UpdateManager", "NewUpdateMessage",
                "The new version [{0}] is available.\r\nWould you like to download the new version?", OnlineVersion.Version);

            var response = MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.YesLaterNo, MessageWindowIcon.PDFCreator);
            if (response == MessageWindowResponse.Yes)
            {
                if (new LicenseValidator().IsValidLicense)
                {
                    try
                    {
                        bool done = false;

                        while (!done)
                        {
                            var downloadWindow = new UpdateDownloadWindow(OnlineVersion.DownloadUrl);
                            bool? result = TopMostHelper.ShowDialogTopMost(downloadWindow, true);

                            if (result != true)
                            {
                                done = true;
                                continue;
                            }

                            if (FileUtil.VerifyMd5(downloadWindow.DownloadedFile, OnlineVersion.FileHash))
                            {
                                done = true;
                                ThreadManager.Instance.UpdateAfterShutdownAction = () => Process.Start(downloadWindow.DownloadedFile);
                                continue;
                            }

                            message = TranslationHelper.GetFormattedTranslation("UpdateManager", "DownloadHashErrorMessage",
                                "The MD5 hash of the downloaded file does not match the expected hash. It is possible that the file has been damaged during the download. Do you want to retry the download?", OnlineVersion.Version);
                            var res = MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.YesNo, MessageWindowIcon.Warning);

                            if (res != MessageWindowResponse.No)
                                continue;

                            ThreadManager.Instance.UpdateAfterShutdownAction = () => Process.Start(downloadWindow.DownloadedFile);
                            done = true;
                        }
                    }
                    catch (Exception)
                    {
                        message = TranslationHelper.GetFormattedTranslation("UpdateManager", "DownloadErrorMessage",
                "There was a problem during the download. Do you want to download the update from the website instead?", OnlineVersion.Version);
                        var res = MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.YesNo, MessageWindowIcon.PDFCreator);

                        if (res == MessageWindowResponse.Yes)
                        {
                            Process.Start(OnlineVersion.DownloadUrl);
                        }
                    }
                }
                else
                {
                    Process.Start(Urls.PdfCreatorDownloadUrl);
                }
            }
                
            if (response != MessageWindowResponse.Later)
                eventArgs.SetNewUpdateTime = true;
        }

        public static void LaunchNoUpdateForm(Object sender, UpdateEventArgs eventArgs)
        {
            string caption = TranslationHelper.GetFormattedTranslation("UpdateManager", "PDFCreatorUpdate",
                "PDFCreator Update");
            String message = TranslationHelper.GetTranslation("UpdateManager", "NoUpdateMessage", "You already have the most recent version.");
            MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.PDFCreator);
        }

        public static void LaunchErrorForm(Object sender, UpdateEventArgs eventArgs)
        {
            string caption = TranslationHelper.GetFormattedTranslation("UpdateManager", "PDFCreatorUpdate",
                "PDFCreator Update");
            String message = TranslationHelper.GetTranslation("UpdateManager", "ErrorMessage", "Failure in update-process.\r\nPlease check your internet-connection and retry later.");
            MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Error);
        }
    }

    class ApplicationVersion
    {
        public Version Version { get; private set; }
        public string DownloadUrl { get; private set; }
        public string FileHash { get; private set; }

        public ApplicationVersion(Version version, string downloadUrl, string fileHash)
        {
            Version = version;
            DownloadUrl = downloadUrl;
            FileHash = fileHash;
        }
    }
}