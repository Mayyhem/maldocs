using System;
using System.IO;
using System.Text;

namespace XorEncryptDecrypt
{
    class Program
    {
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

        static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("0x{0:x2},", b);
            hex.Length--;
            return hex.ToString();
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Error: Please provide 1) the path to the binary file to encrypt, 2) the encryption/decryption key, and 3) the path to an output filename ending in '.bin'.");
                Console.WriteLine("Usage: ./XorEncryptDecrypt.exe ./met.bin 'MyS3cr3tK3y' ./xor.bin");
                Environment.Exit(1);
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Could not find file at provided path: {0}", args[0]);
                Environment.Exit(1);
            }

            // Get code from file
            byte[] codeBytes = File.ReadAllBytes(args[0]);

            // Get encryption key
            string key = args[1];

            // Perform XOR on code
            byte[] XordBytes = XorByteArray(codeBytes, key);

            // Write XORd code to file
            File.WriteAllBytes(args[2], XordBytes);
            Console.WriteLine("Encrypted binary written to: {0}", args[2]);
            Console.WriteLine("byte[] codeBytes = new byte[{0}] {1}", XordBytes.Length, "{ " + ByteArrayToString(XordBytes) + " };");
        }
    }
}