using System;
using System.IO;
using System.Linq;
using NLog;
using pdfforge.PDFCreator.Assistants;
using pdfforge.PDFCreator.Utilities;

namespace pdfforge.PDFCreator.Startup
{
    public class PrintFileStart : IAppStart
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly CommandLineParser _commandLineParser;

        public PrintFileStart(CommandLineParser commandLineParser)
        {
            _commandLineParser = commandLineParser;
        }

        public bool Run()
        {
            _logger.Info("Launched printjob with PrintFile command.");
            string filename = _commandLineParser.GetArgument("PrintFile");

            if (String.IsNullOrEmpty(filename))
            {
                _logger.Error("PrintFile Parameter has no argument");
                return false;
            }

            if (!File.Exists(filename))
            {
                _logger.Error(string.Format("The file \"{0}\" does not exist!", filename));
                return false;
            }

            var printFileAssistant = new PrintFileAssistant();

            string printerName = printFileAssistant.DefaultPrinter;

            if (_commandLineParser.HasArgument("PrinterName"))
            {
                string printer = _commandLineParser.GetArgument("PrinterName");
                var printerHelper = new Shared.Helper.PrinterHelper();
                if (printerHelper.GetPDFCreatorPrinters().Any(p => p.Equals(printer, StringComparison.InvariantCultureIgnoreCase)))
                {
                    printerName = printer;
                }
                else
                {
                    _logger.Warn("The Printer {0} is no PDFCreator printer!", printer);
                }

                _logger.Info("Using printer {0}", printerName);
            }

            if (!printFileAssistant.AddFile(filename, printerName))
            {
                _logger.Warn(string.Format("The file \"{0}\" is not printable!", filename));
                return false;
            }

            if (!printFileAssistant.PrintAll())
            {
                _logger.Error(string.Format("The file \"{0}\" could not be printed!", filename));
                return false;
            }

            return true;
        }
    }
}
