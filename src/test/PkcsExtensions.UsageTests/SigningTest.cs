using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;
using PkcsExtensions.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Security.Cryptography;
using System;
using PkcsExtensions.Pkcs7;

namespace PkcsExtensions.UsageTests
{
    [TestClass]
    public class SigningTest
    {
        [TestMethod]
        public void SignDocument()
        {
            using X509Certificate2 signingCertificate = CertificateGenerator.Create("Signer", X509KeyUsageFlags.NonRepudiation | X509KeyUsageFlags.DigitalSignature);

            Assert.IsTrue(signingCertificate.IsForDocumentSigning());

            byte[] data = Encoding.UTF8.GetBytes("Hello World!");

            ContentInfo content = new ContentInfo(data);
            SignedCms signedCms = new SignedCms(content, false);

            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, signingCertificate);
            signer.DigestAlgorithm = new Oid(Oids.SHA256);
            signer.IncludeOption = X509IncludeOption.WholeChain;
            signer.SignedAttributes.Add(new AsnEncodedData(new Pkcs9SigningTime(DateTime.Now)));
            signer.SignedAttributes.Add(new Pkcs7IdAaContentHint("helloWorld.txt", "text/plain"));
            signer.SignedAttributes.Add(new Pkcs7IdAaSigningCertificateV2(signingCertificate));
            signer.SignedAttributes.Add(new Pkcs9IdSigningPolicy("1.3.158.36061701.1.2.2", HashAlgorithmName.SHA256, "1A5A86D067512E00DB45FCD8DFB9A0574749D1D1F2A7189ED9F2DFE6ADE82DBD"));

            signedCms.ComputeSignature(signer);
            byte[] eidasP7mFileBytes = signedCms.Encode();

            Assert.IsNotNull(eidasP7mFileBytes);
        }
    }
}
