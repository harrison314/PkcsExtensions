using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions
{
    public static class ECDsaExtensions
    {
#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static byte[] ExportSubjectPublicKeyInfo(this ECDsa ecdsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => ecdsa.ExportSubjectPublicKeyInfo(),
                AsnFormat.Pem => PemFormater.ToPemBytes(ecdsa.ExportSubjectPublicKeyInfo(), "PUBLIC KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(ECDsaExtensions))
            };
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static byte[] ExportECPrivateKey(this ECDsa ecdsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => ecdsa.ExportECPrivateKey(),
                AsnFormat.Pem => PemFormater.ToPemBytes(ecdsa.ExportECPrivateKey(), "EC PRIVATE KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(ECDsaExtensions))
            };
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static byte[] ExportPkcs8PrivateKey(this ECDsa ecdsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => ecdsa.ExportPkcs8PrivateKey(),
                AsnFormat.Pem => PemFormater.ToPemBytes(ecdsa.ExportPkcs8PrivateKey(), "PRIVATE KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(ECDsaExtensions))
            };
        }
    }
}
