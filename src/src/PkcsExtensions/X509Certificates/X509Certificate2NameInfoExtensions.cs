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
#if NET6_0_OR_GREATER
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static IReadOnlyList<string> GetNameInfo(this X509Certificate2 certificate, string nameTypeOid, bool forIssuer)
        {
            ThrowHelpers.CheckNullOrEempty(nameof(nameTypeOid), nameTypeOid);

            byte[] nameBytes = forIssuer ? certificate.IssuerName.RawData : certificate.SubjectName.RawData;
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

        public static IReadOnlyList<string> GetNameInfo(this X509Certificate2 certificate, string nameTypeOid, X509NameSource nameSource)
        {
            ThrowHelpers.CheckNullOrEempty(nameof(nameTypeOid), nameTypeOid);
            return GetNameInfo(certificate, nameTypeOid, nameSource == X509NameSource.Issuer);
        }

        public static IReadOnlyList<NameInfo> GetNameInfo(this X509Certificate2 certificate, bool forIssuer)
        {
            byte[] nameBytes = forIssuer ? certificate.IssuerName.RawData : certificate.SubjectName.RawData;
            Dictionary<string, List<string>> infos = new Dictionary<string, List<string>>(StringComparer.Ordinal);

            AsnReader nameReader = new AsnReader(nameBytes, AsnEncodingRules.DER);
            AsnReader mainSequence = nameReader.ReadSequence();
            while (mainSequence.HasData)
            {
                AsnReader x509Name = mainSequence.ReadSetOf().ReadSequence();
                string oid = x509Name.ReadObjectIdentifierAsString();
                if (infos.TryGetValue(oid, out List<string>? list))
                {
                    list.Add(x509Name.GetCharacterString(UniversalTagNumber.PrintableString));
                }
                else
                {
                    list = new List<string>();
                    list.Add(x509Name.GetCharacterString(UniversalTagNumber.PrintableString));
                    infos.Add(oid, list);
                }
            }

            return infos.Select(pair => new NameInfo(pair.Key, pair.Value)).ToList();
        }

        public static IReadOnlyList<NameInfo> GetNameInfo(this X509Certificate2 certificate, X509NameSource nameSource)
        {
            return GetNameInfo(certificate, nameSource == X509NameSource.Issuer);
        }
    }
}
