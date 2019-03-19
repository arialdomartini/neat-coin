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
        public void should_sign_and_verify_a_message()
        {
            const string message = "message to sign";

            using (var rsa = new RSACryptoServiceProvider())
            {
                var privateKey = rsa.ExportParameters(true);
                var publicKey = rsa.ExportParameters(false);

                var signature = Sign(message, privateKey);
                var verification = Verify(message, signature, publicKey);

                verification.Should().Be(true);
            }
        }

        private byte[] Sign(string message, RSAParameters privateKey)
        {
            var dataToEncrypt = _encoding.GetBytes(message);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);
                return rsa.SignData(dataToEncrypt, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
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