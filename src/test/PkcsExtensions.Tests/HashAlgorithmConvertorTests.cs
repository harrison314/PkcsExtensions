using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Tests
{
    [TestClass]
    public class HashAlgorithmConvertorTests
    {
        [TestMethod]
        public void ToOid()
        {
            foreach (HashAlgorithmName name in this.GetNames())
            {
                string oid = HashAlgorithmConvertor.ToOid(name);

                Assert.IsNotNull(oid);
                Assert.AreNotEqual(string.Empty, oid);
            }
        }

        [TestMethod]
        public void ToHashAlgorithm()
        {
            foreach (HashAlgorithmName name in this.GetNames())
            {
                HashAlgorithm algorithm = HashAlgorithmConvertor.ToHashAlgorithm(name);

                Assert.IsNotNull(algorithm);
            }
        }

        [TestMethod]
        public void ToHashSizeInBytes()
        {
            foreach (HashAlgorithmName name in this.GetNames())
            {
                int size = HashAlgorithmConvertor.ToHashSizeInBytes(name);
                HashAlgorithm algorithm = HashAlgorithmConvertor.ToHashAlgorithm(name);

                Assert.AreEqual(algorithm.HashSize / 8, size);
            }
        }

        [TestMethod]
        public void TryFromOid()
        {
            Assert.IsTrue(HashAlgorithmConvertor.TryFromOid(Oids.SHA384, out HashAlgorithmName name1));
            Assert.AreEqual(HashAlgorithmName.SHA384, name1);

            Assert.IsTrue(HashAlgorithmConvertor.TryFromOid(Oids.SHA256, out HashAlgorithmName name2));
            Assert.AreEqual(HashAlgorithmName.SHA256, name2);

            Assert.IsFalse(HashAlgorithmConvertor.TryFromOid("1.4.74.12.1.4", out _));
        }

        private HashAlgorithmName[] GetNames()
        {
            return new HashAlgorithmName[]
            {
                HashAlgorithmName.SHA256,
                HashAlgorithmName.SHA384,
                HashAlgorithmName.SHA512,
                HashAlgorithmName.MD5,
                HashAlgorithmName.SHA1
            };
        }
    }
}
