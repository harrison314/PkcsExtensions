using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.X509Certificates
{
    public static class X509Certificate2Extensions
    {
        private const string idKpClientAuth = "1.3.6.1.5.5.7.3.2";
        private const string idKpCodeSigning = "1.3.6.1.5.5.7.3.3";
        private const string idKpEmailProtection = "1.3.6.1.5.5.7.3.4";

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static bool IsForUsage(this X509Certificate2 certificate, X509KeyUsageFlags usageFlag)
        {
            ThrowHelpers.CheckNull(nameof(certificate), certificate);

            foreach (X509Extension certificateExtension in certificate.Extensions)
            {
                if (certificateExtension is X509KeyUsageExtension usage)
                {
                    if (usage.KeyUsages.HasFlag(usageFlag))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static bool IsForDocumentSigning(this X509Certificate2 certificate)
        {
            return IsForUsage(certificate, X509KeyUsageFlags.NonRepudiation);
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static bool IsForDigitalSigning(this X509Certificate2 certificate)
        {
            return IsForUsage(certificate, X509KeyUsageFlags.DigitalSignature);
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static bool IsForEncryption(this X509Certificate2 certificate)
        {
            return IsForUsage(certificate, X509KeyUsageFlags.KeyEncipherment) || IsForUsage(certificate, X509KeyUsageFlags.DataEncipherment);
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static bool IsForAuthentification(this X509Certificate2 certificate)
        {
            return IsForExtendedKeyUsage(certificate, idKpClientAuth);
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static bool IsForCodeSigning(this X509Certificate2 certificate)
        {
            return IsForExtendedKeyUsage(certificate, idKpCodeSigning)
                && IsForUsage(certificate, X509KeyUsageFlags.DigitalSignature);
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static bool IsForEmailProtection(this X509Certificate2 certificate)
        {
            return IsForExtendedKeyUsage(certificate, idKpEmailProtection);
        }

        private static bool IsForExtendedKeyUsage(this X509Certificate2 certificate, string exceptedUsageOid)
        {
            ThrowHelpers.CheckNull(nameof(certificate), certificate);

            foreach (X509Extension certificateExtension in certificate.Extensions)
            {
                if (certificateExtension is X509EnhancedKeyUsageExtension usage)
                {
                    foreach (System.Security.Cryptography.Oid usageOid in usage.EnhancedKeyUsages)
                    {
                        if (string.Equals(usageOid.Value, exceptedUsageOid, StringComparison.Ordinal))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
