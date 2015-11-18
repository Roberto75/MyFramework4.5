using System;
using System.Collections.Generic;
using System.Reflection;
using SystemInterface.IO;
using SystemInterface.Microsoft.Win32;
using SystemWrapper.IO;
using SystemWrapper.Microsoft.Win32;

namespace pdfforge.PDFCreator.Core.Ghostscript
{
    public class GhostscriptDiscovery
    {
        private readonly IFile _file;
        private readonly IRegistry _registry = new RegistryWrap();
        private readonly IPath _path = new PathWrap();

        public GhostscriptDiscovery()
            : this(new FileWrap(), new RegistryWrap())
        {
            
        }

        public GhostscriptDiscovery(IFile file, IRegistry registry)
        {
            _file = file;
            _registry = registry;

            string assemblyPath = _path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) ?? "";

            if (assemblyPath.StartsWith(@"file:\"))
                ApplicationPath = assemblyPath.Substring(6);

            if (OsHelper.Is64BitOperatingSystem)
                RegistryPath = "SOFTWARE\\Wow6432Node\\GPL Ghostscript";
            else
                RegistryPath = "SOFTWARE\\GPL Ghostscript";
        }

        public string ApplicationPath { get; set; }

        public string RegistryPath { get; set; }

        /// <summary>
        ///     Search for Ghostscript instances in the application folder
        /// </summary>
        /// <returns>A GhostscriptVersion if an internal instance exists, null otherwise</returns>
        public GhostscriptVersion FindInternalInstance()
        {
            string[] paths = { _path.Combine(ApplicationPath, "Ghostscript"), _path.Combine(ApplicationPath, @"..\..\Ghostscript") };

            foreach (string path in paths)
            {
                string exePath = _path.Combine(path, @"Bin\gswin32c.exe");
                string libPath = _path.Combine(path, @"Bin") + ';' + _path.Combine(path, @"Lib") + ';' +
                                 _path.Combine(path, @"Fonts");

                if (_file.Exists(exePath))
                {
                    return new GhostscriptVersion("<internal>", exePath, libPath);
                }
            }

            return null;
        }

        /// <summary>
        ///     Get the internal instance if it exists, otherwise the most recent installed instance
        /// </summary>
        /// <returns>The best matching Ghostscript instance</returns>
        public GhostscriptVersion GetBestGhostscriptInstance(string minGsVersion)
        {
            var version = FindInternalInstance();

            if (version != null)
                return version;

            IList<GhostscriptVersion> versions = FindRegistryInstances(minGsVersion);

            if (versions.Count == 0)
                return null;

            return versions[0];
        }

        private IList<GhostscriptVersion> FindRegistryInstances(string minGsVersion)
        {
            var versions = new List<GhostscriptVersion>();

            IRegistryKey gsMainKey = _registry.LocalMachine.OpenSubKey(RegistryPath);

            if (gsMainKey == null)
                return versions;

            foreach (string subkey in gsMainKey.GetSubKeyNames())
            {
                GhostscriptVersion v = IsGhostscriptInstalled(subkey);

                if ((v != null) && (string.Compare(v.Version, minGsVersion, StringComparison.Ordinal) >= 0))
                    versions.Add(v);
            }

            versions.Reverse();

            return versions;
        }

        /// <summary>
        ///     Check if Ghostscript is installed with a given version. It does a lookup in the registry and checks if the paths
        ///     exist.
        /// </summary>
        /// <param name="version">Name of the version to check, i.e. "9.05"</param>
        /// <returns>A GhostscriptVersion object if a version has been found, null otherwise</returns>
        private GhostscriptVersion IsGhostscriptInstalled(string version)
        {
            try
            {
                IRegistryKey myKey = _registry.LocalMachine.OpenSubKey(RegistryPath + "\\" + version);
                if (myKey == null)
                    return null;

                var gsDll = (string)myKey.GetValue("GS_DLL");
                var gsLib = (string)myKey.GetValue("GS_LIB");

                var gsExe = _path.Combine(_path.GetDirectoryName(gsDll), "gswin32c.exe");

                myKey.Close();
                if (_file.Exists(gsExe))
                {
                    return new GhostscriptVersion(version, gsExe, gsLib);
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }
}