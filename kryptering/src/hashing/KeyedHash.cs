using System;
using System.Text;
using System.Security.Cryptography;

namespace hashing
{
    public static class KeyHash
    {
        public static string Compute(string data, string key = null, KeyedHashAlgorithm hash = null)
        {
            hash = hash ??
                (key == null ? new HMACSHA1() : new HMACSHA1(Encoding.UTF8.GetBytes(key)));

            hash.ComputeHash(Encoding.UTF8.GetBytes(data));

            return Convert.ToBase64String(hash.Hash);
        }
    }
}