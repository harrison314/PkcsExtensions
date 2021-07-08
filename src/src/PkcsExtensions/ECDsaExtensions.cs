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
        public static byte[] ExportSubjectPublicKeyInfo(this ECDsa ecdsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => ecdsa.ExportSubjectPublicKeyInfo(),
                AsnFormat.Pem => PemFormater.ToPemBytes(ecdsa.ExportSubjectPublicKeyInfo(), "PUBLIC KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(ECDsaExtensions))
            };
        }

        [Obsolete("Use ExportECPrivateKey.", true)]
        public static byte[] ExportRSAPrivateKey(this ECDsa ecdsa, AsnFormat format)
        {
            return ecdsa.ExportECPrivateKey(format);
        }

        public static byte[] ExportECPrivateKey(this ECDsa ecdsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => ecdsa.ExportECPrivateKey(),
                AsnFormat.Pem => PemFormater.ToPemBytes(ecdsa.ExportECPrivateKey(), "EC PRIVATE KEY"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(ECDsaExtensions))
            };
        }

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
