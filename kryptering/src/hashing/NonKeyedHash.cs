using System;
using System.Text;
using System.Security.Cryptography;

namespace hashing
{
    public static class NonKeyHash
    {
        public static string Compute(string data, HashAlgorithm hash = null)
        {
            hash = hash ?? new MD5CryptoServiceProvider();

            hash.ComputeHash(Encoding.UTF8.GetBytes(data));

            return Convert.ToBase64String(hash.Hash);
        }
    }
}