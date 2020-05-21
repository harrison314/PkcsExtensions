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
        public static byte[] ExportSubjectPublicKeyInfo(this ECDsa rsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => rsa.ExportSubjectPublicKeyInfo(),
                AsnFormat.Pem => PemFormater.ToPemBytes(rsa.ExportSubjectPublicKeyInfo(), "PUBLIC KEY"),
                _ => throw new NotImplementedException()
            };
        }

        public static byte[] ExportRSAPrivateKey(this ECDsa rsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => rsa.ExportECPrivateKey(),
                AsnFormat.Pem => PemFormater.ToPemBytes(rsa.ExportECPrivateKey(), "EC PRIVATE KEY"),
                _ => throw new NotImplementedException()
            };
        }

        public static byte[] ExportPkcs8PrivateKey(this ECDsa rsa, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => rsa.ExportPkcs8PrivateKey(),
                AsnFormat.Pem => PemFormater.ToPemBytes(rsa.ExportPkcs8PrivateKey(), "PRIVATE KEY"),
                _ => throw new NotImplementedException()
            };
        }
    }
}
