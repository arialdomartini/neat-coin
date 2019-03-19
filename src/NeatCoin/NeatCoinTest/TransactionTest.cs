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
        public void should_crypt_and_decrypt_back_a_message()
        {
            const string message = "message to sign";

            using (var rsa = new RSACryptoServiceProvider())
            {
                var publicKey = rsa.ExportParameters(false);
                var privateKey = rsa.ExportParameters(true);

                var crypted = Crypt(message, publicKey);
                var decryptedData = RsaDecrypt(crypted, privateKey);


                _encoding.GetString(decryptedData).Should().Be(message);
            }
        }

        private byte[] Crypt(string message, RSAParameters publicKey)
        {
            byte[] dataToEncrypt = _encoding.GetBytes(message);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                return rsa.Encrypt(dataToEncrypt, false);
            }
        }

        private static byte[] RsaDecrypt(byte[] dataToDecrypt, RSAParameters rsaKeyInfo)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaKeyInfo);
                return rsa.Decrypt(dataToDecrypt, false);
            }
        }

    }
}