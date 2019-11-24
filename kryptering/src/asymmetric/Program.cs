using System;
using System.Text;
using System.Security.Cryptography;

namespace asymmetric
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = "Dette er en hemmelig tekst";

            RSACryptoServiceProvider decryptor = new RSACryptoServiceProvider();
            RSAParameters publicKey = decryptor.ExportParameters(false);

            // Encrypt
            RSACryptoServiceProvider encryptor = new RSACryptoServiceProvider();
            encryptor.ImportParameters(publicKey);
            byte[] messageBytes = Encoding.Unicode.GetBytes(text);
            byte[] encryptedMessage = encryptor.Encrypt(messageBytes, false);
            var cypher = Convert.ToBase64String(encryptedMessage);

            // Decrypt
            byte[] decryptedBytes = decryptor.Decrypt(Convert.FromBase64String(cypher), false);
            var clear = Encoding.Unicode.GetString(decryptedBytes);

            Console.WriteLine($"{text} => {cypher} => {clear} | validate: {text == clear}");
        }
    }
}
