using PkcsExtensions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.X509Certificates
{
    public static class X509Certificate2NameInfoExtensions
    {
        public static IReadOnlyList<string> GetNameInfo(this X509Certificate2 certificate, string nameTypeOid, bool forIssuer)
        {
            ThrowHelpers.CheckNullOrEempty(nameof(nameTypeOid), nameTypeOid);
            return GetNameInfo(certificate, nameTypeOid, forIssuer ? X509NameSource.Issuer : X509NameSource.Subject);
        }

        public static IReadOnlyList<string> GetNameInfo(this X509Certificate2 certificate, string nameTypeOid, X509NameSource nameSource)
        {
            ThrowHelpers.CheckNullOrEempty(nameof(nameTypeOid), nameTypeOid);

            byte[] nameBytes = nameSource switch
            {
                X509NameSource.Issuer => certificate.IssuerName.RawData,
                X509NameSource.Subject => certificate.SubjectName.RawData,
                _ => ThrowHelpers.NotSupport<byte[], X509NameSource>(nameSource)
            };

            List<string> result = new List<string>();

            AsnReader nameReader = new AsnReader(nameBytes, AsnEncodingRules.DER);
            AsnReader mainSequence = nameReader.ReadSequence();
            while (mainSequence.HasData)
            {
                AsnReader x509Name = mainSequence.ReadSetOf().ReadSequence();
                string oid = x509Name.ReadObjectIdentifierAsString();
                if (string.Equals(nameTypeOid, oid, StringComparison.Ordinal))
                {
                    result.Add(x509Name.GetCharacterString(UniversalTagNumber.PrintableString));
                }
            }

            return result;
        }
    }
}
