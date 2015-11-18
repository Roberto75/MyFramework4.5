using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace pdfforge.PrinterHelper
{
    public class Printer : IPrinter
    {
        static readonly PrinterEnvironment PrinterEnvironment = Environment.Is64BitOperatingSystem ? PrinterEnvironment.WindowsX64 : PrinterEnvironment.WindowsNtX86;
        public static PrinterEnvironment CurrentPrinterEnvironment
        {
            get
            {
                return PrinterEnvironment;
            }
        }
        private IError FindInvalidPrinternames(IEnumerable<string> printerNames)
        {
            var invalidPrinterNames = new List<string>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var printerName in printerNames)
            {
                if (printerName.IndexOfAny(new[] { '!', '\\', ',' }) >= 0)
                    invalidPrinterNames.Add(printerName);
            }
            if (invalidPrinterNames.Count > 0)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.InValidCharInPrinterName);
            return null;
        }
        
        public IError UnInstallPrinter(IWin32Printer win32Printer, string driverName, string monitorName, ILogging logging)
        {
            var pdfcreatorPrinters = win32Printer.GetLocalPrinters(driverName);

            if (pdfcreatorPrinters.Count == 0)
                logging.Log(string.Format("UnInstallPrinter: " + Messages.NoPdfcreatorPrinterInstalled));
            else
            {
                var error = DeletePrinter(pdfcreatorPrinters.Select(x => x.pPrinterName).ToList(), win32Printer, driverName, logging);
                if (error != null)
                    return error;
            }

            bool result;
            // Enumerate PrinterEnvironment enum
            foreach (var printerEnvironment in (PrinterEnvironment[])Enum.GetValues(typeof(PrinterEnvironment)))
            {
                if (win32Printer.IsDriverInstalled(printerEnvironment, driverName))
                {
                    try
                    {
                        logging.Log(string.Format("UnInstallPrinter: DeletePrinterDriver: {0} [Environment: {1}]", driverName, printerEnvironment.GetDescription()));
                        win32Printer.DeletePrinterDriver(printerEnvironment, driverName);
                    }
                    catch (Win32Exception ex)
                    {
                        const string serviceName = "spooler";
                        logging.Log(string.Format("UnInstallPrinter: DeletePrinterDriver failed! Try restarting service : {0}.", serviceName));
                        Service.RestartService(serviceName, 4000);
                        logging.Log("UnInstallPrinter: Finished restarting service.");
                        logging.Log(string.Format("UnInstallPrinter: DeletePrinterDriver: {0} [Environment: {1}]", driverName, printerEnvironment.GetDescription()));
                        result = win32Printer.DeletePrinterDriver(printerEnvironment, driverName);
                        if (result == false)
                            logging.Log("UnInstallPrinter: " + Messages.PrinterDriverUnDeletable + " " + ex.Message);
                    }
                }
                else
                    logging.Log(string.Format("UnInstallPrinter: DeletePrinterDriver: {0} [Environment: {1}] not installed.", driverName, printerEnvironment.GetDescription()));
            }

            if (win32Printer.IsMonitorInstalled(monitorName))
            {
                var printersWithPortsFromSpecificMonitor = win32Printer.GetPrinters(PrinterEnumFlags.PRINTER_ENUM_ALL, monitorName);
                if (printersWithPortsFromSpecificMonitor.Count > 0)
                    return new Error(Error.ERROR_INTERNAL, Messages.PrintersUsingPdfcreatorMonitor, printersWithPortsFromSpecificMonitor.Select(x => x.pPrinterName).ToList());

                logging.Log(string.Format("UnInstallPrinter: DeleteMonitor: {0}", monitorName));
                result = win32Printer.DeleteMonitor(monitorName, true);
                if (result == false)
                    return new Error(Error.ERROR_INTERNAL, Messages.PrinterMonitorUnDeletable);
            }
            else
                logging.Log(string.Format("UnInstallPrinter: DeleteMonitor: {0} not installed.", monitorName));

            return null;
        }
        
        public IError RenamePrinter(IList<string> printerNames, IWin32Printer win32Printer, ILogging logging)
        {
            if (printerNames == null)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoPrinterNameSpecified);

            printerNames = printerNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!printerNames.Any())
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoValidPrinterNameSpecified);

            var error = FindInvalidPrinternames(printerNames);
            if (error != null)
                return error;

            if (printerNames.Count == 1)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoNewPrinterName);

            if (printerNames.Count != 2)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.OnlyTwoPrinterNamesAllowedForRenaming);

            if (!win32Printer.IsPrinterInstalled(printerNames[0]))
                return new Error(Error.ERROR_INVALID_PARAMETER, string.Format(Messages.PrinterNotInstalled, printerNames[0]));

            if (win32Printer.IsPrinterInstalled(printerNames[1]))
                return new Error(Error.ERROR_INVALID_PARAMETER, string.Format(Messages.NewPrinterNameForRenamingAlreadyExists, printerNames[1]));

            logging.Log(string.Format("Rename printer '{0}' to '{1}'.", printerNames[0], printerNames[1]));
            win32Printer.RenamePrinter(printerNames[0], printerNames[1]);

            return null;
        }
        
        public IError CheckAndGetPortApplication(IList<string> portApplications, out string portApplication)
        {
            portApplication = "";

            if (portApplications == null)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoPortApplicationSpecified);

            portApplications = portApplications.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            if (!portApplications.Any())
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoValidPortApplicationSpecified);

            if (portApplications.Count != 1)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.OnlyOnePortApplicationAllowed);
            try
            {
                // Check for valid path
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Path.GetFullPath(portApplications[0]);
            }
            catch (Exception)
            {
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PathPortApplicationNotValid);
            }

            if (!Path.IsPathRooted(portApplications[0]))
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PathPortApplicationNotRooted);

            portApplication = portApplications[0];
            return null;
        }

        public IError CheckAndGetPpdFile(IList<string> ppdFiles, out string ppdFile)
        {
            ppdFile = "";

            if (ppdFiles == null)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoPpdFileSpecified);

            ppdFiles = ppdFiles.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            if (!ppdFiles.Any())
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoValidPortApplicationSpecified);

            if (ppdFiles.Count != 1)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.OnlyOnePortApplicationAllowed);
            try
            {
                // Check for valid path
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Path.GetFullPath(ppdFiles[0]);
            }
            catch (Exception)
            {
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PathPortApplicationNotValid);
            }

            if (!File.Exists(ppdFiles[0]))
                return new Error(Error.ERROR_INVALID_PARAMETER, string.Format(Messages.PpdFileNotFound, ppdFiles[0]));

            ppdFile = ppdFiles[0];
            return null;
        }

        public IError InstallPrinter(IList<string> printerNames, IWin32Printer win32Printer, string monitorName, string portName, string portApplication, string driverName, string externalPpdFile, ILogging logging)
        {
            const string printerMonitorFileName = "pdfcmon.dll";

            var printerEnvironment = CurrentPrinterEnvironment;
            logging.Log(string.Format("InstallPrinter: AddMonitor [Monitor: {0}, Port: {1}, Environment: {2}]", monitorName, portName, printerEnvironment.GetDescription()));

            if (printerNames == null)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoPrinterNameSpecified);

            printerNames = printerNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!printerNames.Any())
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoValidPrinterNameSpecified);

            var error = FindInvalidPrinternames(printerNames);
            if (error != null)
                return error;

            var alreadyInstalledPrinters = win32Printer.GetInstalledPrintersFromList(printerNames);
            if (alreadyInstalledPrinters.Count > 0)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PrintersAlreadyInstalled, alreadyInstalledPrinters);

            if (win32Printer.IsMonitorInstalled(monitorName))
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PrinterPortAlreadyInstalled);

            win32Printer.AddPdfcreatorPrinterMonitor(printerEnvironment, monitorName, portName, printerMonitorFileName, portApplication);

            if (win32Printer.IsDriverInstalled(printerEnvironment, driverName))
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PrinterDriverAlreadyInstalled);

            logging.Log(string.Format("InstallPrinter: AddDriver [Driver: {0}, Environment: {1}]", driverName, printerEnvironment.GetDescription()));
            win32Printer.AddPdfcreatorPrinterDriverGdi(printerEnvironment, driverName, PrinterDescriptionFileLanguage.CurrentCulture, externalPpdFile);

            return AddPrinter(printerEnvironment, printerNames, win32Printer, portName, driverName, logging);
        }

        /// <summary>
        /// This method deletes printers with a specific driver. (Hint: Delete only PDFCreator printers)
        /// </summary>
        /// <returns>
        /// <c>Error</c>: Returns an error object if there was an error. Otherwise <c>null</c>.
        /// </returns>
        public IError DeletePrinter(IList<string> printerNames, IWin32Printer win32Printer, string driverName, ILogging logging)
        {

            if (printerNames == null)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoPrinterNameSpecified);

            printerNames = printerNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!printerNames.Any())
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoValidPrinterNameSpecified);

            var error = FindInvalidPrinternames(printerNames);
            if (error != null)
                return error;

            List<string> notInstalledPrinters;
            List<string> notPrintersWithSpecificDriver;
            if (win32Printer.ArePrintersWithSpecificDriverDeletable(printerNames, driverName, PrinterEnumFlags.PRINTER_ENUM_LOCAL, out notInstalledPrinters, out notPrintersWithSpecificDriver) == false)
            {
                if (notInstalledPrinters.Count > 0)
                    return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PrintersAreNotInstalled, notInstalledPrinters);

                if (notPrintersWithSpecificDriver.Count > 0)
                    return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PrintersAreNotPdfCreatorPrinters, notPrintersWithSpecificDriver);
            }
            else
            {
                foreach (var printerName in printerNames)
                {
                    logging.Log(string.Format("Delete printer: {0}", printerName));
                    win32Printer.DeletePrinter(printerName);
                }
            }

            return null;
        }

        public IError AddPrinter(PrinterEnvironment printerEnvironment, IList<string> printerNames, IWin32Printer win32Printer, string portName, string driverName, ILogging logging)
        {
            if (printerNames == null)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoPrinterNameSpecified);

            printerNames = printerNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!printerNames.Any())
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.NoValidPrinterNameSpecified);

            var error = FindInvalidPrinternames(printerNames);
            if (error != null)
                return error;

            var alreadyInstalledPrinters = win32Printer.GetInstalledPrintersFromList(printerNames);
            if (alreadyInstalledPrinters.Count > 0)
                return new Error(Error.ERROR_INVALID_PARAMETER, Messages.PrintersAlreadyInstalled, alreadyInstalledPrinters);

            if (string.IsNullOrWhiteSpace(portName))
                return new Error(Error.ERROR_INVALID_COMMAND, Messages.NoPortSpecified + Messages.InstallPdfCreatorComment);

            if (!win32Printer.IsPortInstalled(portName))
                return new Error(Error.ERROR_INVALID_COMMAND, Messages.PdfCreatorPortNotInstalled + Messages.InstallPdfCreatorComment);

            if (string.IsNullOrWhiteSpace(driverName))
                return new Error(Error.ERROR_INVALID_COMMAND, Messages.NoPrinterDriverNameSpecified + Messages.InstallPdfCreatorComment);

            if (!win32Printer.IsDriverInstalled(printerEnvironment, driverName))
                return new Error(Error.ERROR_INVALID_COMMAND, Messages.NoPdfCreatorPrinterDriverInstalled + Messages.InstallPdfCreatorComment);

            foreach (var printerName in printerNames)
            {
                logging.Log(string.Format("Add printer: {0}", printerName));
                win32Printer.AddPrinter(printerName, portName, driverName);
                error = win32Printer.AdaptFreememPrinterSetting(printerName, 32767);
                if (error != null)
                    return error;
            }

            return null;
        }
    }
}
