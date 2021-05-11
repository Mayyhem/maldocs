using System;
using System.IO;
using System.Text;

namespace ConvertBinToCSharp
{
    class Program
    {
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
            if (args.Length != 1)
            {
                Console.WriteLine("Error: Please provide 1) the path to the binary file to convert to C#");
                Console.WriteLine("Usage: ./ConvertBinToCSharp.exe ./met.bin");
                Environment.Exit(1);
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Could not find file at provided path: {0}", args[0]);
                Environment.Exit(1);
            }

            // Get code from file
            byte[] codeBytes = File.ReadAllBytes(args[0]);


            // Write code to stdout
            Console.WriteLine("byte[] codeBytes = new byte[{0}] {1}", codeBytes.Length, "{ " + ByteArrayToString(codeBytes) + " };");
        }
    }
}