using System.Security.Cryptography;

namespace NeatCoinTest
{
    public class KeyPair
    {
        public RSAParameters PrivateKey { get; }
        public RSAParameters PublicKey { get; }

        public KeyPair(RSAParameters privateKey, RSAParameters publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    }
}