using PkcsExtenions.ASN1;
using PkcsExtenions.Blazor.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Blazor.Pkcs8
{
    internal static class Pkcs8RsaPrivateKey
    {
        public static RSAParameters PrivateKeyToRasaParams(ReadOnlyMemory<byte> data)
        {
            RSAParameters rsaParams = new RSAParameters();

            AsnReader octedReader = new AsnReader(data, AsnEncodingRules.DER);
            AsnReader sequenceReader = octedReader.ReadSequence();
            sequenceReader.GetIntegerBytes().ToArray();
            rsaParams.Modulus = ToPositiveInteger(sequenceReader.GetIntegerBytes().Span);
            rsaParams.Exponent = ToPositiveInteger(sequenceReader.GetIntegerBytes().Span);
            rsaParams.D = ToPositiveInteger(sequenceReader.GetIntegerBytes().Span);
            rsaParams.P = ToPositiveInteger(sequenceReader.GetIntegerBytes().Span);
            rsaParams.Q = ToPositiveInteger(sequenceReader.GetIntegerBytes().Span);
            rsaParams.DP = ToPositiveInteger(sequenceReader.GetIntegerBytes().Span);
            rsaParams.DQ = ToPositiveInteger(sequenceReader.GetIntegerBytes().Span);
            rsaParams.InverseQ = ToPositiveInteger(sequenceReader.GetIntegerBytes().Span);

            return rsaParams;
        }

        public static void ManagedImportPkcs8PrivateKey(this RSA rsa, byte[] privateKeyInfoBytes)
        {
            AsnReader reader = new AsnReader(privateKeyInfoBytes, AsnEncodingRules.DER);
            PrivateKeyInfo privateKeyInfo = new PrivateKeyInfo(reader);

            RSAParameters parameters = PrivateKeyToRasaParams(privateKeyInfo.PrivateKey);
            rsa.ImportParameters(parameters);
        }

        private static byte[] ToPositiveInteger(ReadOnlySpan<byte> data)
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

            return data.Slice(start).ToArray();
        }
    }
}
