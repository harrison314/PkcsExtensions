using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtensions.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.UsageTests.Algorithms
{
    [TestClass]
    public class DigestRandomGeneratorTests
    {
        [TestMethod]
        public void Example_DigestRandomGenerator()
        {
            using DigestRandomGenerator randomGenerator = new DigestRandomGenerator(HashAlgorithmName.SHA256);
            randomGenerator.GenerateSeed(); // seed using current date

            int randomNnmber = randomGenerator.Next(0, 15);
            uint randomUint = randomGenerator.Next<uint>();

            randomGenerator.AddSeedMaterial(new byte[] { 45, 78, 12, 0, 0, 45 }); // additional seed material

            byte[] data = new byte[45];
            randomGenerator.NextBytes(data); // get random data
        }
    }
}
