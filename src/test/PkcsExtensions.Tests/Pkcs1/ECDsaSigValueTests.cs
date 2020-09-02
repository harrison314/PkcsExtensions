using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1;
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
    public class ECDsaSigValueTests
    {
        [TestMethod]
        public void Encode()
        {
            byte[] data = Encoding.UTF8.GetBytes("Hello World!");

            using ECDsa ec = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            byte[] signature = ec.SignData(data, HashAlgorithmName.SHA256);

            byte[] ecdsaSigValue = ECDsaSigValue.Encode(signature);
            Assert.IsNotNull(ecdsaSigValue);
            this.CheckSignValue(ecdsaSigValue);
        }

        [TestMethod]
        public void TryEncode()
        {
            byte[] data = Encoding.UTF8.GetBytes("Hello World!");

            using ECDsa ec = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            byte[] signature = ec.SignData(data, HashAlgorithmName.SHA256);

            byte[] ecdsaSigValue = new byte[250];

            bool result = ECDsaSigValue.TryEncode(signature, ecdsaSigValue, out int writeBytes);

            Assert.IsTrue(result);
            this.CheckSignValue(ecdsaSigValue.AsSpan(0, writeBytes).ToArray());
        }

        private void CheckSignValue(byte[] ecdsaSigValue)
        {
            Asn1Object asn1 = Asn1Object.FromByteArray(ecdsaSigValue);
            Assert.IsNotNull(asn1);
        }
    }
}
