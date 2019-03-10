using System;
using System.Security.Cryptography;
using System.Text;

namespace NeatCoin
{
    internal class Hash
    {
        private readonly SHA256Managed _sha256Managed;

        public Hash(SHA256Managed sha256Managed)
        {
            _sha256Managed = sha256Managed;
        }

        internal string CalculateHash(string @string) => 
            Convert.ToBase64String(_sha256Managed.ComputeHash(Encoding.Default.GetBytes(@string)));
    }
}