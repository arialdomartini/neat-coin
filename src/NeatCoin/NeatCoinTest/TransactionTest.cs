using System;
using FluentAssertions;
using Xunit;
using System.Security.Cryptography;
using System.Text;

namespace NeatCoinTest
{

    public class TransactionTest
    {
        private readonly UnicodeEncoding _encoding = new UnicodeEncoding();

        [Fact]
        public void should_encrypt_and_decrypt_a_message()
        {
            const string message = "message to sign";

            using (var rsa = new RSACryptoServiceProvider())
            {
                var publicKey = rsa.ExportParameters(false);
                var privateKey = rsa.ExportParameters(true);

                var encrypted = Encrypt(message, publicKey);
                var decryptedData = Decrypt(encrypted, privateKey);


                _encoding.GetString(decryptedData).Should().Be(message);
            }
        }

        [Fact]
        public void should_sign_and_verify_a_message()
        {
            const string message = "message to sign";

            var keyPair = GenerateKeyPair();

            var signature = Sign(message, keyPair.PrivateKey);
            var verification = Verify(message, signature, keyPair.PublicKey);

            verification.Should().Be(true);
        }

        public T WithRsa<T>(Func<RSA, T> func)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                return func(rsa);
            }
        }
        
        public KeyPair GenerateKeyPairold()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                return new KeyPair(
                    rsa.ExportParameters(true),
                    rsa.ExportParameters(false));
            }
        }
        public KeyPair GenerateKeyPair() =>
            WithRsa(GenerateKeyPair);

        private static KeyPair GenerateKeyPair(RSA rsa) =>
            new KeyPair(
                rsa.ExportParameters(true),
                rsa.ExportParameters(false));

        private byte[] Encrypt(string message, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Encrypt(_encoding.GetBytes(message), false);
            }
        }

        private static byte[] Decrypt(byte[] dataToDecrypt, RSAParameters rsaKeyInfo)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaKeyInfo);
                return rsa.Decrypt(dataToDecrypt, false);
            }
        }

        private byte[] Sign(string message, RSAParameters privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(_encoding.GetBytes(message), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        private bool Verify(string message, byte[] signature, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.VerifyData(_encoding.GetBytes(message), signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

    }
}