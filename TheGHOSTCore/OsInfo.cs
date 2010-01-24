using System;
using System.Runtime.InteropServices;

namespace Nanook.TheGhost
{
    public enum Platform
    {
        X86,
        X64,
        Unknown
    }
    
    /// <summary>
	/// OSInfo Class
	/// </summary>
	public class OsInfo
	{
		[StructLayout(LayoutKind.Sequential)] 
			private struct OSVERSIONINFOEX
		{ 
			public int dwOSVersionInfoSize;
			public int dwMajorVersion;
			public int dwMinorVersion;
			public int dwBuildNumber;
			public int dwPlatformId;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;
			public short wServicePackMajor;
			public short wServicePackMinor;
			public short wSuiteMask;
			public byte wProductType;
			public byte wReserved;
		}

        internal const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
        internal const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
        internal const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
        internal const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public UIntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        };

        [DllImport("kernel32.dll")]
        internal static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        public static Platform GetPlatform()
        {
            try
            {
                SYSTEM_INFO sysInfo = new SYSTEM_INFO();
                GetNativeSystemInfo(ref sysInfo);

                switch (sysInfo.wProcessorArchitecture)
                {
                    case PROCESSOR_ARCHITECTURE_AMD64:
                        return Platform.X64;

                    case PROCESSOR_ARCHITECTURE_INTEL:
                        return Platform.X86;

                    default:
                        return Platform.Unknown;
                }
            }
            catch
            {
                return Platform.Unknown;
            }
        }

        public static int ProcessorCount()
        {
            try
            {
                SYSTEM_INFO sysInfo = new SYSTEM_INFO();
                GetNativeSystemInfo(ref sysInfo);
                return (int)sysInfo.dwNumberOfProcessors;
            }
            catch
            {
                return 0;
            }
        }

		[DllImport("kernel32.dll")]
		private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

		#region Private Constants
		private const int VER_NT_WORKSTATION = 1;
		private const int VER_NT_DOMAIN_CONTROLLER = 2;
		private const int VER_NT_SERVER = 3;
		private const int VER_SUITE_SMALLBUSINESS = 1;
		private const int VER_SUITE_ENTERPRISE = 2;
		private const int VER_SUITE_TERMINAL = 16;
		private const int VER_SUITE_DATACENTER = 128;
		private const int VER_SUITE_SINGLEUSERTS = 256;
		private const int VER_SUITE_PERSONAL = 512;
		private const int VER_SUITE_BLADE = 1024;
		#endregion

		#region Public Methods
		/// <summary>
		/// Returns the product type of the operating system running on this computer.
		/// </summary>
		/// <returns>A string containing the the operating system product type.</returns>
		public static string GetOSProductType()
		{
            try
            {
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
                OperatingSystem osInfo = Environment.OSVersion;

                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (!GetVersionEx(ref osVersionInfo))
                {
                    return "";
                }
                else
                {
                    if (osInfo.Version.Major == 4)
                    {
                        if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                        {
                            // Windows NT 4.0 Workstation
                            return " Workstation";
                        }
                        else if (osVersionInfo.wProductType == VER_NT_SERVER)
                        {
                            // Windows NT 4.0 Server
                            return " Server";
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (osInfo.Version.Major == 5)
                    {
                        if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                        {
                            if ((osVersionInfo.wSuiteMask & VER_SUITE_PERSONAL) == VER_SUITE_PERSONAL)
                            {
                                // Windows XP Home Edition
                                return " Home Edition";
                            }
                            else
                            {
                                // Windows XP / Windows 2000 Professional
                                return " Professional";
                            }
                        }
                        else if (osVersionInfo.wProductType == VER_NT_SERVER)
                        {
                            if (osInfo.Version.Minor == 0)
                            {
                                if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                                {
                                    // Windows 2000 Datacenter Server
                                    return " Datacenter Server";
                                }
                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                                {
                                    // Windows 2000 Advanced Server
                                    return " Advanced Server";
                                }
                                else
                                {
                                    // Windows 2000 Server
                                    return " Server";
                                }
                            }
                            else
                            {
                                if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                                {
                                    // Windows Server 2003 Datacenter Edition
                                    return " Datacenter Edition";
                                }
                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                                {
                                    // Windows Server 2003 Enterprise Edition
                                    return " Enterprise Edition";
                                }
                                else if ((osVersionInfo.wSuiteMask & VER_SUITE_BLADE) == VER_SUITE_BLADE)
                                {
                                    // Windows Server 2003 Web Edition
                                    return " Web Edition";
                                }
                                else
                                {
                                    // Windows Server 2003 Standard Edition
                                    return " Standard Edition";
                                }
                            }
                        }
                    }
                }

                return "";
            }
            catch
            {
                return "";
            }
		}

		/// <summary>
		/// Returns the service pack information of the operating system running on this computer.
		/// </summary>
		/// <returns>A string containing the the operating system service pack information.</returns>
		public static string GetOSServicePack()
		{
            try
            {
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();

                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (!GetVersionEx(ref osVersionInfo))
                    return string.Empty;
                else
                    return " " + osVersionInfo.szCSDVersion;
            }
            catch
            {
                return string.Empty;
            }
		}

		/// <summary>
		/// Returns the name of the operating system running on this computer.
		/// </summary>
		/// <returns>A string containing the the operating system name.</returns>
		public static string GetOSName()
		{
            try
            {
                OperatingSystem osInfo = Environment.OSVersion;
                string osName = "UNKNOWN";

                switch (osInfo.Platform)
                {
                    case PlatformID.Win32Windows:
                        {
                            switch (osInfo.Version.Minor)
                            {
                                case 0:
                                    {
                                        osName = "Windows 95";
                                        break;
                                    }

                                case 10:
                                    {
                                        if (osInfo.Version.Revision.ToString() == "2222A")
                                        {
                                            osName = "Windows 98 Second Edition";
                                        }
                                        else
                                        {
                                            osName = "Windows 98";
                                        }
                                        break;
                                    }

                                case 90:
                                    {
                                        osName = "Windows Me";
                                        break;
                                    }
                            }
                            break;
                        }

                    case PlatformID.Win32NT:
                        {
                            switch (osInfo.Version.Major)
                            {
                                case 3:
                                    {
                                        osName = "Windows NT 3.51";
                                        break;
                                    }

                                case 4:
                                    {
                                        osName = "Windows NT 4.0";
                                        break;
                                    }

                                case 5:
                                    {
                                        if (osInfo.Version.Minor == 0)
                                        {
                                            osName = "Windows 2000";
                                        }
                                        else if (osInfo.Version.Minor == 1)
                                        {
                                            osName = "Windows XP";
                                        }
                                        else if (osInfo.Version.Minor == 2)
                                        {
                                            osName = "Windows Server 2003";
                                        }
                                        break;
                                    }

                                case 6:
                                    {
                                        osName = "Windows Longhorn";
                                        break;
                                    }
                            }
                            break;
                        }
                }

                return osName;
            }
            catch
            {
                return string.Empty;
            }
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets the full version of the operating system running on this computer.
		/// </summary>
		public static string OSVersion
		{
			get
			{
                try
                {
                    return Environment.OSVersion.Version.ToString();
                }
                catch
                {
                    return string.Empty;
                }
			}
		}
		
		/// <summary>
		/// Gets the major version of the operating system running on this computer.
		/// </summary>
		public static int OSMajorVersion
		{
			get
			{
                try
                {
			    	return Environment.OSVersion.Version.Major;
                }
                catch
                {
                    return 0;
                }
            }
		}
		
		/// <summary>
		/// Gets the minor version of the operating system running on this computer.
		/// </summary>
		public static int OSMinorVersion
		{
			get
			{
				return Environment.OSVersion.Version.Minor;
			}
		}
		
		/// <summary>
		/// Gets the build version of the operating system running on this computer.
		/// </summary>
		public static int OSBuildVersion
		{
			get
			{
                try
                {
    				return Environment.OSVersion.Version.Build;
                }
                catch
                {
                    return 0;
                }
			}
		}
		
		/// <summary>
		/// Gets the revision version of the operating system running on this computer.
		/// </summary>
		public static int OSRevisionVersion
		{
			get
			{
                try
                {
    				return Environment.OSVersion.Version.Revision;
                }
                catch
                {
                    return 0;
                }
			}
		}
		#endregion
	}
}
