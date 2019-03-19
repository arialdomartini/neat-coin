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

        public T WithRsa<T>(Func<RSACryptoServiceProvider, T> func)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                return func(rsa);
            }
        }

        public KeyPair GenerateKeyPair() =>
            WithRsa(rsa =>
                new KeyPair(
                    rsa.ExportParameters(true),
                    rsa.ExportParameters(false)));

        public byte[] Encrypt(string message, RSAParameters publicKey) =>
            WithRsa(rsa => rsa
                .LoadKey(publicKey)
                .Encrypt(_encoding.GetBytes(message), false));

        public byte[] Decrypt(byte[] dataToDecrypt, RSAParameters privateKey) =>
            WithRsa(rsa => rsa
                .LoadKey(privateKey)
                .Decrypt(dataToDecrypt, false));

        private byte[] Sign(string message, RSAParameters privateKey) =>
            WithRsa(rsa => rsa
                .LoadKey(privateKey)
                .SignData(_encoding.GetBytes(message), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));

        private bool Verify(string message, byte[] signature, RSAParameters publicKey) =>
            WithRsa(rsa => rsa
                .LoadKey(publicKey)
                .VerifyData(_encoding.GetBytes(message), signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)
            );
    }

    public static class RsaCryptoServiceProviderExtensions
    {
        public static RSACryptoServiceProvider LoadKey(this RSACryptoServiceProvider rsa, RSAParameters publicKey)
        {
            rsa.ImportParameters(publicKey);
            return rsa;
        }
    }
}