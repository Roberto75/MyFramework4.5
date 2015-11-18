using System;
using SystemInterface.Microsoft.Win32;
using SystemWrapper.Microsoft.Win32;
using NLog;

namespace pdfforge.PDFCreator.Licensing
{
    public class LicenseValidator
    {
        public MachineId MachineId { get; private set; }
        public string LicenseKey { get; private set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public bool IsValidLicense
        {
            get { return CheckLicense(); }
        }

        private readonly string _licenseServerAnswer;

        public LicenseValidator()
            : this(MachineId.BuildCurrentMachineId(), new RegistryWrap())
        {

        }

        public LicenseValidator(MachineId machineId, IRegistry registry)
        {
            MachineId = machineId;

            LicenseKey = registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "License", null) as string;
            string value = registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\PDFCreator.net", "LSA", null) as string;

            try
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _licenseServerAnswer = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value));
                
                if (string.IsNullOrWhiteSpace(LicenseKey))
                    LicenseKey = null;
            }
            catch (FormatException)
            {
                _licenseServerAnswer = null;
            }
        }

        private bool CheckLicense()
        {
            if (string.IsNullOrWhiteSpace(LicenseKey))
                return false;

            if (string.IsNullOrWhiteSpace(_licenseServerAnswer))
                return false;

            string machineHash = MachineId.CaclculateMachineHash();

            var isValid = _licenseServerAnswer.ToLower().Contains("id=" + machineHash);

            return isValid;
        }
    }
}
