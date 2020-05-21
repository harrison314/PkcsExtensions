using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtensions.Pkcs7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Tests.Pkcs7
{
    [TestClass]
    public class Pkcs9IdSigningPolicyTest
    {
        [TestMethod]
        public void Create()
        {
            new Pkcs9IdSigningPolicy("1.3.158.36061701.1.2.2", HashAlgorithmName.SHA256, "1A5A86D067512E00DB45FCD8DFB9A0574749D1D1F2A7189ED9F2DFE6ADE82DBD");
        }
    }
}
