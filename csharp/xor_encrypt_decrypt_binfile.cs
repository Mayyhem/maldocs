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
            if (args.Length != 2)
            {
                Console.WriteLine("Error: Please provide 1) the path to the binary file to encrypt, and 2) the encryption/decryption key.");
                Console.WriteLine("Usage: ./XorEncryptDecrypt.exe ./met.bin 'MyS3cr3tK3y'");
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

            // Perform XOR on code and write to files
            byte[] XordBytes = XorByteArray(codeBytes, key);
            File.WriteAllBytes("out.bin", XordBytes);
            Console.WriteLine("Encrypted binary written to out.bin");
              
            string csCode = "byte[] codeBytes = new byte[" + XordBytes.Length + "] " + "{ " + ByteArrayToString(XordBytes) + " };";
            File.WriteAllText("out.cs", csCode);
            Console.WriteLine("Encrypted byte[] C# code written to out.cs");

            string b64String = "string b64String = \"" + Convert.ToBase64String(XordBytes) + "\";";
            File.WriteAllText("out.txt", b64String);
            Console.WriteLine("Base64-encoded encrypted byte[] C# code written to out.txt");
        }
    }
}
