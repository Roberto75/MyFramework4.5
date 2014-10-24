using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyManagerCSharp
{
    public class JavaManager
    {
        public static string getJavaCurrentInstallationPath()
        {
            string javaX86Key;
            if (Environment.Is64BitOperatingSystem)
            {
                javaX86Key = "SOFTWARE\\Wow6432Node\\JavaSoft\\Java Runtime Environment\\";
            }
            else
            {
                javaX86Key = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
            }

            string java_home = "";
            string currentVersion = "";
            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaX86Key))
            {
                if (rk != null)
                {
                    currentVersion = rk.GetValue("CurrentVersion").ToString();
                    using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                    {
                        java_home = key.GetValue("JavaHome").ToString();
                    }
                }
            }

            return java_home;
        }
    }
}
