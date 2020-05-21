using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.UsageTests
{
    internal class CertificateGenerator
    {
        public static X509Certificate2 Create(string cn, X509KeyUsageFlags? usageFlags = null, string extraKeyUsageOids = null)
        {
            using ECDsa elipticKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);

            CertificateRequest request = new CertificateRequest($"CN={cn}; C=SK", elipticKey, HashAlgorithmName.SHA256);
            if (usageFlags.HasValue)
            {
                request.CertificateExtensions.Add(new X509KeyUsageExtension(usageFlags.Value, false));
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

            return request.CreateSelfSigned(DateTime.Now.AddDays(-1.0), DateTime.Now.AddDays(1.0));
        }
    }
}
