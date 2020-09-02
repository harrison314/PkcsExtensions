using PkcsExtensions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Pkcs1
{
    // https://tools.ietf.org/html/rfc5480
    public static class ECDsaSigValue
    {
        public static byte[] Encode(ReadOnlySpan<byte> rawPkcs11Signature)
        {
            if (rawPkcs11Signature.Length == 0 || rawPkcs11Signature.Length % 2 != 0)
            {
                ThrowHelpers.ThrowArgumentException($"Parameter {nameof(rawPkcs11Signature)} is empty or has not even length.");
            }

            using AsnWriter asnWriter = new AsnWriter(AsnEncodingRules.DER);
            ConstructECDsaSigValue(rawPkcs11Signature, asnWriter);

            return asnWriter.Encode();
        }

        public static bool TryEncode(ReadOnlySpan<byte> rawPkcs11Signature, Span<byte> destination, out int bytesWritten)
        {
            if (rawPkcs11Signature.Length == 0 || rawPkcs11Signature.Length % 2 != 0)
            {
                ThrowHelpers.ThrowArgumentException($"Parameter {nameof(rawPkcs11Signature)} is empty or has not even length.");
            }

            using AsnWriter asnWriter = new AsnWriter(AsnEncodingRules.DER);
            ConstructECDsaSigValue(rawPkcs11Signature, asnWriter);

            return asnWriter.TryEncode(destination, out bytesWritten);
        }

        private static void ConstructECDsaSigValue(ReadOnlySpan<byte> rawPkcs11Signature, AsnWriter asnWriter)
        {
            int halfIndex = rawPkcs11Signature.Length / 2;
            ReadOnlySpan<byte> rValue = ToPositiveInteger(rawPkcs11Signature.Slice(0, halfIndex));
            ReadOnlySpan<byte> sValue = ToPositiveInteger(rawPkcs11Signature.Slice(halfIndex));

            // ECDSA-Sig-Value
            asnWriter.PushSequence();

            // r
            asnWriter.WriteInteger(rValue);

            //s
            asnWriter.WriteInteger(sValue);

            asnWriter.PopSequence();
        }

        private static ReadOnlySpan<byte> ToPositiveInteger(ReadOnlySpan<byte> data)
        {
            int start = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    start++;
                }
                else
                {
                    break;
                }
            }

            return data.Slice(start);
        }
    }
}
