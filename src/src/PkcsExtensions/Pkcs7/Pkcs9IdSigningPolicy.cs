using PkcsExtensions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Pkcs7
{
    public class Pkcs9IdSigningPolicy : AsnEncodedData
    {
        public string PolicyOid
        {
            get;
            protected set;
        }

        public Pkcs9IdSigningPolicy(string policyOid, HashAlgorithmName algorithmNameForPolicy, ReadOnlySpan<byte> policyHashValue)
            : base(Pkcs7Oids.IdSigningPolicy, CreateRawAsn1(policyOid, algorithmNameForPolicy, policyHashValue))
        {
            this.PolicyOid = policyOid;
        }

        public Pkcs9IdSigningPolicy(string policyOid, HashAlgorithmName algorithmNameForPolicy, string policyHexHashValue)
            : base(Pkcs7Oids.IdSigningPolicy, CreateRawAsn1(policyOid, algorithmNameForPolicy, HexConvertor.GetBytes(policyHexHashValue)))
        {
            this.PolicyOid = policyOid;
        }

        public override void CopyFrom(AsnEncodedData asnEncodedData)
        {
            ThrowHelpers.NotImplemented(nameof(Pkcs7IdAaContentHint));
        }

        public override string Format(bool multiLine)
        {
            ThrowHelpers.NotImplemented(nameof(Pkcs7IdAaContentHint));
            return default;
        }

        private static byte[] CreateRawAsn1(string policyOid, HashAlgorithmName algorithmNameForPolicy, ReadOnlySpan<byte> policyHashValue)
        {
            if (policyOid == null) throw new ArgumentNullException(nameof(policyOid));

            using AsnWriter asnWriter = new AsnWriter(AsnEncodingRules.DER);
            asnWriter.PushSequence();
            asnWriter.WriteObjectIdentifier(policyOid);
            asnWriter.PushSequence();
            asnWriter.WriteObjectIdentifier(HashAlgorithmConvertor.ToOid(algorithmNameForPolicy));
            asnWriter.WriteOctetString(policyHashValue);
            asnWriter.PopSequence();
            asnWriter.PopSequence();
            return asnWriter.Encode();
        }
    }
}
