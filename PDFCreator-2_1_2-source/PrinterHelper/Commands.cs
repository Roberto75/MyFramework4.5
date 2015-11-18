using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pdfforge.PrinterHelper
{
    public class Commands : ICommands
    {
        public IError CheckCommands(IDictionary<string, IList<string>> commandDictionary, IList<string> validCommands)
        {
            var invalidCommands = new List<string>();

            if (commandDictionary == null || commandDictionary.Count == 0)
                return new Error(Error.ERROR_INVALID_COMMAND, Messages.FoundNoCommands);

            if (validCommands == null || validCommands.Count == 0)
                return new Error(Error.ERROR_INVALID_COMMAND, Messages.FoundNoValidCommandsCombination);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var command in commandDictionary)
            {
                if (!validCommands.Contains(command.Key, StringComparer.CurrentCultureIgnoreCase))
                    invalidCommands.Add(command.Key);
            }
            if (invalidCommands.Count != 0)
                return new Error(Error.ERROR_INVALID_COMMAND, Messages.FoundInvalidCommands, invalidCommands);

            if (commandDictionary.ContainsKey("addprinter"))
                return CheckCommandsCount(commandDictionary, 2);

            if (commandDictionary.ContainsKey("renameprinter"))
                return CheckCommandsCount(commandDictionary, 2);

            if (commandDictionary.ContainsKey("deleteprinter"))
                return CheckCommandsCount(commandDictionary, 2);

            var commandName = "uninstallprinter";
            if (commandDictionary.ContainsKey(commandName))
            {
                if (commandDictionary[commandName] != null && commandDictionary[commandName].Count > 0)
                    return new Error(Error.ERROR_INVALID_COMMAND, String.Format(Messages.CommandDoesntSupportParameters, commandName), commandDictionary[commandName]);
                return CheckCommandsCount(commandDictionary, 2);
            }

            commandName = "installprinter";
            const string commandPortApplication = "portapplication";
            const string commandPpdFile = "ppdfile";
            if (commandDictionary.ContainsKey(commandName))
            {
                if (!commandDictionary.ContainsKey(commandPortApplication))
                    return new Error(Error.ERROR_INVALID_COMMAND, Messages.NoPortApplicationSpecified);

                if (commandDictionary.ContainsKey(commandPpdFile))
                    return CheckCommandsCount(commandDictionary, 4);
                return CheckCommandsCount(commandDictionary, 3);
            }

            return new Error(Error.ERROR_INVALID_COMMAND, Messages.FoundUnknownCommandsCombination);
        }

        public IError CheckCommandsCount(IDictionary<string, IList<string>> commandDictionary, int countOfValidCommands)
        {
            var invalidCommandsCombination = new List<string>();
            if ((commandDictionary.Count == countOfValidCommands && commandDictionary.ContainsKey("log")) || (commandDictionary.Count == countOfValidCommands - 1))
                return null;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var command in commandDictionary)
                invalidCommandsCombination.Add(command.Key);
            return new Error(Error.ERROR_INVALID_COMMAND, Messages.FoundInvalidCommandsCombination, invalidCommandsCombination);
        }

        public IError CheckLogging(IDictionary<string, IList<string>> cmdlParams, out ILogging logging)
        {
            logging = new Logging(LogLevel.NoLogging, "");
            const string command = "log";
            if (cmdlParams.ContainsKey(command))
            {
                if (cmdlParams[command] == null || cmdlParams[command].Count == 0)
                    logging = new Logging(LogLevel.LogToConsole, "");
                else
                {
                    if (cmdlParams[command].Count == 1)
                    {
                        try
                        {
                            // Check for valid path
                            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                            Path.GetFullPath(cmdlParams[command][0]);
                        }
                        catch (Exception)
                        {
                            return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PathPortApplicationNotValid);
                        }
                        logging = new Logging(LogLevel.LogToFile, cmdlParams[command][0]);
                    }
                    else
                        return new Error(Error.ERROR_INVALID_PARAMETER, String.Format(Messages.CommandSupportsOnlyOneParameter, command));
                }
            }
            else
                logging = new Logging(LogLevel.NoLogging, "");

            return null;
        }

        public IError CheckCommandAddPrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer,
            PrinterEnvironment printerEnvironment, string portName, string driverName, ILogging logging)
        {
            const string command = "addprinter";
            var printer = new Printer();
            if (cmdlParams != null && cmdlParams.ContainsKey(command))
            {
                if (cmdlParams[command] == null || cmdlParams[command].Count == 0)
                    return new Error(Error.ERROR_INVALID_PARAMETER, string.Format(Messages.CommandWrongNumberOfParameters, command));
                logging.Log("AddPrinter: Started ...");
                var error = printer.AddPrinter(printerEnvironment, cmdlParams[command], win32Printer, portName, driverName, logging);
                var printers = win32Printer.GetInstalledPrintersFromList(cmdlParams[command]);
                if (error != null)
                {
                    if (printers.Count == 0)
                        logging.Log("AddPrinter: Failed [No printer added]");
                    else
                        logging.Log("AddPrinter: Failed [Only " + printers.Count + " printer(s) added]");
                    return error;
                }
                if (printers.Count == cmdlParams[command].Count)
                {
                    logging.Log("AddPrinter: Successful [" + printers.Count + " printer(s) added]");
                    return null; // Command executed successfully
                }
                logging.Log(String.Format(Messages.UnknownCommandExecutionError, command));
                return new Error(Error.ERROR_INVALID_COMMAND, String.Format(Messages.UnknownCommandExecutionError, command));
            }
            return null; // Command not found.
        }

        public IError CheckCommandRenamePrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer, ILogging logging)
        {
            const string command = "renameprinter";
            var printer = new Printer();
            if (cmdlParams != null && cmdlParams.ContainsKey(command))
            {
                if (cmdlParams[command] == null || cmdlParams[command].Count != 2)
                    return new Error(Error.ERROR_INVALID_PARAMETER, string.Format(Messages.CommandWrongNumberOfParameters, command));
                logging.Log("RenamePrinter: Started ...");
                var error = printer.RenamePrinter(cmdlParams[command], win32Printer, logging);
                if (error != null)
                {
                    logging.Log(String.Format("RenamePrinter: Failed renaming printer '{0}'.", cmdlParams[command][0]));
                    return error;
                }
                if (!win32Printer.IsPrinterInstalled(cmdlParams[command][0]) && win32Printer.IsPrinterInstalled(cmdlParams[command][1]))
                {
                    logging.Log(String.Format("RenamePrinter: Successful [Printer '{0}' renamed to '{1}']", cmdlParams[command][0], cmdlParams[command][1]));
                    return null; // Command executed successfully
                }
                logging.Log(String.Format(Messages.UnknownCommandExecutionError, command));
                return new Error(Error.ERROR_INVALID_COMMAND, String.Format(Messages.UnknownCommandExecutionError, command));
            }
            return null; // Command not found.
        }
        
        public IError CheckCommandDeletePrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer, string driverName, ILogging logging)
        {
            const string command = "deleteprinter";
            var printer = new Printer();
            if (cmdlParams != null && cmdlParams.ContainsKey(command))
            {
                if (cmdlParams[command] == null || cmdlParams[command].Count == 0)
                    return new Error(Error.ERROR_INVALID_PARAMETER, string.Format(Messages.CommandWrongNumberOfParameters, command));
                logging.Log("DeletePrinter: Started ...");
                var error = printer.DeletePrinter(cmdlParams[command], win32Printer, driverName, logging);
                if (error != null)
                    return error;

                var printers = win32Printer.GetInstalledPrintersFromList(cmdlParams[command]);
                if (printers.Count == 0)
                {
                    logging.Log("DeletePrinter: Successful [" + cmdlParams[command].Count + " printer(s) deleted]");
                    return null; // Command executed successfully
                }
                logging.Log(String.Format(Messages.UnknownCommandExecutionError, command));
                return new Error(Error.ERROR_INVALID_COMMAND, String.Format(Messages.UnknownCommandExecutionError, command));
            }
            return null; // Command not found.
        }

        public IError CheckCommandInstallPrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer, string monitorName, string portName, string driverName, ILogging logging)
        {
            const string command = "installprinter";
            var printer = new Printer();
            if (cmdlParams != null && cmdlParams.ContainsKey(command))
            {
                if (cmdlParams[command] == null || cmdlParams[command].Count == 0)
                    return new Error(Error.ERROR_INVALID_PARAMETER, string.Format(Messages.CommandWrongNumberOfParameters, command));

                logging.Log("InstallPrinter: Started ...");

                string portApplication;

                const string commandPortApplication = "portapplication";
                if (!cmdlParams.ContainsKey(commandPortApplication))
                    return new Error(Error.ERROR_INVALID_COMMAND, Messages.NoPortApplicationSpecified);

                var error = printer.CheckAndGetPortApplication(cmdlParams[commandPortApplication], out portApplication);
                if (error != null)
                    return error;

                var externalPpdFile = "";
                const string commandPddFile = "ppdfile";
                if (cmdlParams.ContainsKey(commandPddFile))
                {
                    error = printer.CheckAndGetPpdFile(cmdlParams[commandPddFile], out externalPpdFile);
                    if (error != null)
                        return error;
                }

                error = printer.InstallPrinter(cmdlParams[command], win32Printer, monitorName, portName, portApplication, driverName, externalPpdFile, logging);
                if (error != null)
                    return error;

                var installedPrinters = win32Printer.GetInstalledPrintersFromList(cmdlParams["installprinter"]);
                if (installedPrinters.Count == cmdlParams[command].Count)
                {
                    logging.Log("InstallPrinter: Successful [" + installedPrinters.Count + " printer(s) installed]");
                    return null; // Command executed successfully
                }
                logging.Log(String.Format(Messages.UnknownCommandExecutionError, command));
                return new Error(Error.ERROR_INVALID_COMMAND, String.Format(Messages.UnknownCommandExecutionError, command));
            }
            return null; // Command not found.
        }

        public IError CheckCommandUnInstallPrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer, string driverName, string monitorName, ILogging logging)
        {
            const string command = "uninstallprinter";
            var printer = new Printer();
            if (cmdlParams != null && cmdlParams.ContainsKey(command))
            {
                if (cmdlParams[command] != null && cmdlParams[command].Count != 0)
                    return new Error(Error.ERROR_INVALID_PARAMETER, string.Format(Messages.CommandWrongNumberOfParameters, command));

                logging.Log("UnInstallPrinter: Started ...");
                var pdfcreatorPrinters1 = win32Printer.GetLocalPrinters(driverName);

                var error = printer.UnInstallPrinter(win32Printer, driverName, monitorName, logging);
                var pdfcreatorPrinters2 = win32Printer.GetLocalPrinters(driverName);
                if (error != null)
                {
                    if (pdfcreatorPrinters2.Count != 0)
                    {
                        logging.Log("UnInstallPrinter: Failed [" + (pdfcreatorPrinters1.Count - pdfcreatorPrinters2.Count) + " printer(s) uninstalled]");
                    }

                    return error;
                }
                if (pdfcreatorPrinters1.Count > 0 && pdfcreatorPrinters2.Count == 0)
                {
                    logging.Log("UnInstallPrinter: Successful [" + pdfcreatorPrinters1.Count + " printer(s) uninstalled]");
                    return null; // Command executed successfully
                }
                if (pdfcreatorPrinters1.Count > 0)
                    return new Error(Error.ERROR_INVALID_COMMAND, String.Format(Messages.UnknownCommandExecutionError, command));
            }
            return null; // Command not found.
        }

        public static List<string> ValidCommands
        {
            get
            {
                var validCommands = new List<string>
                {
                    "AddPrinter",
                    "RenamePrinter",
                    "DeletePrinter",
                    "InstallPrinter",
                    "UnInstallPrinter",
                    "PortApplication",
                    "PPDFile",
                    "Log"
                };
                return validCommands;
            }
        }
    }
}
