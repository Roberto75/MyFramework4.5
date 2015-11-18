using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using NLog;
using pdfforge.PDFCreator.Core.Ghostscript.OutputDevices;

namespace pdfforge.PDFCreator.Core.Ghostscript
{
    /// <summary>
    ///     Provides access to Ghostscript, either through DLL access or by calling the Ghostscript exe
    /// </summary>
    public class GhostScript
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public GhostScript(GhostscriptVersion ghostscriptVersion)
        {
            GhostscriptVersion = ghostscriptVersion;
        }

        public GhostscriptVersion GhostscriptVersion { get; private set; }
        public event EventHandler<OutputEventArgs> Output;

        private bool RunGsCommandLine(IEnumerable<string> parameters)
        {
            // Start the child process.
            var p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = GhostscriptVersion.ExePath;
            p.StartInfo.CreateNoWindow = true;

            bool isFirst = true;
            var sb = new StringBuilder();

            foreach (string s in parameters)
            {
                string tmp = s;
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }

                if (tmp.Contains(" "))
                {
                    tmp = tmp.Replace("\"", "\\\"");
                    tmp = "\"" + tmp + "\"";
                }

                sb.Append(tmp);
                sb.Append(" ");
            }

            p.StartInfo.Arguments = sb.ToString();

            p.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.

            var sbout = new StringBuilder();

            while (!p.StandardOutput.EndOfStream)
            {
                var c = (char) p.StandardOutput.Read();
                sbout.Append(c);

                if ((c == ']') || (c == '\n'))
                {
                    RaiseOutputEvent(sbout.ToString());

                    sbout.Length = 0;
                }
            }

            if (sbout.Length > 0)
                RaiseOutputEvent(sbout.ToString());

            p.WaitForExit();

            return p.ExitCode == 0;
        }

        private void RaiseOutputEvent(string message)
        {
            if (Output != null)
            {
                Output(this, new OutputEventArgs(message));
            }
        }

        private bool Run(string[] parameters)
        {
            _logger.Debug("Ghostscript Parameters:\r\n" + String.Join("\r\n", parameters));

            return RunGsCommandLine(parameters);
        }

        /// <summary>
        ///     Runs Ghostscript with the parameters specified by the OutputDevice
        /// </summary>
        /// <param name="output"></param>
        public bool Run(OutputDevice output)
        {
            var parameters = (List<string>) output.GetGhostScriptParameters(GhostscriptVersion);
            var success = Run(parameters.ToArray());

            string outputFolder = Path.GetDirectoryName(output.Job.OutputFilenameTemplate);

            if (outputFolder != null && !Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
            
            output.CollectTemporaryOutputFiles();

            return success;
        }
    }

    public class OutputEventArgs : EventArgs
    {
        public OutputEventArgs(string output)
        {
            Output = output;
        }

        public string Output { get; private set; }
    }
}