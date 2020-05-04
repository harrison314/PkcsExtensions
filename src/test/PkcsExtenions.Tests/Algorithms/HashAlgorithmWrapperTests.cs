using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtenions.Algorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Tests.Algorithms
{
    [TestClass]
    public class HashAlgorithmWrapperTests
    {
        [TestMethod]
        public void Clear()
        {
            using SHA1 sha1 = SHA1.Create();
            HashAlgorithmWrapper hashAlgorithmWrapper = new HashAlgorithmWrapper(sha1);

            hashAlgorithmWrapper.Clear();
        }

        [TestMethod]
        public void UpdateWithArray()
        {
            using SHA1 sha1 = SHA1.Create();
            HashAlgorithmWrapper hashAlgorithmWrapper = new HashAlgorithmWrapper(sha1);

            byte[] data1 = new byte[2];
            byte[] data2 = new byte[20];
            byte[] data3 = new byte[147];

            Random r = new Random(42);
            r.NextBytes(data1);
            r.NextBytes(data2);
            r.NextBytes(data3);

            using MemoryStream ms = new MemoryStream();
            ms.Write(data1);
            ms.Write(data2);
            ms.Write(data3);
            ms.Position = 0L;
            using SHA1 alternative = SHA1.Create();

            byte[] exceptedHash = alternative.ComputeHash(ms);

            hashAlgorithmWrapper.Update(data1);
            hashAlgorithmWrapper.Update(data2);
            hashAlgorithmWrapper.Update(data3);

            byte[] hash = hashAlgorithmWrapper.DoFinal();

            CollectionAssert.AreEquivalent(exceptedHash, hash, "Error in cputed hash.");
        }

        [TestMethod]
        public void UpdateWithSubArray()
        {
            using SHA1 sha1 = SHA1.Create();
            HashAlgorithmWrapper hashAlgorithmWrapper = new HashAlgorithmWrapper(sha1);

            byte[] data1 = new byte[2];
            byte[] data2 = new byte[24];
            byte[] data3 = new byte[147];

            Random r = new Random(42);
            r.NextBytes(data1);
            r.NextBytes(data2);
            r.NextBytes(data3);

            using MemoryStream ms = new MemoryStream();
            ms.Write(data1, 0, 2);
            ms.Write(data2, 4, 20);
            ms.Write(data3, 13, 120);
            ms.Position = 0L;
            using SHA1 alternative = SHA1.Create();

            byte[] exceptedHash = alternative.ComputeHash(ms);

            hashAlgorithmWrapper.Update(data1, 0, 2);
            hashAlgorithmWrapper.Update(data2, 4, 20);
            hashAlgorithmWrapper.Update(data3, 13, 120);

            byte[] hash = hashAlgorithmWrapper.DoFinal();

            CollectionAssert.AreEquivalent(exceptedHash, hash, "Error in cputed hash.");
        }

        [TestMethod]
        public void DoFinal()
        {
            using SHA1 sha1 = SHA1.Create();
            HashAlgorithmWrapper hashAlgorithmWrapper = new HashAlgorithmWrapper(sha1);

            byte[] data3 = new byte[147];

            Random r = new Random(42);
            r.NextBytes(data3);
    
            using SHA1 alternative = SHA1.Create();

            byte[] exceptedHash = alternative.ComputeHash(data3);

            hashAlgorithmWrapper.Update(data3);

            byte[] hash = hashAlgorithmWrapper.DoFinal();

            CollectionAssert.AreEquivalent(exceptedHash, hash, "Error in cputed hash.");
        }

        [TestMethod]
        public void TryDoFinal()
        {
            using SHA1 sha1 = SHA1.Create();
            HashAlgorithmWrapper hashAlgorithmWrapper = new HashAlgorithmWrapper(sha1);

            byte[] data3 = new byte[147];

            Random r = new Random(42);
            r.NextBytes(data3);

            using SHA1 alternative = SHA1.Create();

            byte[] exceptedHash = alternative.ComputeHash(data3);

            hashAlgorithmWrapper.Update(data3);

            byte[] hash = new byte[80];
            Assert.IsFalse(hashAlgorithmWrapper.TryDoFinal(new Span<byte>(hash, 0, 5), out _));
            Assert.IsTrue(hashAlgorithmWrapper.TryDoFinal(new Span<byte>(hash, 0, 78), out int witeBytes));

            byte[] subArray = hash.Take(witeBytes).ToArray();
            CollectionAssert.AreEquivalent(exceptedHash, subArray, "Error in cputed hash.");
            Assert.AreEqual(20, witeBytes);
        }
    }
}
