using System;
using System.Runtime.InteropServices;
using System.Text;
using SystemInterface.Microsoft.Win32;
using SystemWrapper.Microsoft.Win32;
using pdfforge.PDFCreator.Utilities;
using pdfforge.PDFCreator.WindowsApi;

namespace pdfforge.PDFCreator.Licensing
{
    public class MachineId
    {
        public long SystemVolumeSerial { get; private set; }
        public string WindowsProductId { get; private set; }

        public MachineId(long systemVolumeSerial, string windowsProductId)
        {
            SystemVolumeSerial = systemVolumeSerial;
            WindowsProductId = windowsProductId.Replace("-", "");
        }

        public static MachineId BuildCurrentMachineId()
        {
            return BuildCurrentMachineId(Kernel32.GetSystemVolumeSerial, new RegistryWrap());
        }

        public static MachineId BuildCurrentMachineId(Func<long> getSystemVolumeSerial, IRegistry registry)
        {
            return new MachineId(getSystemVolumeSerial(), GetWindowsProductId(registry));
        }

        private static string GetWindowsProductId(IRegistry registry)
        {
            var v = registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductId", null);
            if (v == null)
                return "";
            return v.ToString();
        }

        public string CaclculateMachineHash()
        {
            return CaclculateMachineHash("GQ461qpa6s0SeD4qabZce6JVP7sTywtN");
        }

        public string CaclculateMachineHash(string salt)
        {
            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("salt");
            
            string serialString = SystemVolumeSerial.ToString("X8");

            var hashBase = serialString + WindowsProductId + salt;
            return HashUtil.GetSha1Hash(hashBase);
        }
    }
}
