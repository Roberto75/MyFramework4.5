using NLog;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.Startup
{
    public class AppStartFactory
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        public IAppStart CreateApplicationStart(string[] commandLineArgs)
        {
            CommandLineParser commandlineParser = new CommandLineParser(commandLineArgs);
            
            if (commandlineParser.HasArgument("PrintFile"))
            {
                return new PrintFileStart(commandlineParser);
            }

            MaybePipedStart appStart;

            // let's see if we have a new JobInfo passed as command line argument
            var newJob = FindJobInfoFile(commandlineParser);

            if (newJob != null)
            {
                appStart = new NewPrintJobStart(newJob);
            }
            else
            {
                appStart = new MainWindowStart();
            }

            if (commandlineParser.HasArgument("ManagePrintJobs"))
            {
                appStart.StartManagePrintJobs = true;
            }

            return appStart;
        }

        private string FindJobInfoFile(CommandLineParser commandlineParser)
        {
            string infFile = null;

            if (!commandlineParser.HasArgument("InfoDataFile") && !commandlineParser.HasArgument("PIF"))
                return null;

            if (commandlineParser.HasArgument("InfoDataFile"))
            {
                _logger.Info("Launched PDFCreator with InfoDataFile parameter.");
                infFile = commandlineParser.GetArgument("InfoDataFile");
            }

            if (infFile == null && commandlineParser.HasArgument("PIF"))
            {
                _logger.Info("Launched PDFCreator with PIF parameter.");
                infFile = commandlineParser.GetArgument("PIF");
            }

            _logger.Debug("Recevied \"" + infFile + "\" as command line parameter.");

            return infFile;
        }
    }
}
