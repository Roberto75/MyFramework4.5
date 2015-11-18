using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pdfforge.PDFCreator.Helper
{
    public static class VersionHelper
    {
        /// <summary>
        /// Get current application version as version
        /// </summary>
        /// <returns>Version</returns>
        public static Version GetCurrentApplicationVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// Get current application version as string
        /// </summary>
        /// <returns>Version in format "x.x"</returns>
        public static string GetCurrentApplicationVersion_TwoDigits()
        {
            var v = GetCurrentApplicationVersion();
            var currentVersion = string.Format("{0}.{1}", v.Major, v.Minor);

            return currentVersion;
        }

        /// <summary>
        /// Get current application version as string
        /// </summary>
        /// <returns>Version in format "x.x.x"</returns>
        public static string GetCurrentApplicationVersion_ThreeDigits()
        {
            var v = GetCurrentApplicationVersion();
            var currentVersion = string.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build);

            return currentVersion;
        }

        /// <summary>
        /// Get current application version with build number
        /// </summary>
        /// <returns>Version with Buildnumber in format "vX.X.X Build XXXX"</returns>
        public static string GetCurrentApplicationVersion_WithBuildnumber()
        {
            var v = GetCurrentApplicationVersion();
            var currentVersionString = string.Format("v{0}.{1}.{2}", v.Major, v.Minor, v.Build);
            if (v.Revision == 0)
                currentVersionString += @" (Developer Preview)";
            else
                currentVersionString += string.Format(" Build {0}", v.Revision);

            return currentVersionString;
        }
    }
}
