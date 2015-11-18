using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Threading;

namespace pdfforge.PrinterHelper
{
    public struct DriverFiles
    {
        public string DriverPath;
        public string ConfigFile;
        public string HelpFile;
        public string DataFile;
        public List<string> DependentFiles;
    }

    public enum PrinterEnvironment
    {
        [Description("Windows x64")]
        WindowsX64,
        [Description("Windows NT x86")]
        WindowsNtX86
    };

    public enum PrinterDescriptionFileLanguage
    {
        CurrentCulture,
        English,
        German
    };

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                        return attr.Description;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Based on:
    /// Managing Printers Programatically using C# and WMI
    /// By Raymund Macaalay, 13 May 2010
    /// 
    /// http://www.codeproject.com/Articles/80680/Managing-Printers-Programatically-using-C-and-WMI
    /// 
    /// Licensed under The Code Project Open License (CPOL)
    /// http://www.codeproject.com/info/cpol10.aspx
    /// </summary>
    public class PrinterUtil
    {

        public static int LastError { get; private set; }
        
        private static void SaveStreamToFile(Stream stream, string fileName)
        {
            using (var reader = new BinaryReader(stream))
            {
                using (var writer = new BinaryWriter(File.OpenWrite(fileName)))
                {
                    var buffer = new byte[2048];
                    int current;
                    do
                    {
                        current = reader.Read(buffer, 0, buffer.Length);
                        writer.Write(buffer, 0, current);
                    } while (current > 0);  
                }
            }
        }

        public static void ExtractPrinterFilesWindows8(PrinterEnvironment printerEnvironment, PrinterDescriptionFileLanguage language, string printerDriverDirectory, bool extractPpdFile, out DriverFiles driverFiles)
        {
            const string resources = "pdfforge.PrinterHelper.Resources.Drivers.";
            const string resourcesWindows8 = resources + "Windows8.";
            string resourcesWindows8Environment;

            switch (printerEnvironment)
            {
                case PrinterEnvironment.WindowsX64:
                    resourcesWindows8Environment = resourcesWindows8 + "x64.";
                    break;
                case PrinterEnvironment.WindowsNtX86:
                    resourcesWindows8Environment = resourcesWindows8 + "x86.";
                    break;
                default:
                    throw new Exception("ExtractPrinterFilesWindows8: Unknown printer environment \"" + printerEnvironment + "\"!");
            }

            #region Extract driver file
            const string driverFileName = "PSCRIPT5.DLL";
            driverFiles.DriverPath = driverFileName;
            SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcesWindows8Environment + driverFileName),
                Path.Combine(printerDriverDirectory, driverFiles.DriverPath));
            #endregion

            #region Extract config file
            const string configFileName = "PS5UI.DLL";
            driverFiles.ConfigFile = configFileName;
            SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcesWindows8Environment + configFileName),
                Path.Combine(printerDriverDirectory, driverFiles.ConfigFile));
            #endregion

            #region Extract help file
            const string helpFileName = "PSCRIPT.HLP";
            driverFiles.HelpFile = helpFileName;
            SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcesWindows8 + helpFileName),
                Path.Combine(printerDriverDirectory, driverFiles.HelpFile));
            #endregion

            #region Extract data file
            driverFiles.DataFile = "";
            if (extractPpdFile)
            {
                string dataFileName;
                var currentCulture = Thread.CurrentThread.CurrentCulture;
                if ((language == PrinterDescriptionFileLanguage.German) || (language == PrinterDescriptionFileLanguage.CurrentCulture && currentCulture.TwoLetterISOLanguageName == "de"))
                    dataFileName = "PDFCREATOR_german.PPD";
                else
                    dataFileName = "PDFCREATOR_english.PPD";

                driverFiles.DataFile = "PDFCREAT.PPD";
                SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(resources + dataFileName),
                    Path.Combine(printerDriverDirectory, driverFiles.DataFile));
            }
            #endregion

            #region Extract dependent files
            driverFiles.DependentFiles = new List<string>();

            const string dependentFileName1 = "PS_SCHM.GDL";
            driverFiles.DependentFiles.Add(dependentFileName1);
            SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcesWindows8 + dependentFileName1),
                Path.Combine(printerDriverDirectory, dependentFileName1));

            const string dependentFileName2 = "PSCRIPT.NTF";
            driverFiles.DependentFiles.Add(dependentFileName2);
            SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcesWindows8Environment + dependentFileName2),
                Path.Combine(printerDriverDirectory, dependentFileName2));
            #endregion
        }

        public static void DeletePrinterDriverFilesFromPrinterDriverDirectory(string printerDriverDirectory, string externalPpdFile)
        {
            var file = Path.Combine(printerDriverDirectory, "PSCRIPT5.DLL");
            if (File.Exists(file)) File.Delete(file);
            file = Path.Combine(printerDriverDirectory, "PS5UI.DLL");
            if (File.Exists(file)) File.Delete(file);
            file = Path.Combine(printerDriverDirectory, "PSCRIPT.HLP");
            if (File.Exists(file)) File.Delete(file);
            file = Path.Combine(printerDriverDirectory, "PDFCREAT.PPD");
            if (File.Exists(file)) File.Delete(file);
            file = Path.Combine(printerDriverDirectory, "PS_SCHM.GDL");
            if (File.Exists(file)) File.Delete(file);
            file = Path.Combine(printerDriverDirectory, "PSCRIPT.NTF");
            if (File.Exists(file)) File.Delete(file);

            if (externalPpdFile != null && externalPpdFile.Trim().Length > 0 && File.Exists(externalPpdFile)) File.Delete(externalPpdFile);
        }

        public static void ExtractMonitorFile(PrinterEnvironment printerEnvironment, string monitorDriverFileName)
        {
            const string resources = "pdfforge.PrinterHelper.Resources.Monitor.";
            string resourcesMonitorEnvironment;
            switch (printerEnvironment)
            {
                case PrinterEnvironment.WindowsX64:
                    resourcesMonitorEnvironment = resources + "x64.";
                    break;
                case PrinterEnvironment.WindowsNtX86:
                    resourcesMonitorEnvironment = resources + "x86.";
                    break;
                default:
                    throw new Exception("ExtractMonitorFile: Unknown printer environment \"" + printerEnvironment + "\"!");
            }

            #region Extract monitor file

            SaveStreamToFile(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcesMonitorEnvironment + "pdfcmon.dll"),
                Path.Combine(Environment.SystemDirectory, monitorDriverFileName));
            #endregion
        }
    }
}
