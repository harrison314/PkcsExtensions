using System.Text;

namespace PkcsExtenions.ASN1
{
    /// <summary>
    /// Compatibility encoding for T61Strings. Interprets the characters as UTF-8 or
    /// ISO-8859-1 as a fallback.
    /// <summary>
    internal class T61Encoding : Encoding
    {
        private static readonly Encoding s_utf8Encoding = new UTF8Encoding(false, throwOnInvalidBytes: true);
        private static readonly Encoding s_latin1Encoding = Encoding.GetEncoding("iso-8859-1");

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return s_utf8Encoding.GetByteCount(chars, index, count);
        }

        public override unsafe int GetByteCount(char* chars, int count)
        {
            return s_utf8Encoding.GetByteCount(chars, count);
        }

        public override int GetByteCount(string s)
        {
            return s_utf8Encoding.GetByteCount(s);
        }

#if netcoreapp || uap
        public override int GetByteCount(ReadOnlySpan<char> chars)
        {
            return s_utf8Encoding.GetByteCount(chars);
        }
#endif

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return s_utf8Encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
        {
            return s_utf8Encoding.GetBytes(chars, charCount, bytes, byteCount);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            try
            {
                return s_utf8Encoding.GetCharCount(bytes, index, count);
            }
            catch (DecoderFallbackException)
            {
                return s_latin1Encoding.GetCharCount(bytes, index, count);
            }
        }

        public override unsafe int GetCharCount(byte* bytes, int count)
        {
            try
            {
                return s_utf8Encoding.GetCharCount(bytes, count);
            }
            catch (DecoderFallbackException)
            {
                return s_latin1Encoding.GetCharCount(bytes, count);
            }
        }

#if netcoreapp || uap
        public override int GetCharCount(ReadOnlySpan<byte> bytes)
        {
            try
            {
                return s_utf8Encoding.GetCharCount(bytes);
            }
            catch (DecoderFallbackException)
            {
                return s_latin1Encoding.GetCharCount(bytes);
            }
        }
#endif

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            try
            {
                return s_utf8Encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
            }
            catch (DecoderFallbackException)
            {
                return s_latin1Encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
            }
        }

        public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
        {
            try
            {
                return s_utf8Encoding.GetChars(bytes, byteCount, chars, charCount);
            }
            catch (DecoderFallbackException)
            {
                return s_latin1Encoding.GetChars(bytes, byteCount, chars, charCount);
            }
        }

        public override int GetMaxByteCount(int charCount)
        {
            return s_utf8Encoding.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount)
        {
            // Latin-1 is single byte encoding, so byteCount == charCount
            // UTF-8 is multi-byte encoding, so byteCount >= charCount
            // We want to return the maximum of those two, which happens to be byteCount.
            return byteCount;
        }
    }
}
