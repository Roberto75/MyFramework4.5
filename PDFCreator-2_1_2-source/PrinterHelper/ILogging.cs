using System.Collections.Generic;

namespace pdfforge.PrinterHelper
{
    public interface ILogging
    {
        LogLevel LogLevel { get; }
        string LogFile { get; }
        void Log(string message);
        void Log(string message, List<string> details);
        void LogAllPrinterInfos();
    }
}