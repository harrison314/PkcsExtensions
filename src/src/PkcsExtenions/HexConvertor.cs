using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions
{
    public static class HexConvertor
    {
        public static byte[] GetBytes(string hexValue)
        {
            if (hexValue == null) throw new ArgumentNullException(nameof(hexValue));

            byte[] array = new byte[hexValue.Length / 2];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte)((GetHexVal(hexValue[i << 1]) << 4) + (GetHexVal(hexValue[(i << 1) + 1])));
            }

            return array;
        }

        public static string GetString(Span<byte> data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendFormat("{0:X2}", data[i]);
            }

            return sb.ToString();
        }

        private static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
