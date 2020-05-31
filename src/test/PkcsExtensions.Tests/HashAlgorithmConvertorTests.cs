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
