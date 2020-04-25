using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Tests
{
    internal static class CertificateGenerator
    {
        public static X509Certificate2 Create(string cn)
        {
            using RSA rsaKeys = RSA.Create(2048);

            CertificateRequest request = new CertificateRequest($"CN={cn}; C=SK", rsaKeys, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return request.CreateSelfSigned(DateTime.Now.AddDays(-1.0), DateTime.Now.AddDays(1.0));
        }
    }
}
