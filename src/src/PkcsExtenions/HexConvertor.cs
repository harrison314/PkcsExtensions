using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions
{
    public static class HexConvertor
    {
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

        public static string GetString(Span<byte> data, HexFormat hexFormat = HexFormat.UpperCase)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendFormat(hexFormat == HexFormat.LowerCase ? "{0:x2}" : "{0:X2}", data[i]);
            }

            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHexVal(char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
