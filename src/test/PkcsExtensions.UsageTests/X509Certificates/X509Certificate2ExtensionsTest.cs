using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PkcsExtensions.X509Certificates;

namespace PkcsExtensions.UsageTests.X509Certificates
{
    [TestClass]
    public class X509Certificate2ExtensionsTest
    {
        [TestMethod]
        public void Example_Usages()
        {
            X509Certificate2 certificate = ExampleCertificate.Instance;

            StringBuilder sb = new StringBuilder();
            sb.Append("This certificate is for: ");

            if (certificate.IsForAuthentification())
            {
                sb.Append("Authentification, ");
            }

            if (certificate.IsForCodeSigning())
            {
                sb.Append("Code Signing, ");
            }

            if (certificate.IsForDigitalSigning())
            {
                sb.Append("Digital Signing, ");
            }

            if (certificate.IsForDocumentSigning())
            {
                sb.Append("Document Signing, ");
            }

            if (certificate.IsForEmailProtection())
            {
                sb.Append("Email Protection, ");
            }

            if (certificate.IsForEncryption())
            {
                sb.Append("Encryption, ");
            }

            Assert.IsNotNull(sb.ToString());
        }
    }
}
