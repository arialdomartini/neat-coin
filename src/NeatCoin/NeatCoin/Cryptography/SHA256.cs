using System;
using System.Security.Cryptography;
using System.Text;

namespace NeatCoin.Cryptography
{
    internal class SHA256 : ICryptography
    {
        private readonly SHA256Managed _sha256Managed;

        public SHA256(SHA256Managed sha256Managed)
        {
            _sha256Managed = sha256Managed;
        }

        public string HashOf(string @string) => 
            Convert.ToBase64String(_sha256Managed.ComputeHash(Encoding.Default.GetBytes(@string)));
    }
}