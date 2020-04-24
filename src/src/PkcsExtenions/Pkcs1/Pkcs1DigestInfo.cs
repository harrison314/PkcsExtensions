using PkcsExtenions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Pkcs1
{
    public static class Pkcs1DigestInfo
    {
        public static byte[] Encode(HashAlgorithmName hashAlgorithmName, ReadOnlySpan<byte> digest)
        {
            using AsnWriter asnWriter = new AsnWriter(AsnEncodingRules.DER);
            ConstructAsn1(hashAlgorithmName, digest, asnWriter);

            return asnWriter.Encode();
        }

        public static bool TryEncode(HashAlgorithmName hashAlgorithmName, ReadOnlySpan<byte> digest, Span<byte> destination, out int bytesWritten)
        {
            using AsnWriter asnWriter = new AsnWriter(AsnEncodingRules.DER);
            ConstructAsn1(hashAlgorithmName, digest, asnWriter);

            return asnWriter.TryEncode(destination, out bytesWritten);
        }

        private static void ConstructAsn1(HashAlgorithmName hashAlgorithmName, ReadOnlySpan<byte> digest, AsnWriter asnWriter)
        {
            // Digest info
            asnWriter.PushSequence();

            // AlgorithmIdetifier
            string oid = HashAlgorithmConvertor.ToOid(hashAlgorithmName);
            asnWriter.PushSequence();
            asnWriter.WriteObjectIdentifier(oid);
            asnWriter.WriteNull();
            asnWriter.PopSequence();

            asnWriter.WriteOctetString(digest);
            asnWriter.PopSequence();
        }
    }
}
