using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions
{
    public static class RSAExtensions
    {
#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static byte[] ExportSubjectPublicKeyInfo(this RSA rsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => rsa.ExportSubjectPublicKeyInfo(),
                AsnFormat.Pem => PemFormater.ToPemBytes(rsa.ExportSubjectPublicKeyInfo(), "PUBLIC KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(RSAExtensions))
            };
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static byte[] ExportRSAPublicKey(this RSA rsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => rsa.ExportRSAPublicKey(),
                AsnFormat.Pem => PemFormater.ToPemBytes(rsa.ExportRSAPublicKey(), "RSA PUBLIC KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(RSAExtensions))
            };
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static byte[] ExportRSAPrivateKey(this RSA rsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => rsa.ExportRSAPrivateKey(),
                AsnFormat.Pem => PemFormater.ToPemBytes(rsa.ExportRSAPrivateKey(), "RSA PRIVATE KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(RSAExtensions))
            };
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static byte[] ExportPkcs8PrivateKey(this RSA rsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => rsa.ExportPkcs8PrivateKey(),
                AsnFormat.Pem => PemFormater.ToPemBytes(rsa.ExportPkcs8PrivateKey(), "PRIVATE KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(RSAExtensions))
            };
        }
    }
}
