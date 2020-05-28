using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtensions.Algorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Tests.Algorithms
{
    [TestClass]
    public class HashAlgorithmExtensionsTests
    {
        [TestMethod]
        public void UpdateWithArray()
        {
            using SHA1 sha1 = SHA1.Create();

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

            sha1.Update(data1);
            sha1.Update(data2);
            sha1.Update(new byte[0]);
            sha1.Update(data3);

            byte[] hash = sha1.DoFinal();

            CollectionAssert.AreEquivalent(exceptedHash, hash, "Error in cputed hash.");
        }

        [TestMethod]
        public void UpdateWithSubArray()
        {
            using SHA1 sha1 = SHA1.Create();

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

            sha1.Update(data1, 0, 2);
            sha1.Update(data2, 4, 20);
            sha1.Update(new byte[5], 2, 0);
            sha1.Update(data3, 13, 120);

            byte[] hash = sha1.DoFinal();

            CollectionAssert.AreEquivalent(exceptedHash, hash, "Error in cputed hash.");
        }

        [TestMethod]
        public void DoFinal()
        {
            using SHA1 sha1 = SHA1.Create();

            byte[] data3 = new byte[147];

            Random r = new Random(42);
            r.NextBytes(data3);

            using SHA1 alternative = SHA1.Create();

            byte[] exceptedHash = alternative.ComputeHash(data3);

            sha1.Update(data3);

            byte[] hash = sha1.DoFinal();

            CollectionAssert.AreEquivalent(exceptedHash, hash, "Error in cputed hash.");
        }

        [TestMethod]
        public void TryDoFinal()
        {
            using SHA1 sha1 = SHA1.Create();

            byte[] data3 = new byte[147];

            Random r = new Random(42);
            r.NextBytes(data3);

            using SHA1 alternative = SHA1.Create();

            byte[] exceptedHash = alternative.ComputeHash(data3);

            sha1.Update(data3);

            byte[] hash = new byte[80];
            Assert.IsFalse(sha1.TryDoFinal(new Span<byte>(hash, 0, 5), out _));
            Assert.IsTrue(sha1.TryDoFinal(new Span<byte>(hash, 0, 78), out int witeBytes));

            byte[] subArray = hash.Take(witeBytes).ToArray();
            CollectionAssert.AreEquivalent(exceptedHash, subArray, "Error in cputed hash.");
            Assert.AreEqual(20, witeBytes);
        }
    }
}
