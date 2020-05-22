using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtensions.Pkcs1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.UsageTests.Pkcs1
{
    [TestClass]
    public class Pkcs1DigestInfoTests
    {
        [TestMethod]
        public void Example_Pkcs1DigestInfo()
        {
            byte[] dataToSign = Encoding.ASCII.GetBytes("Hello world");
            using SHA256 sha = SHA256.Create();
            byte[] hashToSign = sha.ComputeHash(dataToSign);

           byte[] digestInfo = Pkcs1DigestInfo.Encode(HashAlgorithmName.SHA256, hashToSign);

            SignUsingPkcs11(digestInfo);
        }

        private static void SignUsingPkcs11(byte[] digestInfo)
        {
            // Only for example
        }
    }
}
