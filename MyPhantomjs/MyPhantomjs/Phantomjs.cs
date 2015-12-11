using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security;

namespace My.Phantomjs
{
    public class Phantomjs
    {
        public enum EnumTaskType
        {
            Rasterize,
            RGraphToImage
        }


        public bool cacheEnabled { get; set; }
        public List<My.Phantomjs.Models.Task> Tasks { get; set; }

        private System.IO.FileInfo _phantomjs_exe;
        private System.IO.DirectoryInfo _outputFolder;

        public Phantomjs(string phantomjs_exe, string outputFolder)
        {
            _phantomjs_exe = new System.IO.FileInfo(phantomjs_exe);
            _outputFolder = new System.IO.DirectoryInfo(outputFolder);

            if (!_outputFolder.Exists)
            {
                _outputFolder.Create();
            }

            Tasks = new List<Models.Task>();
        }

        public bool process()
        {

            bool esito = true;

            foreach (My.Phantomjs.Models.Task t in Tasks)
            {
                Console.Write("Elaborazione del task: " + t.fileName + " ...");

                process(t);

                if (t.Esito == My.Phantomjs.Models.Task.EnumEsito.Failed)
                {
                    Console.WriteLine(" fallita!");
                    esito = false;
                }

                Console.WriteLine(" conclusa con successo");
            }

            return esito;
        }

        private bool isInCache(System.IO.FileInfo fi)
        {

            //double minuti;
            //minuti = ( DateTime.Now - fi.CreationTime).TotalMinutes;

            TimeSpan span = DateTime.Now.Subtract(fi.CreationTime);
            //Debug.WriteLine("Time Difference (seconds): " + span.Seconds);
            //Debug.WriteLine("Time Difference (minutes): " + span.Minutes);
            //Debug.WriteLine("Time Difference (hours): " + span.Hours);
            //Debug.WriteLine("Time Difference (days): " + span.Days);

            //Debug.WriteLine("Time Difference (seconds): " + span.TotalSeconds);
            //Debug.WriteLine("Time Difference (minutes): " + span.TotalMinutes);
            //Debug.WriteLine("Time Difference (hours): " + span.TotalHours);
            //Debug.WriteLine("Time Difference (days): " + span.TotalDays);

            //applico una cache di 10 minuti
            //if (span.TotalMinutes <= 10)
            if (span.TotalHours <= 5)
            {
                Debug.WriteLine(fi.Name + " < di 5 Hours ... get report from cache ... ");
                return true;
            }

            return false;
        }

        public void process(Models.Task task)
        {
            if (!_phantomjs_exe.Exists)
            {
                throw new ApplicationException("phantomjs.exe non trovato");
            }
            cacheEnabled = true;

            Debug.WriteLine("cacheEnabled: " + cacheEnabled.ToString());
            Debug.WriteLine(task.Url.ToString());

            //System.IO.File.Delete(String.Format("{0}\\{1}", _outputFolder.FullName, task.fileName));

            System.IO.FileInfo outputFile = new System.IO.FileInfo(String.Format("{0}\\{1}", _outputFolder.FullName, task.fileName));

            if (outputFile.Exists)
            {

                if (cacheEnabled && task.cacheEnabled && isInCache(outputFile))
                {
                    task.Esito = Models.Task.EnumEsito.Success;
                    task.OutputFile = outputFile;
                    return;
                }

                try
                {
                    Debug.WriteLine("Delete file from cache: " + outputFile.Name);

                    outputFile.Delete();
                    //System.IO.File.Delete(outputFile.FullName);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                }

            }


            string args = "";
            // args = String.Format("{0}\\rasterize.js {1} {2} ", _phantomjs_exe.Directory.FullName, task.Url, outputFile.FullName  );
            //i file .js devono stare nella stessa cartella in cui è presente il file phantom.exe

            switch (task.TaskType)
            {
                case EnumTaskType.Rasterize:
                    args = String.Format("rasterize.js {0} {1}", Uri.EscapeUriString(task.Url.ToString()), outputFile.FullName);
                    // args = String.Format("rasterize.js {0} {1}", Uri.EscapeUriString(task.Url.ToString()), String.Format("{0}\\{1}", _outputFolder.FullName, task.fileName));

                    break;
                case EnumTaskType.RGraphToImage:
                    args = String.Format("RGraphToImage.js \"{0}\" \"{1}\"", Uri.EscapeUriString(task.Url.ToString()), outputFile.FullName);
                    //args = String.Format("RGraphToImage.js \"{0}\" \"{1}\"", Uri.EscapeUriString(task.Url.ToString()), String.Format("{0}\\{1}", _outputFolder.FullName, task.fileName));
                    break;
            }


            //System.Threading.Thread.Sleep(5000);

            Debug.WriteLine("ARGS: " + args);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            //startInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            process.StartInfo.FileName = _phantomjs_exe.FullName;
            process.StartInfo.Arguments = args;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = _phantomjs_exe.Directory.FullName;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            StringBuilder outputData = new StringBuilder();
            process.OutputDataReceived += (sender, e) => outputData.Append(e.Data);
            //process.OutputDataReceived += (sender, e) => Debug.WriteLine(String.Format("received output: {0}", e.Data));



            ////Autenticazione
            //process.StartInfo.UserName = "roberto.rutigliano";
            //process.StartInfo.Domain = "TECHUB";
            //process.StartInfo.Password = getSecureString("1cambiami!");

            //process.StartInfo.Verb=



            //  runas.exe /netonly /user:TECHUB\roberto.rutigliano /password:1cambiami! "C:\Program Files (x86)\Microsoft SQL Server\110\Tools\Binn\ManagementStudio\Ssms.exe"

            string stdError = null;

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();

                process.WaitForExit();

                Debug.WriteLine("Output: " + outputData.ToString());

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex);
                task.Esito = Models.Task.EnumEsito.Failed;
                task.Messaggio = ex.Message;
                return;
            }



            if (process.ExitCode == 0)
            {
                Debug.WriteLine(outputData.ToString());
                task.Esito = Models.Task.EnumEsito.Success;

                //aggiorno la data del file!
                outputFile.CreationTime = DateTime.Now;

                task.OutputFile = outputFile;
                // task.OutputFile = new System.IO.FileInfo (String.Format("{0}\\{1}", _outputFolder.FullName, task.fileName));
            }
            else
            {
                var message = new StringBuilder();

                if (!string.IsNullOrEmpty(stdError))
                {
                    message.AppendLine(stdError);
                }

                if (outputData.Length != 0)
                {
                    //message.AppendLine("Std output:");
                    message.AppendLine(outputData.ToString());
                }

                task.Esito = Models.Task.EnumEsito.Failed;
                task.Messaggio = message.ToString();

            }

        }

        public SecureString getSecureString(string source)
        {
            if (String.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            SecureString result = new SecureString();

            //foreach (char c in source.ToCharArray())
            //{
            //    result.AppendChar(c);
            //}



            for (int x = 0; x < source.Length; x++)
            {
                result.AppendChar(source[x]);
            }

            return result;
        }

    }
}
