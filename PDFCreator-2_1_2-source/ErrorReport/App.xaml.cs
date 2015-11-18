using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace pdfforge.PDFCreator.ErrorReport
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 1)
                return;

            string errorFile = e.Args[0];

            try
            {

                if (!File.Exists(errorFile))
                    return;

                string errorText = File.ReadAllText(errorFile);

                var err = new ErrorReportWindow(errorText, false);

                err.ShowDialog();

                if (!Debugger.IsAttached)
                    File.Delete(errorFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
