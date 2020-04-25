using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PkcsExtenions.X509Certificates;

namespace PkcsExtenions.Tests.X509Certificate
{
    [TestClass]
    public class X509Certificate2ExtensionsTests
    {
        [DataTestMethod]
        [DataRow(X509KeyUsageFlags.NonRepudiation, true)]
        [DataRow(X509KeyUsageFlags.NonRepudiation | X509KeyUsageFlags.DigitalSignature, true)]
        [DataRow(X509KeyUsageFlags.DigitalSignature, false)]
        public void IsForDocumentSigning(X509KeyUsageFlags flags, bool exceptedResult)
        {
            X509Certificate2 certificate = CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags);
            bool result = certificate.IsForDocumentSigning();

            Assert.AreEqual(exceptedResult, result);
        }

        [DataTestMethod]
        [DataRow(X509KeyUsageFlags.DigitalSignature, true)]
        [DataRow(X509KeyUsageFlags.NonRepudiation | X509KeyUsageFlags.DigitalSignature, true)]
        [DataRow(X509KeyUsageFlags.CrlSign, false)]
        public void IsForDigitalSigning(X509KeyUsageFlags flags, bool exceptedResult)
        {
            X509Certificate2 certificate = CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags);
            bool result = certificate.IsForDigitalSigning();

            Assert.AreEqual(exceptedResult, result);
        }

        [DataTestMethod]
        [DataRow(X509KeyUsageFlags.DataEncipherment, true)]
        [DataRow(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.DigitalSignature, true)]
        [DataRow(X509KeyUsageFlags.CrlSign, false)]
        public void IsForEncryption(X509KeyUsageFlags flags, bool exceptedResult)
        {
            X509Certificate2 certificate = CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags);
            bool result = certificate.IsForEncryption();

            Assert.AreEqual(exceptedResult, result);
        }

        [DataTestMethod]
        [DataRow(X509KeyUsageFlags.DataEncipherment, true)]
        [DataRow(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.DigitalSignature, true)]
        [DataRow(X509KeyUsageFlags.CrlSign, false)]
        public void IsForAuthentification(X509KeyUsageFlags flags, bool exceptedResult)
        {
            X509Certificate2 certificate = (exceptedResult) ?
                CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags, "1.3.6.1.5.5.7.3.2") :
                CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags);

            bool result = certificate.IsForEncryption();

            Assert.AreEqual(exceptedResult, result);
        }

        [DataTestMethod]
        [DataRow(X509KeyUsageFlags.DataEncipherment, false)]
        [DataRow( X509KeyUsageFlags.DigitalSignature, true)]
        [DataRow( X509KeyUsageFlags.DigitalSignature, false)]
        [DataRow(X509KeyUsageFlags.CrlSign, false)]
        public void IsForCodeSigning(X509KeyUsageFlags flags, bool exceptedResult)
        {
            X509Certificate2 certificate = (exceptedResult) ?
                CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags, "1.3.6.1.5.5.7.3.3") :
                CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags);

            bool result = certificate.IsForCodeSigning();

            Assert.AreEqual(exceptedResult, result);
        }

        [DataTestMethod]
        [DataRow(X509KeyUsageFlags.DigitalSignature, true)]
        [DataRow(X509KeyUsageFlags.CrlSign, false)]
        public void IsForEmailProtection(X509KeyUsageFlags flags, bool exceptedResult)
        {
            X509Certificate2 certificate = (exceptedResult) ?
                CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags, "1.3.6.1.5.5.7.3.4") :
                CertificateGenerator.Create("X509Certificate2ExtensionsTests", flags);

            bool result = certificate.IsForEmailProtection();

            Assert.AreEqual(exceptedResult, result);
        }
    }
}
