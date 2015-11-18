using System;
using System.Collections.Generic;

namespace pdfforge.PrinterHelper
{
    public interface IWin32Printer
    {
        System.Runtime.InteropServices.ComTypes.FILETIME DateTimeToFiletime(DateTime time);
        bool AddPrinter(string printerName, string portName, string driverName);
        bool DeletePrinterDriver(PrinterEnvironment printerEnvironment, string printerDriverName);
        void AddPdfcreatorPrinterMonitor(PrinterEnvironment printerEnvironment, string monitorName, string portName, string monitorDriverFileName, string programFullFileName);
        void AddPdfcreatorPrinterDriverGdi(PrinterEnvironment printerEnvironment, string driverName, PrinterDescriptionFileLanguage language, string externalPpdFile);
        bool DeleteMonitor(string monitorName, bool deleteDriverFile);
        bool DeletePrinter(string printerName);
        IList<PORT_INFO_2> GetPorts();
        bool GetMonitor(string portName, out string monitorName);
        IList<MONITOR_INFO_2> GetMonitors();
        IList<DRIVER_INFO_3> GetPrinterDriver(PrinterEnvironment printerEnvironment);

        /// <summary>
        /// This method returns a list of installed printer.
        /// </summary>
        /// <param name="printerEnumFlags">A flag that describes the kind of printer (e.g. local, network or both).</param>
        /// <returns>
        /// A list of all installed printer. 
        /// </returns>
        IList<PRINTER_INFO_2> GetPrinters(PrinterEnumFlags printerEnumFlags);

        IList<PRINTER_INFO_2> GetPrinters(PrinterEnumFlags printerEnumFlags, string monitorName);

        /// <summary>
        /// This method returns a list of locally installed printer.
        /// </summary>
        /// <returns>
        /// A list of locally installed printer. 
        /// </returns>
        IList<PRINTER_INFO_2> GetLocalPrinters();

        /// <summary>
        /// Returns a list of local printers that use a specific printer driver.
        /// </summary>
        /// <param name="driverName">The name of the printer driver.</param>
        /// <returns>
        /// A list of local printers that use a specific printer driver. 
        /// </returns>
        IList<PRINTER_INFO_2> GetLocalPrinters(string driverName);

        bool IsPortInstalled(string portName);
        bool IsMonitorInstalled(string monitorName);
        bool IsDriverInstalled(PrinterEnvironment printerEnvironment, string driverName);

        /// <summary>
        /// This method checks whether an installed printer (locally or network) exists.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <param name="printerEnumFlags">A flag that describes the kind of printer (e.g. local or network).</param>
        /// <returns>
        /// <c>true</c>: If the printer exists. Otherwise <c>false</c>.
        /// </returns>
        bool IsPrinterInstalled(string printerName, PrinterEnumFlags printerEnumFlags);

        /// <summary>
        /// This method checks whether a local installed printer exists.
        /// </summary>
        /// <returns>
        /// <c>true</c>: If the local printer exists. Otherwise <c>false</c>.
        /// </returns>
        bool IsPrinterInstalled(string printerName);

        bool RenamePrinter(string oldPrinterName, string newPrintername);

        /// <summary>
        /// This method checks if the printers of a given list are installed and returns all installed printers.
        /// </summary>
        /// <returns>
        /// Return a list of installed printers.
        /// </returns>
        IList<string> GetInstalledPrintersFromList(IEnumerable<string> printers);

        bool ArePrintersDeletable(IEnumerable<string> printers, out List<string> notInstalledPrinters);
        bool GetPrinter(string printerName, PrinterEnumFlags printerEnumFlags, out PRINTER_INFO_2 printer);
        bool ArePrintersWithSpecificDriverDeletable(IEnumerable<string> printers, string driverName, PrinterEnumFlags printerEnumFlags, out List<string> notInstalledPrinters, out List<string> notPrintersWithSpecificDriver);
        IError AdaptFreememPrinterSetting(string printerName, int memorySize);
    }
}