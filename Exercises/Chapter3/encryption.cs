using System;
using System.Text;
using System.Security.Cryptography;
class Program
{
    public static byte[] key = Encoding.UTF8.GetBytes("ENCRYPTIONISGOOD");
    public static byte[] iv = Encoding.UTF8.GetBytes("NOITISNTTHATGOOD");

    public static void Main(string[] args)
    {
        string text = String.Join(" ",args);
        Console.WriteLine("Text to encrypt: {0}",text);
        string a = Encrypt(text);
        Console.WriteLine("Text encrypted base64: {0}",a);
        string actual = Decrypt(a);
        if (text == actual){Console.WriteLine("All good.");}
    }
    public static string Encrypt(string text)
    {
        SymmetricAlgorithm algorithm = Aes.Create();
        ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
        byte[] inputbuffer = Encoding.UTF8.GetBytes(text);
        byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Convert.ToBase64String(outputBuffer);
    }

    public static string Decrypt(string text)
    {
        SymmetricAlgorithm algorithm = Aes.Create();
        ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
        byte[] inputbuffer = Convert.FromBase64String(text);
        byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
        return Encoding.UTF8.GetString(outputBuffer);
    }
}