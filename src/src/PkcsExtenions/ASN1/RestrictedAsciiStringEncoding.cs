using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace PkcsExtenions.ASN1
{
    internal abstract class RestrictedAsciiStringEncoding : SpanBasedEncoding
    {
        private readonly bool[] _isAllowed;

        protected RestrictedAsciiStringEncoding(byte minCharAllowed, byte maxCharAllowed)
        {
            Debug.Assert(minCharAllowed <= maxCharAllowed);
            Debug.Assert(maxCharAllowed <= 0x7F);

            bool[] isAllowed = new bool[0x80];

            for (byte charCode = minCharAllowed; charCode <= maxCharAllowed; charCode++)
            {
                isAllowed[charCode] = true;
            }

            _isAllowed = isAllowed;
        }

        protected RestrictedAsciiStringEncoding(IEnumerable<char> allowedChars)
        {
            bool[] isAllowed = new bool[0x7F];

            foreach (char c in allowedChars)
            {
                if (c >= isAllowed.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(allowedChars));
                }

                Debug.Assert(isAllowed[c] == false);
                isAllowed[c] = true;
            }

            _isAllowed = isAllowed;
        }

        public override int GetMaxByteCount(int charCount)
        {
            return charCount;
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }

        protected override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, bool write)
        {
            if (chars.IsEmpty)
            {
                return 0;
            }

            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];

                if ((uint)c >= (uint)_isAllowed.Length || !_isAllowed[c])
                {
                    EncoderFallback.CreateFallbackBuffer().Fallback(c, i);

                    Debug.Fail("Fallback should have thrown");
                    throw new CryptographicException();
                }

                if (write)
                {
                    bytes[i] = (byte)c;
                }
            }

            return chars.Length;
        }

        protected override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars, bool write)
        {
            if (bytes.IsEmpty)
            {
                return 0;
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];

                if ((uint)b >= (uint)_isAllowed.Length || !_isAllowed[b])
                {
                    DecoderFallback.CreateFallbackBuffer().Fallback(
                        new[] { b },
                        i);

                    Debug.Fail("Fallback should have thrown");
                    throw new CryptographicException();
                }

                if (write)
                {
                    chars[i] = (char)b;
                }
            }

            return bytes.Length;
        }
    }
}
