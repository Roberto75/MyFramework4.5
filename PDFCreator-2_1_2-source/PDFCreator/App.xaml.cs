using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using NLog;
using pdfforge.PDFCreator.Assistants;
using pdfforge.PDFCreator.Core.Ghostscript;
using pdfforge.PDFCreator.Core.Jobs;
using pdfforge.PDFCreator.Helper;
using pdfforge.PDFCreator.Shared.Helper;
using pdfforge.PDFCreator.Shared.ViewModels;
using pdfforge.PDFCreator.Shared.Views;
using pdfforge.PDFCreator.Startup;
using pdfforge.PDFCreator.Threading;
using pdfforge.PDFCreator.Utilities.Communication;

namespace pdfforge.PDFCreator
{
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public App()
        {
            InitializeComponent();

            System.Windows.Forms.Application.EnableVisualStyles();
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var globalMutex = new GlobalMutex("PDFCreator-137a7751-1070-4db4-a407-83c1625762c7");
            globalMutex.Acquire();
            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Thread.CurrentThread.Name = "ProgramThread";

            try
            {
                RunApplication(e.Args);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("There was an error while starting PDFCreator:\r\n{0}\r\n{1}", ex.Message,
                    ex));

                string message = TranslationHelper.Instance.GetFormattedTranslation("Program", "ErrorWhileStarting",
                    "There was an error while starting PDFCreator:\r\n{0}\r\n{1}", ex.Message, ex);
                const string caption = "PDFCreator";
                MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Error);
            }
            finally
            {
                globalMutex.Release();
                Logger.Debug("Ending PDFCreator");
                Shutdown();
            }
        }

        private void RunApplication(string[] commandLineArguments)
        {
            //Must be done first to initilize the translator
            SettingsHelper.Init();
            
            // Check translations and Ghostscript. Exit if there is a problem
            CheckInstallation();

            AppStartFactory appStartFactory = new AppStartFactory();
            var appStart = appStartFactory.CreateApplicationStart(commandLineArguments);

            // PrintFile needs to be started before initializing the JonbInfoQueue
            if (appStart is PrintFileStart)
                {
                appStart.Run();
                    return;
                }

                Logger.Debug("Starting PDFCreator");

            if (commandLineArguments.Length > 0)
                {
                Logger.Info("Command Line parameters: \r\n" + String.Join(" ", commandLineArguments));
                }

            if (!InitializeJobInfoQueue())
                return;

            // Start the application
            appStart.Run();

            Logger.Debug("Waiting for all threads to finish");
            ThreadManager.Instance.WaitForThreadsAndShutdown(this);
        }

        private bool InitializeJobInfoQueue()
                {
            JobInfoQueue.Init();

            if (!JobInfoQueue.Instance.SpoolFolderIsAccessible())
                    {
                return TryRepairSpoolPath();
                        }

            return true;
                        }

        private void CheckInstallation()
        {
            if (TranslationHelper.Instance.TranslationPath == null)
            {
                MessageBox.Show(@"Could not find any translation. Please reinstall PDFCreator.",
                    @"Translations missing");
                Shutdown(1);
                }

            // Verfiy that Ghostscript is installed and exit if not
            EnsureGhoscriptIsInstalled();
        }

        private void EnsureGhoscriptIsInstalled()
        {
            if (!HasGhostscriptInstance())
            {
                Logger.Debug("No valid Ghostscript version found. Exiting...");
                var message = TranslationHelper.Instance.GetTranslation("ConversionWorkflow", "NoSupportedGSFound",
                    "Can't find a supported Ghostscript installation.\r\n\r\nProgram exiting now.");
                const string caption = @"PDFCreator";
                MessageWindow.ShowTopMost(message, caption, MessageWindowButtons.OK, MessageWindowIcon.Error);
                Environment.Exit(1);
            }
        }

        private bool TryRepairSpoolPath()
        {
            Logger.Error("The spool folder is not accessible due to a permission problem. PDFCreator will not work this way");

            string tempFolder = Path.GetFullPath(Path.Combine(JobInfoQueue.Instance.SpoolFolder, ".."));
            string username = Environment.UserName;

            Logger.Debug("Username is {0}", username);

            string title = TranslationHelper.Instance.GetTranslation("Application", "SpoolFolderAccessDenied", "Access Denied");
            string message = TranslationHelper.Instance.GetFormattedTranslation("Application", "SpoolFolderAskToRepair", "The temporary path where PDFCreator stores the print jobs can't be accessed. This is a configuration problem on your machine and needs to be fixed. Do you want PDFCreator to attempt repairing it?\r\nYour spool folder is: {0}", tempFolder);

            Logger.Debug("Asking to start repair..");
            if (MessageWindow.ShowTopMost(message, title, MessageWindowButtons.YesNo, MessageWindowIcon.Exclamation) == MessageWindowResponse.Yes)
            {
                string repairToolPath = AppDomain.CurrentDomain.BaseDirectory;
                repairToolPath = Path.Combine(repairToolPath, "RepairFolderPermissions.exe");

                string repairToolParameters = string.Format("\"{0}\" \"{1}\"", username, tempFolder);

                Logger.Debug("RepairTool path is: {0}", repairToolPath);
                Logger.Debug("Parameters: {0}", repairToolParameters);

                if (!File.Exists(repairToolPath))
                {
                    Logger.Error("RepairFolderPermissions.exe does not exist!");
                    title = TranslationHelper.Instance.GetTranslation("Application", "RepairToolNotFound", "RepairTool not found");
                    message = TranslationHelper.Instance.GetFormattedTranslation("Application", "SetupFileMissing",
                        "An important PDFCreator file is missing ('{0}'). Please reinstall PDFCreator!",
                        Path.GetFileName(repairToolPath));

                    MessageWindow.ShowTopMost(message, title, MessageWindowButtons.OK, MessageWindowIcon.Error);
                    return false;
                }

                Logger.Debug("Starting RepairTool...");
                var shellExecuteHelper = new ShellExecuteHelper();
                var result = shellExecuteHelper.RunAsAdmin(repairToolPath, repairToolParameters);
                Logger.Debug("Done: {0}", result.ToString());
            }

            Logger.Debug("Now we'll check again, if the spool folder is not accessible");
            if (!JobInfoQueue.Instance.SpoolFolderIsAccessible())
            {
                Logger.Info("The spool folder could not be repaired.");
                title = TranslationHelper.Instance.GetTranslation("Application", "SpoolFolderAccessDenied", "Access Denied");
                message = TranslationHelper.Instance.GetFormattedTranslation("Application", "SpoolFolderUnableToRepair", "PDFCreator was not able to repair your spool folder. Please contact your administrator or the support to assist you in changing the permissions of the path '{0}'.", tempFolder);

                MessageWindow.ShowTopMost(message, title, MessageWindowButtons.OK, MessageWindowIcon.Exclamation);
                return false;
            }

            Logger.Info("The spool folder was repaired successfully");

            return true;
        }

        private bool HasGhostscriptInstance()
        {
            GhostscriptVersion gsVersion = new GhostscriptDiscovery().GetBestGhostscriptInstance(GhostscriptJob.MinGsVersion);

            return gsVersion != null;
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception) e.ExceptionObject;
            Logger.FatalException(string.Format("Uncaught exception, IsTerminating: {0}", e.IsTerminating), ex);
            ErrorAssistant.ShowErrorWindowInNewProcess(ex);
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.ErrorException("Uncaught exception in WPF thread", e.Exception);

            e.Handled = true;

            bool terminateRequested;
            ErrorAssistant.ShowErrorWindow(e.Exception, out terminateRequested);

            if (terminateRequested)
            {
                Current.Shutdown(1);
            }
        }
    }
}
