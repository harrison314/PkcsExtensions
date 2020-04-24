using System;
using System.Text;

namespace PkcsExtenions.ASN1
{
    internal static class AsnCharacterStringEncodings
    {
        private static readonly Encoding s_utf8Encoding = new UTF8Encoding(false, throwOnInvalidBytes: true);
        private static readonly Encoding s_bmpEncoding = new BMPEncoding();
        private static readonly Encoding s_ia5Encoding = new IA5Encoding();
        private static readonly Encoding s_visibleStringEncoding = new VisibleStringEncoding();
        private static readonly Encoding s_printableStringEncoding = new PrintableStringEncoding();
        private static readonly Encoding s_t61Encoding = new T61Encoding();

        internal static Encoding GetEncoding(UniversalTagNumber encodingType)
        {
            switch (encodingType)
            {
                case UniversalTagNumber.UTF8String:
                    return s_utf8Encoding;
                case UniversalTagNumber.PrintableString:
                    return s_printableStringEncoding;
                case UniversalTagNumber.IA5String:
                    return s_ia5Encoding;
                case UniversalTagNumber.VisibleString:
                    return s_visibleStringEncoding;
                case UniversalTagNumber.BMPString:
                    return s_bmpEncoding;
                case UniversalTagNumber.T61String:
                    return s_t61Encoding;
                default:
                    throw new ArgumentOutOfRangeException(nameof(encodingType), encodingType, null);
            }
        }
    }
}
