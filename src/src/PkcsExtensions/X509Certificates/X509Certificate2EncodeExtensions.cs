using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.X509Certificates
{
    public static class X509Certificate2EncodeExtensions
    {
        public static byte[] GetEncoded(this X509Certificate2 certificate, AsnFormat format)
        {
            return format switch
            {
                AsnFormat.Der => certificate.RawData,
                AsnFormat.Pem => PemFormater.ToPemBytes(certificate.RawData, "CERTIFICATE"),
                _ => ThrowHelpers.NotImplemented<byte[]>(nameof(X509Certificate2EncodeExtensions))
            };
        }
    }
}
