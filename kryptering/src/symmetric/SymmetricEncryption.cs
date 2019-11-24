using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace symmetric
{
    public static class SymmetricEncryption
    {
        public static string Encrypt(string clearText, string password, string salt, SymmetricAlgorithm algorithm = null)
        {
            try
            {
                algorithm = algorithm ?? new RijndaelManaged();

                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt.PadRight(8)));
                algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
                algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

                string cypherText = null;
                using (MemoryStream output = new MemoryStream())
                {
                    using (CryptoStream crypto = new CryptoStream(output, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        var bytes = Encoding.UTF8.GetBytes(clearText);
                        crypto.Write(bytes, 0, bytes.Length);
                    }

                    cypherText = Convert.ToBase64String(output.ToArray());
                }

                return cypherText;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                algorithm?.Clear();
            }
        }

        public static string Decrypt(string cypherText, string password, string salt, SymmetricAlgorithm algorithm = null)
        {
            try
            {
                algorithm = algorithm ?? new RijndaelManaged();

                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt.PadRight(8)));
                algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
                algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

                string clearText = null;
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream crypto = new CryptoStream(stream, algorithm.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] data = Convert.FromBase64String(cypherText);
                        crypto.Write(data, 0, data.Length);
                    }

                    clearText = Encoding.UTF8.GetString(stream.ToArray());
                }

                return clearText;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                algorithm?.Clear();
            }
        }
    }
}