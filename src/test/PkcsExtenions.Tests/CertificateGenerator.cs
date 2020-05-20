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
        public static X509Certificate2 Create(string cn, X509KeyUsageFlags? usageFlags = null, string extraKeyUsageOids = null, X509Certificate2 signedCertificate = null)
        {
            using RSA rsaKeys = RSA.Create(2048);

            CertificateRequest request = new CertificateRequest($"CN={cn}; C=SK", rsaKeys, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            if (usageFlags.HasValue)
            {
                request.CertificateExtensions.Add(new X509KeyUsageExtension(usageFlags.Value, false));
                if (usageFlags.Value.HasFlag(X509KeyUsageFlags.KeyCertSign))
                {
                    request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 1, false));
                }
            }

            if (!string.IsNullOrEmpty(extraKeyUsageOids))
            {
                OidCollection oidCollection = new OidCollection();
                foreach (string oid in extraKeyUsageOids.Split(new char[] { ';', ',' }))
                {
                    oidCollection.Add(new Oid(oid.Trim()));
                }
                request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(oidCollection, false));
            }

            if (signedCertificate == null)
            {
                return request.CreateSelfSigned(DateTime.Now.AddDays(-1.0), DateTime.Now.AddDays(1.0));
            }
            else
            {
                byte[] serial = new byte[] { 1 };
                // BitConverter.TryWriteBytes(serial.AsSpan(6), DateTime.UtcNow.Ticks);
                return request.Create(signedCertificate, DateTime.Now.AddDays(-0.5), DateTime.Now.AddDays(0.5), serial);
            }
        }
    }
}
