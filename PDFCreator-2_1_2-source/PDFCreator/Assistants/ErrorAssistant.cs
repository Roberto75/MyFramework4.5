using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using pdfforge.PDFCreator.ErrorReport;
using pdfforge.PDFCreator.Helper;

namespace pdfforge.PDFCreator.Assistants
{
    public static class ErrorAssistant
    {
        private static string ComposeErrorText(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Error Report for PDFCreator v" + UpdateAssistant.CurrentVersion);

            sb.AppendLine();
            sb.AppendLine("Exception:");
            sb.AppendLine(ex.ToString());

            sb.AppendLine();
            sb.AppendLine("Environment:");
            sb.AppendLine(Environment.OSVersion.VersionString);

            sb.AppendLine();
            sb.AppendLine("Environment variables:");
            var env = Environment.GetEnvironmentVariables();

            foreach (var item in env.Keys)
            {
                sb.AppendLine(item + "=" + env[item]);
            }

            return sb.ToString();
        }

        public static void ShowErrorWindow(Exception ex, out bool terminateApplication)
        {
            var err = new ErrorReportWindow(ComposeErrorText(ex), true);

            if (err.ShowDialog() == true)
                terminateApplication = true;
            else
                terminateApplication = false;
        }

        public static bool ShowErrorWindowInNewProcess(Exception ex)
        {
            string errorReporterPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) ?? "";
            errorReporterPath = Path.Combine(errorReporterPath, "ErrorReport.exe");

            if (File.Exists(errorReporterPath))
                return false;

            try
            {
                string errorFile = Path.GetTempPath() + Guid.NewGuid() + ".err";
                File.WriteAllText(errorFile, ComposeErrorText(ex));

                Process.Start(errorReporterPath, "\"" + errorFile + "\"");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return true;
        }
    }
}
