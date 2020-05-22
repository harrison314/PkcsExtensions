using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PkcsExtensions.Algorithms;

namespace PkcsExtensions.UsageTests.Algorithms
{
    [TestClass]
    public class HashAlgorithmExtensionsTests
    {
        [TestMethod]
        public void Example_ComputeHash()
        {
            byte[] first = Encoding.ASCII.GetBytes("Hello ");
            byte[] second = Encoding.ASCII.GetBytes("world!");

            using SHA256 sha = SHA256.Create();
            sha.Update(first);
            sha.Update(second);

            byte[] hash = sha.DoFinal();
            string result = $"SHA256 of 'Hello world!' is {HexConvertor.GetString(hash)}";
            Assert.IsNotNull(result);
        }
    }
}
