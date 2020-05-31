using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using PkcsExtensions.Pkcs1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Tests.Pkcs1
{
    [TestClass]
    public class Pkcs1DigestInfoTests
    {
        [TestMethod]
        public void Encode_SHA1()
        {
            byte[] digestValue = new byte[20];
            byte[] result = Pkcs1DigestInfo.Encode(HashAlgorithmName.SHA1, digestValue);

            CheckAsn1(result);
        }

        [TestMethod]
        public void Encode_SHA256()
        {
            byte[] digestValue = new byte[32];
            byte[] result = Pkcs1DigestInfo.Encode(HashAlgorithmName.SHA256, digestValue);

            CheckAsn1(result);
        }


        [TestMethod]
        public void Encode_SHA384()
        {
            byte[] digestValue = new byte[48];
            byte[] result = Pkcs1DigestInfo.Encode(HashAlgorithmName.SHA384, digestValue);

            CheckAsn1(result);
        }

        [TestMethod]
        public void Encode_SHA512()
        {
            byte[] digestValue = new byte[64];
            byte[] result = Pkcs1DigestInfo.Encode(HashAlgorithmName.SHA512, digestValue);

            CheckAsn1(result);
        }

        [TestMethod]
        public void TtryEncode_SHA256()
        {
            byte[] digestValue = new byte[32];
            byte[] result = new byte[1024];
            bool success = Pkcs1DigestInfo.TryEncode(HashAlgorithmName.SHA256, digestValue, result, out int len);

            Assert.IsTrue(success);

            CheckAsn1(result.AsSpan(0, len).ToArray());
        }

        [TestMethod]
        public void TtryEncode_SHA512()
        {
            byte[] digestValue = new byte[32];
            byte[] result = new byte[1024];
            bool success = Pkcs1DigestInfo.TryEncode(HashAlgorithmName.SHA256, digestValue, result, out int len);

            Assert.IsTrue(success);

            CheckAsn1(result.AsSpan(0, len).ToArray());
        }

        private static void CheckAsn1(byte[] result)
        {
            Assert.IsNotNull(result);

            DigestInfo digestInfo = DigestInfo.GetInstance(Asn1Object.FromByteArray(result));
            Assert.IsNotNull(digestInfo.AlgorithmID.Algorithm);
            Assert.IsNotNull(digestInfo.GetDigest());
        }
    }
}
