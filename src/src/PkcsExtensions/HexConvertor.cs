﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions
{
    public static class HexConvertor
    {
        private static readonly char[] lowerCaseHex = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private static readonly char[] upperCaseHex = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static byte[] GetBytes(string hexValue)
        {
            ThrowHelpers.CheckNull(nameof(hexValue), hexValue);
            return GetBytes(hexValue.AsSpan());
        }

        public static byte[] GetBytes(ReadOnlySpan<char> hexValue)
        {
            if (hexValue.Length > 2 && hexValue[0] == '0' && (hexValue[1] == 'x' || hexValue[1] == 'X'))
            {
                hexValue = hexValue.Slice(2);
            }

            if ((hexValue.Length & 0x01) == 0x01)
            {
                throw new ArgumentException($"The argument hexValue contains an odd number of hexadecimal characters.");
            }

            byte[] array = new byte[hexValue.Length / 2];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)((GetHexVal(hexValue[i << 1]) << 4) + (GetHexVal(hexValue[(i << 1) + 1])));
            }

            return array;
        }

        public static bool TryGetBytes(ReadOnlySpan<char> hexValue, Span<byte> ouput, out int writeBytes)
        {
            if (hexValue.Length > 2 && hexValue[0] == '0' && (hexValue[1] == 'x' || hexValue[1] == 'X'))
            {
                hexValue = hexValue.Slice(2);
            }

            if ((hexValue.Length & 0x01) == 0x01)
            {
                throw new ArgumentException($"The argument hexValue contains an odd number of hexadecimal characters.");
            }

            int size = hexValue.Length / 2;
            if (ouput.Length < size)
            {
                writeBytes = 0;
                return false;
            }

            Span<byte> array = ouput.Slice(0, size);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)((GetHexVal(hexValue[i << 1]) << 4) + (GetHexVal(hexValue[(i << 1) + 1])));
            }

            writeBytes = size;
            return true;
        }

        public static string GetString(ReadOnlySpan<byte> data, HexFormat hexFormat = HexFormat.UpperCase)
        {
            char[] values = hexFormat == HexFormat.LowerCase ? lowerCaseHex : upperCaseHex;

#if NET5_0_OR_GREATER
            if (data.Length < 150)
            {
                Span<char> buffer = stackalloc char[data.Length * 2];
                int j = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    buffer[j++] = values[data[i] >> 4];
                    buffer[j++] = values[data[i] & 0xF];
                }

                return new string(buffer);
            }
            else
            {
                StringBuilder sb = new StringBuilder(data.Length * 2);
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(values[data[i] >> 4]);
                    sb.Append(values[data[i] & 0xF]);
                }

                return sb.ToString();
            }

#else
            StringBuilder sb = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(values[data[i] >> 4]);
                sb.Append(values[data[i] & 0xF]);
            }

            return sb.ToString();
#endif
        }

        public static bool TryGetString(ReadOnlySpan<byte> data, HexFormat hexFormat, Span<char> destination, out int witeChars)
        {
            witeChars = data.Length * 2;
            if (data.Length * 2 > destination.Length)
            {
                witeChars = 0;
                return false;
            }

            char[] values = hexFormat == HexFormat.LowerCase ? lowerCaseHex : upperCaseHex;

            for (int i = 0; i < witeChars; i += 2)
            {
                byte value = data[i >> 1];
                destination[i] = values[value >> 4];
                destination[i + 1] = values[value & 0xF];
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHexVal(char hex)
        {
            if (!((hex >= '0' && hex <= '9') || (hex >= 'a' && hex <= 'f') || (hex >= 'A' && hex <= 'F')))
            {
                ThrowNonHexCharacter(hex);
            }

            int val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        private static void ThrowNonHexCharacter(char hex)
        {
            string unicode = $"0000{((int)hex):X}";
            unicode = unicode.Substring(unicode.Length - 4);

            string charactedDescription;
            if (char.IsLetterOrDigit(hex) || char.IsPunctuation(hex) || char.IsSymbol(hex))
            {
                charactedDescription = $"'{hex}' (\\u{unicode})";
            }
            else if (char.IsWhiteSpace(hex))
            {
                charactedDescription = $"whitespace (\\u{unicode})";
            }
            else
            {
                charactedDescription = $"\\u{unicode}";
            }

            throw new ArgumentException($"The argument hexValue contains non-hexadecimal character: {charactedDescription}.");
        }
    }
}
