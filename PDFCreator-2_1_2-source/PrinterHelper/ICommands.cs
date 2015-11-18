using System.Collections.Generic;

namespace pdfforge.PrinterHelper
{
    public interface ICommands
    {
        IError CheckCommands(IDictionary<string, IList<string>> commandDictionary, IList<string> validCommands);
        IError CheckCommandUnInstallPrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer, string driverName, string monitorName, ILogging logging);
        IError CheckCommandInstallPrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer, string monitorName, string portName, string driverName, ILogging logging);

        IError CheckCommandAddPrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer,
            PrinterEnvironment printerEnvironment, string portName, string driverName, ILogging logging);

        IError CheckCommandRenamePrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer, ILogging logging);
        IError CheckCommandDeletePrinterAndExecute(IDictionary<string, IList<string>> cmdlParams, IWin32Printer win32Printer, string driverName, ILogging logging);
        IError CheckLogging(IDictionary<string, IList<string>> cmdlParams, out ILogging logging);
    }
}