using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace pdfforge.PrinterHelper
{
    // ReSharper disable InconsistentNaming
    [Flags]
    public enum PortType
    {
        Write = 0x1,
        Read = 0x2,
        Redirected = 0x4,
        NetAttached = 0x8
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PORT_INFO_2
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPortName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pMonitorName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDescription;
        public PortType fPortType;
        internal uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PRINTER_INFO_2
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pServerName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrinterName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pShareName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPortName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDriverName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pComment;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pLocation;
        public IntPtr pDevMode;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pSepFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pPrintProcessor;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDatatype;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pParameters;
        public IntPtr pSecurityDescriptor;
        public uint Attributes; // See note below!
        public uint Priority;
        public uint DefaultPriority;
        public uint StartTime;
        public uint UntilTime;
        public uint Status;
        public uint cJobs;
        public uint AveragePPM;
    }

    [FlagsAttribute]
    public enum PrinterEnumFlags
    {
        // ReSharper disable UnusedMember.Global
        PRINTER_ENUM_DEFAULT = 0x00000001,
        PRINTER_ENUM_LOCAL = 0x00000002,
        PRINTER_ENUM_CONNECTIONS = 0x00000004,
        PRINTER_ENUM_FAVORITE = 0x00000004,
        PRINTER_ENUM_NAME = 0x00000008,
        PRINTER_ENUM_REMOTE = 0x00000010,
        PRINTER_ENUM_SHARED = 0x00000020,
        PRINTER_ENUM_NETWORK = 0x00000040,
        PRINTER_ENUM_EXPAND = 0x00004000,
        PRINTER_ENUM_CONTAINER = 0x00008000,
        PRINTER_ENUM_ICONMASK = 0x00ff0000,
        PRINTER_ENUM_ICON1 = 0x00010000,
        PRINTER_ENUM_ICON2 = 0x00020000,
        PRINTER_ENUM_ICON3 = 0x00040000,
        PRINTER_ENUM_ICON4 = 0x00080000,
        PRINTER_ENUM_ICON5 = 0x00100000,
        PRINTER_ENUM_ICON6 = 0x00200000,
        PRINTER_ENUM_ICON7 = 0x00400000,
        PRINTER_ENUM_ICON8 = 0x00800000,
        PRINTER_ENUM_HIDE = 0x01000000,
        PRINTER_ENUM_ALL = PRINTER_ENUM_LOCAL + PRINTER_ENUM_NETWORK
        // ReSharper restore UnusedMember.Global
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct MONITOR_INFO_2
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pEnvironment;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDLLName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DRIVER_INFO_3
    {
        public UInt32 cVersion;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pEnvironment;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDriverPath;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDataFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pConfigFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pHelpFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDependentFiles;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pMonitorName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDefaultDataType;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DRIVER_INFO_8
    {
        public UInt32 cVersion;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pEnvironment;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDriverPath;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDataFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pConfigFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pHelpFile;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDependentFiles;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pMonitorName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pDefaultDataType;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszzPreviousNames;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftDriverDate;
        public UInt64 dwlDriverVersion;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszMfgName;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszOEMUrl;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszHardwareID;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszProvider;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszPrintProcessor;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszVendorSetup;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszzColorProfiles;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszInfPath;
        public UInt32 dwPrinterDriverAttributes;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszzCoreDriverDependencies;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftMinInboxDriverVerDate;
        public UInt64 dwlMinInboxDriverVerVersion;
    }

    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/cc244650.aspx
    /// </summary>
    [Flags]
    enum ACCESS_MASK : uint
    {
        // ReSharper disable UnusedMember.Global
        PRINTER_ACCESS_ADMINISTER = 0x00000004,
        PRINTER_ACCESS_USE = 0x00000008,
        PRINTER_ACCESS_MANAGE_LIMITED = 0x00000040,
        PRINTER_ALL_ACCESS = 0x000F000C
        // ReSharper restore UnusedMember.Global
    }

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd162839%28v=vs.85%29.aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal class PRINTER_DEFAULTS
    {
        // ReSharper disable NotAccessedField.Global
        public IntPtr pDatatype;
        public IntPtr pDevMode;
        public ACCESS_MASK DesiredAccess;
        // ReSharper restore NotAccessedField.Global
    }
    // ReSharper restore InconsistentNaming

    public class Win32Printer : IWin32Printer
    {
        // ReSharper disable InconsistentNaming
        private const int ERROR_INSUFFICIENT_BUFFER = 122;
        // ReSharper restore InconsistentNaming

        #region DLL imports
        // ReSharper disable InconsistentNaming
        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern IntPtr AddPrinter(string pName, uint level, [In] ref PRINTER_INFO_2 pPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int AddPrinterDriver(string pName, uint level, [In] ref DRIVER_INFO_3 pDriverInfo);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        static extern int ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int GetPrinterDriverDirectory(string pName, string pEnvironment, uint level, [Out] StringBuilder pDriverDirectory, uint cbBuf, [In] ref uint pcbNeened);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int AddMonitor(string pName, uint Level, ref MONITOR_INFO_2 pMonitors);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int DeletePrinterDriverEx(string pName, string pEnviroment, string pDriverName, int dwDeleteFlag, int dwVersionFlag);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int DeleteMonitor(string pName, string pEnvironment, string pMonitorName);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        static extern bool DeletePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        static extern int OpenPrinter(string pPrinterName, ref IntPtr phPrinter, PRINTER_DEFAULTS pDefault);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        static extern bool EnumPorts(string pName, uint level, IntPtr lpbPorts, uint cbBuf, ref uint pcbNeeded, ref uint pcReturned);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern bool EnumMonitors(string pName, uint level, IntPtr pMonitors, uint cbBuf, ref uint pcbNeeded, ref uint pcReturned);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern bool EnumPrinters(PrinterEnumFlags flags, string Name, uint Level, IntPtr pPrinterEnum, uint cbBuf, ref uint pcbNeeded, ref uint pcReturned);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern bool EnumPrinterDrivers(string pName, string pEnvironment, uint level, IntPtr pDriverInfo, uint cdBuf, ref uint pcbNeeded, ref uint pcRetruned);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern int SetPrinter(IntPtr hPrinter, uint Level, IntPtr pPrinter, uint command);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetPrinter(IntPtr hPrinter, uint Level, IntPtr pPrinter, uint cbBuf, ref uint pcbNeeded);
        // ReSharper restore InconsistentNaming
        #endregion

        public System.Runtime.InteropServices.ComTypes.FILETIME DateTimeToFiletime(DateTime time)
        {
            System.Runtime.InteropServices.ComTypes.FILETIME filetime;
            var hFt1 = time.ToFileTimeUtc();
            filetime.dwLowDateTime = (int)(hFt1 & 0xFFFFFFFF);
            filetime.dwHighDateTime = (int)(hFt1 >> 32);
            return filetime;
        }

        // Adds the printer
        public bool AddPrinter(string printerName, string portName, string driverName)
        {
            var printerInfo2 = new PRINTER_INFO_2
            {
                pPrinterName = printerName,
                pPortName = portName,
                pDriverName = driverName,
                pPrintProcessor = "WinPrint",
                pDatatype = "RAW",
                pComment = "PDFCreator Printer",
                pShareName = printerName,
                Priority = 1,
                DefaultPriority = 1
            };

            var hPrinter = AddPrinter("", 2, ref printerInfo2);
            if (hPrinter == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            var res = ClosePrinter(hPrinter);
            if (res == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return true;
        }

        // Deletes the printer driver
        public bool DeletePrinterDriver(PrinterEnvironment printerEnvironment, string printerDriverName)
        {
            // ReSharper disable once InconsistentNaming
            const int DPD_DELETE_UNUSED_FILES = 1;
            var result = DeletePrinterDriverEx("", printerEnvironment.GetDescription(), printerDriverName, DPD_DELETE_UNUSED_FILES, 3);

            if (result == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return true;
        }

        // Add a PDFCreator printer monitor and port
        public void AddPdfcreatorPrinterMonitor(PrinterEnvironment printerEnvironment, string monitorName, string portName, string monitorDriverFileName, string programFullFileName)
        {
            var userRoot = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Print\\Monitors\\" + monitorName;

            Registry.SetValue(userRoot, "PdfCreator", programFullFileName);
            Registry.SetValue(userRoot, "Port", portName);

            var monitorInfo2 = new MONITOR_INFO_2
            {
                pName = monitorName,
                pEnvironment = printerEnvironment.GetDescription(),
                pDLLName = monitorDriverFileName
            };

            PrinterUtil.ExtractMonitorFile(printerEnvironment, monitorDriverFileName);

            var result = AddMonitor("", 2, ref monitorInfo2);

            if (result == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        // Add a GDI PDFCreator printerdriver
        public void AddPdfcreatorPrinterDriverGdi(PrinterEnvironment printerEnvironment, string driverName, PrinterDescriptionFileLanguage language, string externalPpdFile)
        {
            DriverFiles driverFiles;
            var printerDriverDirectory = GetPrinterDriverDirectory(printerEnvironment);
            if (externalPpdFile != null && externalPpdFile.Trim().Length > 0 && File.Exists(externalPpdFile))
            {
                File.Copy(externalPpdFile, Path.Combine(printerDriverDirectory, Path.GetFileName(externalPpdFile)), true);
                PrinterUtil.ExtractPrinterFilesWindows8(printerEnvironment, language, printerDriverDirectory, false, out driverFiles);
                driverFiles.DataFile = externalPpdFile;
            }
            else
                PrinterUtil.ExtractPrinterFilesWindows8(printerEnvironment, language, printerDriverDirectory, true, out driverFiles);

            var driverInfo3 = new DRIVER_INFO_3
            {
                cVersion = 3,
                pName = driverName,
                pEnvironment = printerEnvironment.GetDescription(),
                pDefaultDataType = "RAW",
                pMonitorName = "",
                pDriverPath = driverFiles.DriverPath,
                pConfigFile = driverFiles.ConfigFile,
                pHelpFile = driverFiles.HelpFile,
                pDataFile = driverFiles.DataFile,
                pDependentFiles = string.Join("\0", driverFiles.DependentFiles.ToArray()) + "\0\0"
            };

            var result = AddPrinterDriver("", 3, ref driverInfo3);

            PrinterUtil.DeletePrinterDriverFilesFromPrinterDriverDirectory(printerDriverDirectory, externalPpdFile);

            if (result == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        // Get printer driver directoy
        private static string GetPrinterDriverDirectory(PrinterEnvironment printerEnvironment)
        {
            var printerDriverDirectory = new StringBuilder(1024);
            uint i = 0;
            var result = GetPrinterDriverDirectory(null, printerEnvironment.GetDescription(), 1, printerDriverDirectory, 1024, ref i);
            if (result == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            return printerDriverDirectory.ToString();
        }

        // Deletes the printer monitor
        public bool DeleteMonitor(string monitorName, bool deleteDriverFile)
        {
            var monitors = GetMonitors();
            var found = false;

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var monitor in monitors)
            {
                if (!monitorName.Equals(monitor.pName, StringComparison.CurrentCultureIgnoreCase)) continue;
                found = true;
                break;
            }
            if (!found)
                throw new Exception("Monitor \"" + monitorName + "\" not found!");
            // ReSharper restore LoopCanBeConvertedToQuery

            var result = DeleteMonitor("", "", monitorName);
            if (result == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            if (deleteDriverFile)
            {
                foreach (var monitor in monitors)
                {
                    if (monitorName.Equals(monitor.pName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        string driverFile = Path.Combine(Environment.SystemDirectory, monitor.pDLLName);
                        if (File.Exists(driverFile))
                        {
                            File.Delete(driverFile);
                            if (File.Exists(driverFile))
                                throw new Exception("Could not delete monitor driver file \"" + driverFile + "\"!");
                            return true;
                        }
                        throw new Exception("Driver file \"" + driverFile + "\" doesn't exist anymore!");
                    }
                }
                return false;
            }
            return false;
        }

        // Deletes the printer
        public bool DeletePrinter(string printerName)
        {
            var hPrinter = IntPtr.Zero;
            var printerDefaults = new PRINTER_DEFAULTS
            {
                DesiredAccess = ACCESS_MASK.PRINTER_ALL_ACCESS,
                pDatatype = IntPtr.Zero,
                pDevMode = IntPtr.Zero
            };

            try
            {
                if (OpenPrinter(printerName, ref hPrinter, printerDefaults) == 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                if (!DeletePrinter(hPrinter))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            finally
            {
                if (hPrinter != IntPtr.Zero && ClosePrinter(hPrinter) == 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return true;
        }

        // Get ports
        public IList<PORT_INFO_2> GetPorts()
        {
            var ports = new List<PORT_INFO_2>();
            var size = Marshal.SizeOf(typeof(PORT_INFO_2));
            uint pcbNeeded = 0;
            uint pcReturned = 0;

            if (EnumPorts(null, 2, IntPtr.Zero, 0, ref pcbNeeded, ref pcReturned))
                return ports;

            if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
            {
                var pPorts = Marshal.AllocHGlobal((int)pcbNeeded);
                if (EnumPorts(null, 2, pPorts, pcbNeeded, ref pcbNeeded, ref pcReturned))
                {
                    var currentPort = pPorts;

                    for (var i = 0; i < pcReturned; i++)
                    {
                        ports.Add((PORT_INFO_2)Marshal.PtrToStructure(currentPort, typeof(PORT_INFO_2)));
                        currentPort = currentPort + size;
                    }
                    Marshal.FreeHGlobal(pPorts);

                    return ports;
                }
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        // Get Monitor
        public bool GetMonitor(string portName, out string monitorName)
        {
            monitorName = "";
            var ports = GetPorts();
            foreach (var port in ports)
            {
                if (port.pMonitorName.Equals(portName, StringComparison.CurrentCultureIgnoreCase))
                {
                    monitorName = port.pMonitorName;
                    return true;
                }
            }
            return false;
        }

        // Get monitors
        public IList<MONITOR_INFO_2> GetMonitors()
        {
            var monitors = new List<MONITOR_INFO_2>();
            var size = Marshal.SizeOf(typeof(MONITOR_INFO_2));
            uint pcbNeeded = 0;
            uint pcReturned = 0;

            if (EnumMonitors(null, 2, IntPtr.Zero, 0, ref pcbNeeded, ref pcReturned))
                return monitors;

            if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
            {
                var pMonitors = Marshal.AllocHGlobal((int)pcbNeeded);
                if (EnumMonitors(null, 2, pMonitors, pcbNeeded, ref pcbNeeded, ref pcReturned))
                {
                    var currentMonitor = pMonitors;

                    for (var i = 0; i < pcReturned; i++)
                    {
                        monitors.Add((MONITOR_INFO_2)Marshal.PtrToStructure(currentMonitor, typeof(MONITOR_INFO_2)));
                        currentMonitor = currentMonitor + size;
                    }
                    Marshal.FreeHGlobal(pMonitors);

                    return monitors;
                }
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        // Get drivers
        public IList<DRIVER_INFO_3> GetPrinterDriver(PrinterEnvironment printerEnvironment)
        {
            var drivers = new List<DRIVER_INFO_3>();
            var size = Marshal.SizeOf(typeof(DRIVER_INFO_3));
            uint pcbNeeded = 0;
            uint pcReturned = 0;

            if (EnumPrinterDrivers(null, printerEnvironment.GetDescription(), 3, IntPtr.Zero, 0, ref pcbNeeded, ref pcReturned))
                return drivers;

            if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
            {
                var pDrivers = Marshal.AllocHGlobal((int)pcbNeeded);
                if (EnumPrinterDrivers(null, printerEnvironment.GetDescription(), 3, pDrivers, pcbNeeded, ref pcbNeeded, ref pcReturned))
                {
                    var currentDriver = pDrivers;

                    for (var i = 0; i < pcReturned; i++)
                    {
                        drivers.Add((DRIVER_INFO_3)Marshal.PtrToStructure(currentDriver, typeof(DRIVER_INFO_3)));
                        currentDriver = currentDriver + size;
                    }
                    Marshal.FreeHGlobal(pDrivers);

                    return drivers;
                }
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// This method returns a list of installed printer.
        /// </summary>
        /// <param name="printerEnumFlags">A flag that describes the kind of printer (e.g. local, network or both).</param>
        /// <returns>
        /// A list of all installed printer. 
        /// </returns>
        public IList<PRINTER_INFO_2> GetPrinters(PrinterEnumFlags printerEnumFlags)
        {
            var printers = new List<PRINTER_INFO_2>();
            var size = Marshal.SizeOf(typeof(PRINTER_INFO_2));
            uint pcbNeeded = 0;
            uint pcReturned = 0;
            if (EnumPrinters(printerEnumFlags, null, 2, IntPtr.Zero, 0, ref pcbNeeded, ref pcReturned))
                return printers;

            if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
            {
                var pPrinters = Marshal.AllocHGlobal((int)pcbNeeded);
                if (EnumPrinters(printerEnumFlags, null, 2, pPrinters, pcbNeeded, ref pcbNeeded, ref pcReturned))
                {
                    var currentPrinter = pPrinters;

                    for (var i = 0; i < pcReturned; i++)
                    {
                        printers.Add((PRINTER_INFO_2)Marshal.PtrToStructure(currentPrinter, typeof(PRINTER_INFO_2)));
                        currentPrinter = currentPrinter + size;
                    }

                    Marshal.FreeHGlobal(pPrinters);
                    return printers;
                }
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public IList<PRINTER_INFO_2> GetPrinters(PrinterEnumFlags printerEnumFlags, string monitorName)
        {
            var printers = GetPrinters(printerEnumFlags);
            var printersWithMonitor = new List<PRINTER_INFO_2>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var printer in printers)
            {
                string printerMonitorName;
                if (GetMonitor(printer.pPortName, out printerMonitorName))
                {
                    if (monitorName.Equals(printerMonitorName, StringComparison.CurrentCultureIgnoreCase))
                        printersWithMonitor.Add(printer);
                }
            }
            return printersWithMonitor;
        }

        /// <summary>
        /// This method returns a list of locally installed printer.
        /// </summary>
        /// <returns>
        /// A list of locally installed printer. 
        /// </returns>
        public IList<PRINTER_INFO_2> GetLocalPrinters()
        {
            return GetPrinters(PrinterEnumFlags.PRINTER_ENUM_LOCAL);
        }

        /// <summary>
        /// Returns a list of local printers that use a specific printer driver.
        /// </summary>
        /// <param name="driverName">The name of the printer driver.</param>
        /// <returns>
        /// A list of local printers that use a specific printer driver. 
        /// </returns>
        public IList<PRINTER_INFO_2> GetLocalPrinters(string driverName)
        {
            PrinterEnvironment printerEnvironment;
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (Environment.Is64BitOperatingSystem)
                printerEnvironment = PrinterEnvironment.WindowsX64;
            else
                printerEnvironment = PrinterEnvironment.WindowsNtX86;

            var printersWithSpecificDriver = new List<PRINTER_INFO_2>();
            if (!IsDriverInstalled(printerEnvironment, driverName))
                return printersWithSpecificDriver;
            var printers = GetPrinters(PrinterEnumFlags.PRINTER_ENUM_LOCAL);
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var printer in printers)
            {
                if (printer.pDriverName.Equals(driverName, StringComparison.CurrentCultureIgnoreCase))
                    printersWithSpecificDriver.Add(printer);
            }
            return printersWithSpecificDriver;
        }

        // Check port
        public bool IsPortInstalled(string portName)
        {
            var ports = GetPorts();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var port in ports)
            {
                if (port.pPortName.Equals(portName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        // Check monitor
        public bool IsMonitorInstalled(string monitorName)
        {
            var monitors = GetMonitors();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var monitor in monitors)
            {
                if (monitor.pName.Equals(monitorName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        // Check driver
        public bool IsDriverInstalled(PrinterEnvironment printerEnvironment, string driverName)
        {
            var drivers = GetPrinterDriver(printerEnvironment);
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var driver in drivers)
            {
                if (driver.pName.Equals(driverName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// This method checks whether an installed printer (locally or network) exists.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <param name="printerEnumFlags">A flag that describes the kind of printer (e.g. local or network).</param>
        /// <returns>
        /// <c>true</c>: If the printer exists. Otherwise <c>false</c>.
        /// </returns>
        public bool IsPrinterInstalled(string printerName, PrinterEnumFlags printerEnumFlags)
        {
            var printers = GetPrinters(printerEnumFlags);
            // ReSharper disable once LoopCanBeConvertedToQuery

            foreach (var printer in printers)
            {
                if (printer.pPrinterName.Equals(printerName, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// This method checks whether a local installed printer exists.
        /// </summary>
        /// <returns>
        /// <c>true</c>: If the local printer exists. Otherwise <c>false</c>.
        /// </returns>
        public bool IsPrinterInstalled(string printerName)
        {
            return IsPrinterInstalled(printerName, PrinterEnumFlags.PRINTER_ENUM_LOCAL);
        }

        // Rename printer
        public bool RenamePrinter(string oldPrinterName, string newPrintername)
        {
            var hPrinter = IntPtr.Zero;
            var printerDefaults = new PRINTER_DEFAULTS
            {
                DesiredAccess = ACCESS_MASK.PRINTER_ALL_ACCESS,
                pDatatype = IntPtr.Zero,
                pDevMode = IntPtr.Zero
            };

            try
            {
                if (OpenPrinter(oldPrinterName, ref hPrinter, printerDefaults) == 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                uint pcbNeeded = 0;
                if (GetPrinter(hPrinter, 2, IntPtr.Zero, 0, ref pcbNeeded) != 0)
                    return false;

                if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    var pAddr = Marshal.AllocHGlobal((int)pcbNeeded);
                    if (GetPrinter(hPrinter, 2, pAddr, pcbNeeded, ref pcbNeeded) != 0)
                    {
                        var printerInfo2 = (PRINTER_INFO_2)Marshal.PtrToStructure(pAddr, typeof(PRINTER_INFO_2));
                        var pAddr2 = Marshal.AllocHGlobal(Marshal.SizeOf(printerInfo2));
                        printerInfo2.pPrinterName = newPrintername;
                        Marshal.StructureToPtr(printerInfo2, pAddr2, false);
                        if (SetPrinter(hPrinter, 2, pAddr2, 0) == 0)
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        Marshal.FreeHGlobal(pAddr2);
                    }
                    else
                        return false;
                    Marshal.FreeHGlobal(pAddr);
                }
                else
                    return false;

            }
            finally
            {
                if (hPrinter != IntPtr.Zero)
                {
                    if (ClosePrinter(hPrinter) == 0)
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            return true;
        }

        /// <summary>
        /// This method checks if the printers of a given list are installed and returns all installed printers.
        /// </summary>
        /// <returns>
        /// Return a list of installed printers.
        /// </returns>
        public IList<string> GetInstalledPrintersFromList(IEnumerable<string> printers)
        {
            var alreadyInstalledPrinters = new List<string>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var printer in printers)
            {
                if (IsPrinterInstalled(printer))
                    alreadyInstalledPrinters.Add(printer);
            }

            return alreadyInstalledPrinters;
        }

        // Check deletable printers
        public bool ArePrintersDeletable(IEnumerable<string> printers, out List<string> notInstalledPrinters)
        {
            notInstalledPrinters = new List<string>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var printer in printers)
            {
                if (!IsPrinterInstalled(printer))
                    notInstalledPrinters.Add(printer);
            }
            return notInstalledPrinters.Count == 0;
        }

        public bool GetPrinter(string printerName, PrinterEnumFlags printerEnumFlags, out PRINTER_INFO_2 printer)
        {
            printer = new PRINTER_INFO_2();
            var printers = GetPrinters(PrinterEnumFlags.PRINTER_ENUM_LOCAL);
            foreach (var printer1 in printers)
            {
                if (printer1.pPrinterName.Equals(printerName, StringComparison.CurrentCultureIgnoreCase))
                {
                    printer = printer1;
                    return true;
                }
            }
            return false;
        }

        // Check printers with specific driver
        public bool ArePrintersWithSpecificDriverDeletable(IEnumerable<string> printers, string driverName, PrinterEnumFlags printerEnumFlags, out List<string> notInstalledPrinters, out List<string> notPrintersWithSpecificDriver)
        {
            notInstalledPrinters = new List<string>();
            notPrintersWithSpecificDriver = new List<string>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var printer1 in printers)
            {
                if (!IsPrinterInstalled(printer1))
                    notInstalledPrinters.Add(printer1);
                else
                {
                    PRINTER_INFO_2 printer;
                    GetPrinter(printer1, printerEnumFlags, out printer);
                    if (!printer.pDriverName.Equals(driverName, StringComparison.CurrentCultureIgnoreCase))
                        notPrintersWithSpecificDriver.Add(printer1);
                }
            }
            return (notInstalledPrinters.Count == 0) && (notPrintersWithSpecificDriver.Count == 0);
        }

        public IError AdaptFreememPrinterSetting(string printerName, int memorySize)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Print\Printers\" + printerName + @"\PrinterDriverData", true))
            {
                if (key != null)
                {
                    key.SetValue("FreeMem", memorySize, RegistryValueKind.DWord);
                    return null;
                }
                return new Error(Error.ERROR_INTERNAL, string.Format(Messages.PrinterFreememSettingFailure, printerName));
            }
        }
    }
}
