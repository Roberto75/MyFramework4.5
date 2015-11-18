using System.Collections.Generic;

namespace pdfforge.PrinterHelper
{
    public interface IPrinter
    {
        IError UnInstallPrinter(IWin32Printer win32Printer, string driverName, string monitorName, ILogging logging);
        IError RenamePrinter(IList<string> printerNames, IWin32Printer win32Printer, ILogging logging);
        IError CheckAndGetPortApplication(IList<string> portApplications, out string portAppliation);
        IError InstallPrinter(IList<string> printerNames, IWin32Printer win32Printer, string monitorName, string portName, string portApplication, string driverName, string externalPpdFile, ILogging logging);

        /// <summary>
        /// This method deletes printers with a specific driver. (Hint: Delete only PDFCreator printers)
        /// </summary>
        /// <returns>
        /// <c>Error</c>: Returns an error object if there was an error. Otherwise <c>null</c>.
        /// </returns>
        IError DeletePrinter(IList<string> printerNames, IWin32Printer win32Printer, string driverName, ILogging logging);

        IError AddPrinter(PrinterEnvironment printerEnvironment, IList<string> printerNames, IWin32Printer win32Printer, string portName, string driverName, ILogging logging);
    }
}