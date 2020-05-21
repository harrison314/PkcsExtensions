using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtensions.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Tests.Algorithms
{
    [TestClass]
    public class DigestRandomGeneratorTests
    {
        [TestMethod]
        public void AddSeedMaterial()
        {
            using DigestRandomGenerator generator = new DigestRandomGenerator(HashAlgorithmName.SHA1);
            generator.AddSeedMaterial(new byte[] { 1, 4, 7, 8, 5 });
        }

        [TestMethod]
        public void NextBytes()
        {
            byte[] buffer = new byte[45];
            using DigestRandomGenerator generator = new DigestRandomGenerator(HashAlgorithmName.SHA1);
            generator.NextBytes(buffer);
        }

        [TestMethod]
        public void NextBytesWithLenght()
        {
            byte[] buffer = new byte[45];
            using DigestRandomGenerator generator = new DigestRandomGenerator(HashAlgorithmName.SHA1);
            generator.NextBytes(buffer, 1, 20);
        }

        [TestMethod]
        public void NextBytesSpan()
        {
            Span<byte> buffer = stackalloc byte[20];
            using DigestRandomGenerator generator = new DigestRandomGenerator(HashAlgorithmName.SHA1);
            generator.NextBytes(buffer);
        }

        [TestMethod]
        public void ChiSquared()
        {
            using DigestRandomGenerator generator = new DigestRandomGenerator(HashAlgorithmName.SHA1);
            generator.GenerateSeed();
            RandomTester.RunChiSquaredTests(generator);
        }

        [TestMethod]
        public void CtrWithHashAlgorithm()
        {
            Span<byte> buffer = stackalloc byte[20];
            using SHA256 sha256 = SHA256.Create();
            using DigestRandomGenerator generator = new DigestRandomGenerator(sha256);
            generator.NextBytes(buffer);
        }
    }
}
