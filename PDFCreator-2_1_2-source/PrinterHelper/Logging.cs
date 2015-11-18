using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace pdfforge.PrinterHelper
{
    public enum LogLevel
    {
        NoLogging,
        LogToConsole,
        LogToFile
    }

    public class Logging : ILogging
    {
        private const int CountSepChar = 30;
        public Logging(LogLevel logLevel, string logFile)
        {
            LogLevel = logLevel;
            LogFile = logFile;
            if (LogLevel == LogLevel.LogToFile)
                Log(new string('-', CountSepChar) + " PrinterHelper - Start " + new string('-', CountSepChar));
        }
        ~Logging()
        {
            if (LogLevel == LogLevel.LogToFile)
                Log(new string('-', CountSepChar) + " PrinterHelper - End " + new string('-', CountSepChar));
        }

        public LogLevel LogLevel { get; private set; }

        public string LogFile { get; private set; }

        public void Log(string message)
        {
            Log(message, null);
        }

        public void Log(string message, List<string> details)
        {
            if (LogLevel == LogLevel.LogToConsole)
            {
                if (message != null)
                    Console.WriteLine(message);
                if (details != null && details.Count > 0)
                {
                    foreach (var detail in details)
                        // ReSharper disable once LocalizableElement
                        Console.WriteLine(" " + detail);
                }
            }
            if (LogLevel == LogLevel.LogToFile)
            {
                using (var streamWriter = File.AppendText(LogFile))
                {
                    if (message != null)
                        streamWriter.WriteLine(message);
                    if (details != null && details.Count > 0)
                    {
                        foreach (var detail in details)
                            streamWriter.WriteLine(" " + detail);
                    }
                }
            }
        }

        public void LogAllPrinterInfos()
        {
            var win32Printer = new Win32Printer();
            var ports = win32Printer.GetPorts();
            Log("Installed ports [" + ports.Count + "]:");
            foreach (var port in ports)
                Log(" " + port.pPortName + " [Monitor: " + port.pMonitorName + ", Description: " + port.pDescription + "]");

            var monitors = win32Printer.GetMonitors();
            Log("Installed monitors [" + monitors.Count + "]:");
            foreach (var monitor in monitors)
                Log(" " + monitor.pName + " [Environment: " + monitor.pEnvironment + ", DLLName: " + monitor.pDLLName + " (" + GetFileVersion(Path.Combine(Environment.SystemDirectory, monitor.pDLLName)) + ")]");

            var drivers = win32Printer.GetPrinterDriver(PrinterEnvironment.WindowsNtX86);
            Log("Installed drivers (x86) [" + drivers.Count + "]:");
            foreach (var driver in drivers)
                Log(" " + driver.pName + " [DriverPath: " + driver.pDriverPath + " (" + GetFileVersion(Path.Combine(Environment.SystemDirectory, driver.pDriverPath)) + ")]");

            drivers = win32Printer.GetPrinterDriver(PrinterEnvironment.WindowsX64);
            Log("Installed drivers (x64) [" + drivers.Count + "]:");
            foreach (var driver in drivers)
                Log(" " + driver.pName + " [DriverPath: " + driver.pDriverPath + " (" + GetFileVersion(Path.Combine(Environment.SystemDirectory, driver.pDriverPath)) + ")]");

            var printers = win32Printer.GetPrinters(PrinterEnumFlags.PRINTER_ENUM_ALL);
            Log("Installed printers [" + printers.Count + "]:");
            foreach (var printer in printers)
                Log(" " + printer.pPrinterName + " [DriverName:" + printer.pDriverName + ", PortName:" + printer.pPortName + "]");
        }

        private static string GetFileVersion(string fileName)
        {
            try
            {
                return FileVersionInfo.GetVersionInfo(fileName).FileVersion;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
