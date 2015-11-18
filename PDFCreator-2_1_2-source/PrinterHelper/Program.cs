using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace pdfforge.PrinterHelper
{
    public class Program
    {
        public const string PortName = "pdfcmon";
        public const string MonitorName = "pdfcmon";
        public const string DriverName = "PDFCreator";

        public IError Run(IDictionary<string, IList<string>> cmdlParams, ICommands commands, IWin32Printer win32Printer, ILogging logging)
        {
            var printerEnvironment = Printer.CurrentPrinterEnvironment;

            var error = commands.CheckCommands(cmdlParams, Commands.ValidCommands);
            if (error != null)
                return error;

            // Log printer infos
            logging.LogAllPrinterInfos();

            // AddPrinter
            error = commands.CheckCommandAddPrinterAndExecute(cmdlParams, win32Printer, printerEnvironment, PortName, DriverName, logging);
            if (error != null)
                return error;

            // RenamePrinter
            error = commands.CheckCommandRenamePrinterAndExecute(cmdlParams, win32Printer, logging);
            if (error != null)
                return error;

            // DeletePrinter
            error = commands.CheckCommandDeletePrinterAndExecute(cmdlParams, win32Printer, DriverName, logging);
            if (error != null)
                return error;

            // InstallPrinter
            error = commands.CheckCommandInstallPrinterAndExecute(cmdlParams, win32Printer, MonitorName, PortName, DriverName, logging);
            if (error != null)
                return error;

            // UnInstallPrinter
            error = commands.CheckCommandUnInstallPrinterAndExecute(cmdlParams, win32Printer, DriverName, MonitorName, logging);
            if (error != null)
                return error;

            logging.LogAllPrinterInfos();
            return null;
        }

        [STAThread]
        static void Main(string[] args)
        {
            var commands = new Commands();
            var win32Printer = new Win32Printer();

            var cmdlParams = CommandLineParser.ParseCommandLine(args, true, true);
            if (cmdlParams.Count == 0)
            {
                Usage();
                Environment.Exit(0);
            }

            // Log command
            ILogging logging;
            var error = commands.CheckLogging(cmdlParams, out logging);
            if (error != null)
            {
                if (logging.LogLevel == LogLevel.NoLogging)
                    logging = new Logging(LogLevel.LogToConsole, logging.LogFile);
                WriteErrorAndExit(error, logging);
            }

            try
            {
                error = new Program().Run(cmdlParams, commands, win32Printer, logging);
                if (error != null)
                {
                    if (logging.LogLevel == LogLevel.NoLogging)
                        logging = new Logging(LogLevel.LogToConsole, logging.LogFile);
                    WriteErrorAndExit(error, logging);
                }
            }
            catch (Exception exception)
            {
                // ReSharper disable LocalizableElement
                Console.WriteLine("Error:");
                Console.WriteLine(" Message = {0}", exception.Message);
                Console.WriteLine(" Source = {0}", exception.Source);
                Console.WriteLine(" StackTrace = {0}", exception.StackTrace);
                Console.WriteLine(" TargetSite = {0}", exception.TargetSite);
                // ReSharper restore LocalizableElement
            }

            if (Debugger.IsAttached)
                Console.Read();
        }

        private static void Usage()
        {
            Console.WriteLine(@"PrinterHelper " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + @"             © pdfforge");
            Console.WriteLine();
            Console.WriteLine(@"Usage:");
            Console.WriteLine(@"PrinterHelper.exe /AddPrinter <PrinterName1 PrinterName2 PrinterName3 ...>");
            Console.WriteLine(@"PrinterHelper.exe /RenamePrinter <PrinterName> <NewName>");
            Console.WriteLine(@"PrinterHelper.exe /DeletePrinter <PrinterName1 PrinterName2 PrinterName3 ...>");
            //Console.WriteLine(@"PrinterHelper.exe /InstallPrinter <PrinterName1 PrinterName2 PrinterName3 ...> /PortApplication <Path-to-port-application>");
            //Console.WriteLine(@"PrinterHelper.exe /UnInstallPrinter (Attention: Using this command uninstalls all PDFCreator printers!)");
        }

        private static void WriteErrorAndExit(IError error, ILogging logging)
        {
            logging.Log("Error: " + error.Message);

            if (error.Details != null)
            {
                foreach (var detail in error.Details)
                    logging.Log(" " + detail);
            }
            Environment.Exit(error.Code);
        }
    }
}
