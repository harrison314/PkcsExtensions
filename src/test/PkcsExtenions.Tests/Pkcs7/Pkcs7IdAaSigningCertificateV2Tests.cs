using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtenions.Pkcs7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Tests.Pkcs7
{
    [TestClass]
    public class Pkcs7IdAaSigningCertificateV2Tests
    {
        private readonly X509Certificate2 certficate1;
        private readonly X509Certificate2 certficate2;

        public Pkcs7IdAaSigningCertificateV2Tests()
        {
            this.certficate1 = CertificateGenerator.Create("texter1");
            this.certficate2 = CertificateGenerator.Create("texter2");
        }

        [TestMethod]
        public void CreateFromCertificate()
        {
            new Pkcs7IdAaSigningCertificateV2(this.certficate1);
        }

        [TestMethod]
        public void CreateFromCertificates()
        {
            X509Certificate2[] certificates = new X509Certificate2[]
            {
                this.certficate1,
                this.certficate2
            };

            new Pkcs7IdAaSigningCertificateV2(certificates);
        }

        [TestMethod]
        public void CreateFromCertificateAndHashAlgorithm()
        {
            new Pkcs7IdAaSigningCertificateV2(this.certficate1, HashAlgorithmName.SHA1);
        }

        [TestMethod]
        public void CreateFromCertificatesAndHashAlgorithm()
        {
            X509Certificate2[] certificates = new X509Certificate2[]
            {
                this.certficate1,
                this.certficate2
            };

            new Pkcs7IdAaSigningCertificateV2(certificates, HashAlgorithmName.SHA1);
        }
    }
}
