using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PkcsExtensions.X509Certificates;

namespace PkcsExtensions.Tests.X509Certificate
{
    [TestClass]
    public class X509Certificate2EncodeExtensionsTests
    {
        [DataTestMethod]
        [DataRow(AsnFormat.Der)]
        [DataRow(AsnFormat.Pem)]
        public void GetEncoded(AsnFormat format)
        {
            X509Certificate2 certificate = CertificateGenerator.Create("X509Certificate2ExtensionsTests");

            byte[] encoded = certificate.GetEncoded(format);

            X509Certificate2 result = new X509Certificate2(encoded);
            Assert.IsNotNull(result);
        }  
    }
}
