using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtenions.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Tests.Algorithms
{
    [TestClass]
    public class RngRandomGeneratorTests
    {
        [TestMethod]
        public void AddSeedMaterial()
        {
            using RngRandomGenerator generator = new RngRandomGenerator();
            generator.AddSeedMaterial(new byte[] { 1, 4, 7, 8, 5 });
        }

        [TestMethod]
        public void NextBytes()
        {
            byte[] buffer = new byte[45];
            using RngRandomGenerator generator = new RngRandomGenerator();
            generator.NextBytes(buffer);
        }

        [TestMethod]
        public void NextBytesWithLenght()
        {
            byte[] buffer = new byte[45];
            using RngRandomGenerator generator = new RngRandomGenerator();
            generator.NextBytes(buffer, 1, 20);
        }

        [TestMethod]
        public void NextBytesSpan()
        {
            Span<byte> buffer = stackalloc byte[20];
            using RngRandomGenerator generator = new RngRandomGenerator();
            generator.NextBytes(buffer);
        }

        [TestMethod]
        public void ChiSquared()
        {
            using RngRandomGenerator generator = new RngRandomGenerator();
            RandomTester.RunChiSquaredTests(generator);
        }
    }
}
