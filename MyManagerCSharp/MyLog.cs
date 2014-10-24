using System;
using System.Text;
using System.Diagnostics;

namespace MyManagerCSharp {

    public static class MyLog
    {
        private static System.Diagnostics.TextWriterTraceListener _fileLog;

        public static bool _init(string fullPath)
        {
            _fileLog = new System.Diagnostics.TextWriterTraceListener(fullPath);
            _fileLog.TraceOutputOptions = TraceOptions.None;

            return true;
        }


        static public void Flush()
        {
            _fileLog.Flush();
        }

        static public void Close()
        {
            _fileLog.Close();
        }


        public static void Error(string message)
        {
            WriteEntry(message, "Error");
        }

        public static void Exception(string message, Exception ex)
        {
            if (ex == null)
            {
                WriteEntry(message + " Attenzione l'eccezione ricevuta è NULL.", "Exception");
            }
            else
            {
                WriteEntry(message + " " + ex.Message, "Exception");
            }

        }

        public static void Warning(string message)
        {
            WriteEntry(message, "Warning");
        }

        public static void Info(string message)
        {
            WriteEntry(message, "Info");
        }

        private static void WriteEntry(string message, string type)
        {
            _fileLog.WriteLine(
                    string.Format("{0};{1};{2};",
                                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                                  type,
                                  message));
            Flush();
        }

    }

}