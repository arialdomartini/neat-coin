using System;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Xunit;

namespace NeatCoinTest
{

    public class SimpleRSA
    {
        private static readonly UnicodeEncoding Encoding = new UnicodeEncoding();
        private RSACryptoServiceProvider _rsaCryptoServiceProvider => new RSACryptoServiceProvider(1024);

        public KeyPair GeneratePair()
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            var pair = rsaKeyPairGenerator.GenerateKeyPair();

            return new KeyPair(
                pair.SerializedPrivate(),
                pair.SerializedPublic());
        }

        public string Sign(string message, string serializedPrivateKey)
        {
            using (var rsa = _rsaCryptoServiceProvider)
            {
                rsa.ImportParameters(
                    serializedPrivateKey.ToRsaParameterPrivate());

                var signData = rsa.SignData(
                    message.ToByteArray(),
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1);

                return signData.ToBase64String();

            }
        }

        public bool Verify(string message, string signature, string publicKey)
        {
            try
            {
                using (var rsa = _rsaCryptoServiceProvider)
                {
                    rsa.ImportParameters(publicKey.ToRsaParametersPublic());

                    return rsa.VerifyData(Encoding.GetBytes(message), signature.BytesFromBase64(), HashAlgorithmName.SHA256,
                        RSASignaturePadding.Pkcs1);
                }
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public static class ConversionExtensions
    {
        private static readonly UnicodeEncoding Encoding = new UnicodeEncoding();

        public static byte[] ToByteArray(this string s) =>
            Encoding.GetBytes(s);

        public static RSAParameters ToRsaParametersPublic(this string publicKey) =>
            DotNetUtilities.ToRSAParameters(publicKey.ToPublicKey());

        public static RSAParameters ToRsaParameterPrivate(this string serializedPrivateKey) =>
            DotNetUtilities.ToRSAParameters(serializedPrivateKey.ToPrivateKey());

        private static RsaKeyParameters ToPublicKey(this string serializedPublic) =>
            (RsaKeyParameters) PublicKeyFactory.CreateKey(Convert.FromBase64String(serializedPublic));

        private static RsaPrivateCrtKeyParameters ToPrivateKey(this string serializedPrivate) =>
            (RsaPrivateCrtKeyParameters) PrivateKeyFactory.CreateKey(Convert.FromBase64String(serializedPrivate));

        public static byte[] BytesFromBase64(this string @string) =>
            Convert.FromBase64String(@string);

        internal static string ToBase64String(this byte[] o) =>
            Convert.ToBase64String(o);

        private static byte[] Encoded(this Asn1Encodable asn1Encodable) =>
            asn1Encodable.ToAsn1Object().GetDerEncoded();

        public static string SerializedPrivate(this AsymmetricCipherKeyPair pair) =>
            PrivateKeyInfoFactory
                .CreatePrivateKeyInfo(pair.Private)
                .Encoded()
                .ToBase64String();

        public static string SerializedPublic(this AsymmetricCipherKeyPair pair) =>
            SubjectPublicKeyInfoFactory
                .CreateSubjectPublicKeyInfo(pair.Public)
                .Encoded()
                .ToBase64String();
    }

    public class KeyPair
    {
        public string PrivateKey { get; }
        public string PublicKey { get; }

        public KeyPair(string privateKey, string publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    }


    public class EncryptTest
    {
        [Fact]
        public void should_sign_and_verify_a_message_with_KeyPair()
        {
            const string message = "message to sign";

            var simpleRsa = new SimpleRSA();
            var pair = simpleRsa.GeneratePair();

            var signed = simpleRsa.Sign(message, pair.PrivateKey);

            var verification = simpleRsa.Verify(message, signed, pair.PublicKey);

            verification.Should().Be(true);
        }

        [Fact]
        public void should_detect_a_fake_signature()
        {
            const string message = "message to sign";

            var simpleRsa = new SimpleRSA();
            var pair = simpleRsa.GeneratePair();

            var signed = "fake signature";

            var verification = simpleRsa.Verify(message, signed, pair.PublicKey);

            verification.Should().Be(false);
        }

        [Fact]
        public void should_detect_a_base64_fake_signature()
        {
            const string message = "message to sign";

            var simpleRsa = new SimpleRSA();
            var pair = simpleRsa.GeneratePair();

            var signed = "fake signature".ToByteArray().ToBase64String();

            var verification = simpleRsa.Verify(message, signed, pair.PublicKey);

            verification.Should().Be(false);
        }
    }
}