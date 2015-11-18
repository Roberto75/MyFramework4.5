using System;
using Microsoft.Win32;
using System.IO;
using pdfforge.PDFCreator.Utilities;
using System.Diagnostics;

namespace SetupHelper
{
    class Program
    {
        // ReSharper disable once InconsistentNaming
        private const string SHELL_KEY = "{0001B4FD-9EA3-4D90-A79E-FD14BA3AB01D}";

        public static bool Verbose = true;

        private static string _text = "Create PDF and image files with PDFCreator";
        private static string _appPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "PDFCreator.exe");

        static void Main(string[] args)
        {
            bool showUsage = true;

            var clp = new CommandLineParser(args);

            if (clp.HasArgument("Description"))
            {
                _text = clp.GetArgument("Description");
            }

            if (clp.HasArgument("Path"))
            {
                _appPath = clp.GetArgument("Path");
            }

            if (clp.HasArgument("FileExtensions"))
            {
                showUsage = false;
                try
                {
                    switch (clp.GetArgument("FileExtensions"))
                    {
                        case "Add": AddExplorerIntegration(_appPath, _text); break;
                        case "Remove": RemoveExplorerIntegration(); break;
                        default: showUsage = true; break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Environment.ExitCode = 1;
                }
            }

            if (showUsage)
                Usage();
            
            if (Debugger.IsAttached)
                Console.Read();
        }

        private static void Usage()
        {
            Console.WriteLine("SetupHelper " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + "             © pdfforge");
            Console.WriteLine();
            Console.WriteLine("usage:");
            Console.WriteLine("SetupHelper.exe /FileExtensions=Add|Remove [/Description=<Context menu text>] [/Path=<Path to PDFCreator.exe>]");
        }

        /// <summary>
        /// Remove all context menu items for PDFCreator
        /// </summary>
        private static void RemoveExplorerIntegration()
        {
            RegistryKey parent = Registry.ClassesRoot;

            string[] subkeys = parent.GetSubKeyNames();

            foreach (string k in subkeys)
            {
                if (!k.StartsWith("."))
                    continue;

                if (!FileUtil.FileAssocHasPrint(k) && (!FileUtil.FileAssocHasPrintTo(k)))
                    continue;

                using (RegistryKey key = parent.OpenSubKey(k))
                {
                    if (key == null)
                        continue;

                    object o = key.GetValue("");

                    if ((o == null) || !(o is string))
                        continue;

                    var typeName = o as string;

                    using (var typekey = parent.OpenSubKey(typeName + @"\shell\" + SHELL_KEY, true))
                    {
                        try
                        {
                            Write("Removing " + k + ": ");
                            if (typekey != null)
                                parent.DeleteSubKeyTree(typeName + @"\shell\" + SHELL_KEY);
                            
                            WriteLine("success.");
                        }
                        catch (Exception ex) { WriteLine("failed: " + ex.Message); }
                    }
                }
            }
        }

        /// <summary>
        /// Add context menu items for all printable file types. It will print the files with PDFCreator
        /// </summary>
        /// <param name="applicationPath">Full path to PDFCreator.exe</param>
        /// <param name="text">The description text that will be shown in the context menu</param>
        private static void AddExplorerIntegration(string applicationPath, string text)
        {
            RegistryKey parent = Registry.ClassesRoot;

            string[] subkeys = parent.GetSubKeyNames();

            foreach (var k in subkeys)
            {
                if (!k.StartsWith("."))
                    continue;

                if (!FileUtil.FileAssocHasPrint(k) && (!FileUtil.FileAssocHasPrintTo(k)))
                    continue;

                using (RegistryKey key = parent.OpenSubKey(k))
                {
                    if (key == null)
                        continue;

                    object o = key.GetValue("");

                    if ((o == null) || !(o is string))
                        continue;

                    var typeName = o as string;

                    using (var typekey = parent.OpenSubKey(typeName, true))
                    {
                        try
                        {
                            Write(k + ": ");
                            SetRegistryValue(typekey, @"shell\" + SHELL_KEY, null, text);
                            SetRegistryValue(typekey, @"shell\" + SHELL_KEY + @"\command", null, "\"" + applicationPath + "\" /PrintFile=\"%1\"");
                            WriteLine("success.");
                        }
                        catch (Exception ex) { WriteLine("failed: " + ex.Message); }
                    }
                }
            }
        }

        private static void Write(string message)
        {
            if (!Verbose)
                return;
            Console.Write(message);
        }

        private static void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }

        private static void SetRegistryValue(RegistryKey parent, string path, string name, string value)
        {
            parent.CreateSubKey(path);

            using (var k = parent.OpenSubKey(path, true))
            {
                if (k != null)
                    k.SetValue(name, value);
            }
        }
    }
}
