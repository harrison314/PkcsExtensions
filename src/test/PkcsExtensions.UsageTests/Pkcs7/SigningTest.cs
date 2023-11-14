using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography.X509Certificates;
using PkcsExtensions.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Security.Cryptography;
using System;
using PkcsExtensions.Pkcs7;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PkcsExtensions.UsageTests.Pkcs7
{
    [TestClass]
    public class SigningTest
    {
        [TestMethod]
        public void Example_SignDocument()
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
            signer.SignedAttributes.Add(new Pkcs9IdSigningPolicy("1.3.158.36061701.1.2.2", HashAlgorithmName.SHA256, "1A5A86D067512E00DB45FCD8DFB9A0574749D1D1F2A7189ED9F2DFE6ADE82DBD")); // Only for old Slovak format - SK ZEP

            signedCms.ComputeSignature(signer, false);
            byte[] eidasP7mFileBytes = signedCms.Encode();

            Assert.IsNotNull(eidasP7mFileBytes);
        }

#if INTEGRATION_TESTS
        [TestMethod]
#endif
        public async Task Example_SignDocumentWithTs()
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
            // Without Pkcs9IdSigningPolicy

            signedCms.ComputeSignature(signer, false);

            await this.CreateTimeStamp(signedCms);

            byte[] eidasP7mFileBytes = signedCms.Encode();

            Assert.IsNotNull(eidasP7mFileBytes);
        }

        private async Task CreateTimeStamp(SignedCms signedCms)
        {
            // See: https://www.glennwatson.net/posts/rfc-3161-signing

            const string timeStampAuthorityUri = "http://time.certum.pl/";
            byte[] nonce = new byte[8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(nonce);
            }

            SignerInfo newSignerInfo = signedCms.SignerInfos[0];
            Rfc3161TimestampRequest? request = Rfc3161TimestampRequest.CreateFromSignerInfo(
                 newSignerInfo,
                 HashAlgorithmName.SHA256,
                 requestSignerCertificates: true,
                 nonce: nonce);

            using HttpClient client = new HttpClient();
            using ReadOnlyMemoryContent content = new ReadOnlyMemoryContent(request.Encode());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/timestamp-query");

            using HttpResponseMessage? httpResponse = await client.PostAsync(timeStampAuthorityUri, content).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new CryptographicException($"There was a error from the timestamp authority. It responded with {httpResponse.StatusCode} {(int)httpResponse.StatusCode}: {httpResponse.Content}");
            }

            if (httpResponse.Content.Headers.ContentType?.MediaType != "application/timestamp-reply")
            {
                throw new CryptographicException("The reply from the time stamp server was in a invalid format.");
            }

            byte[]? data = await httpResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            Rfc3161TimestampToken? timestampToken = request.ProcessResponse(data, out _);

            Oid signatureTimeStampOid = new Oid("1.2.840.113549.1.9.16.2.14");
            newSignerInfo.AddUnsignedAttribute(new AsnEncodedData(signatureTimeStampOid, timestampToken.AsSignedCms().Encode()));
        }
    }
}
