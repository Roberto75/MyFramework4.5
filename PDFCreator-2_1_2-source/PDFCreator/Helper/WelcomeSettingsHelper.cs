using SystemInterface.Microsoft.Win32;
using SystemWrapper.Microsoft.Win32;

namespace pdfforge.PDFCreator.Helper
{
    public class WelcomeSettingsHelper
    {
        public const string RegistryKeyForWelcomeSettings = @"HKEY_CURRENT_USER\Software\PDFCreator.net";
        public const string RegistryValueForWelcomeVersion = @"LatestWelcomeVersion";

        private readonly IRegistry _registryWrap;

        public WelcomeSettingsHelper()
        {
           _registryWrap = new RegistryWrap(); 
        }

        public WelcomeSettingsHelper(IRegistry registryWrap)
        {
            _registryWrap = registryWrap;
        }

        public bool IsFirstRun()
        {
            var currentApplicationVersion = VersionHelper.GetCurrentApplicationVersion_WithBuildnumber();
            var welcomeVersionFromRegistry = GetWelcomeVersionFromRegistry();

            if (currentApplicationVersion.Equals(welcomeVersionFromRegistry))
                return false;
            
            return true;
        }

        private string GetWelcomeVersionFromRegistry()
        {
            var value = _registryWrap.GetValue(RegistryKeyForWelcomeSettings, RegistryValueForWelcomeVersion, null);
            if (value == null)
                return "";
            return value.ToString();
        }

        public void SetCurrentApplicationVersionAsWelcomeVersionInRegistry()
        {
            var currentApplicationVersion = VersionHelper.GetCurrentApplicationVersion_WithBuildnumber();
            _registryWrap.SetValue(RegistryKeyForWelcomeSettings, RegistryValueForWelcomeVersion, currentApplicationVersion);
        }
    }
}
