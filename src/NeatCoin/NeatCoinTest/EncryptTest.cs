using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Xunit;

namespace NeatCoinTest
{

    public class Helper
    {
        private static readonly UnicodeEncoding Encoding = new UnicodeEncoding();

        public static AsymmetricCipherKeyPair Generate()
        {
            var rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(), 1024));
            return rsaKeyPairGenerator.GenerateKeyPair();
        }

        public static byte[] Sign(AsymmetricCipherKeyPair pair, string message)
        {
            var serializedPrivate = SerializedPrivate(pair);

            var serializedPublic = SerializedPublic(pair);

            RsaPrivateCrtKeyParameters privateKey = PrivateKey(serializedPrivate);
            RsaKeyParameters publicKey = PublicKey(serializedPublic);

            var rsa = new RSACryptoServiceProvider(512);
            var param = DotNetUtilities.ToRSAParameters(privateKey);
            rsa.ImportParameters(param);

            var signData = rsa.SignData(Encoding.GetBytes(message), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return signData;
        }

        public static bool Verify(AsymmetricCipherKeyPair pair, string message, byte[] signature)
        {
            var serializedPrivate = SerializedPrivate(pair);

            var serializedPublic = SerializedPublic(pair);

            RsaPrivateCrtKeyParameters privateKey = PrivateKey(serializedPrivate);
            RsaKeyParameters publicKey = PublicKey(serializedPublic);

            var rsa = new RSACryptoServiceProvider(512);
            var param = DotNetUtilities.ToRSAParameters(publicKey);
            rsa.ImportParameters(param);

            return rsa.VerifyData(Encoding.GetBytes(message), signature, HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);

        }

        private static RsaKeyParameters PublicKey(string serializedPublic)
        {
            return (RsaKeyParameters) PublicKeyFactory.CreateKey(Convert.FromBase64String(serializedPublic));
        }

        private static RsaPrivateCrtKeyParameters PrivateKey(string serializedPrivate)
        {
            return (RsaPrivateCrtKeyParameters) PrivateKeyFactory.CreateKey(Convert.FromBase64String(serializedPrivate));
        }

        private static string SerializedPublic(AsymmetricCipherKeyPair pair)
        {
            var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pair.Public);
            var serializedPublicBytes = publicKeyInfo.ToAsn1Object().GetDerEncoded();
            var serializedPublic = Convert.ToBase64String(serializedPublicBytes);
            return serializedPublic;
        }

        private static string SerializedPrivate(AsymmetricCipherKeyPair pair)
        {
            var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(pair.Private);
            var serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            var serializedPrivate = Convert.ToBase64String(serializedPrivateBytes);
            return serializedPrivate;
        }
    }


    public class EncryptTest
    {
        [Fact]
        public void should_sign_and_verify_a_message()
        {
            const string message = "message to sign";

            var asymmetricCipherKeyPair = Helper.Generate();

            var bytes = Helper.Sign(asymmetricCipherKeyPair, message);
            var signed = Convert.ToBase64String(bytes);

            var verification = Helper.Verify(asymmetricCipherKeyPair, message, bytes);

            verification.Should().Be(true);
        }
    }
}