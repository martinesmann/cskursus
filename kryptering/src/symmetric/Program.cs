using System;
using System.Security.Cryptography;

namespace symmetric
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates the default implementation, which is RijndaelManaged. (.net classic/windows only) 
            var algorithm = SymmetricAlgorithm.Create("Rijndael");

            //var algorithm = RijndaelManaged.Create();
            //var algorithm = TripleDES.Create();
            //var algorithm = AesManaged.Create();
            //var algorithm = new RC2CryptoServiceProvider();
            //var algorithm = new DESCryptoServiceProvider();

            var text = "Dette er en hemmelig tekst";

            var cypher = SymmetricEncryption.Encrypt(text, "mit kodeord", "salt", algorithm);
            var clear = SymmetricEncryption.Decrypt(cypher, "mit kodeord", "salt", algorithm);

            Console.WriteLine($"{text} => {cypher} => {clear} | validate: {text == clear}");
        }
    }
}
