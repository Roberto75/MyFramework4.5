using System;
using System.ComponentModel;
using System.Diagnostics;
using pdfforge.PDFCreator.Shared.Views;

namespace pdfforge.PDFCreator.Shared.Helper
{
    public enum ShellExecuteResult
    {
        Success,
        Failed,
        RunAsWasDenied
    }

    public class ShellExecuteHelper
    {
        public ShellExecuteResult RunAsAdmin(string path, string arguments)
        {
            var psi = new ProcessStartInfo();

            psi.FileName = path;
            psi.Arguments = arguments;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;

            // Use ShellExecute and RunAs starting with Windows Vista
            if (Environment.OSVersion.Version.Major >= 6)
            {
                psi.UseShellExecute = true;
                psi.Verb = "runas";
            }

            try
            {
                Process p = Process.Start(psi);
                p.WaitForExit(10000);

                if (p.ExitCode == 0)
                {
                    return ShellExecuteResult.Success;
                }

                return ShellExecuteResult.Failed;
            }
            catch (Win32Exception)
            {
                return ShellExecuteResult.RunAsWasDenied;
            }
            catch (SystemException)
            {
                return ShellExecuteResult.Failed;
            }

        }
    }
}
