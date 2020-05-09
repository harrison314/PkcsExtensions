using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PkcsExtenions
{
    public static class PemFormater
    {
        public static string ToPem(ReadOnlySpan<byte> data, string name)
        {
            ThrowHelpers.CheckNullOrEempty(nameof(name), name);

            return string.Concat("-----BEGIN ", name, "-----\r\n",
                Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks),
                "\r\n-----END ", name, " -----");
        }

        public static byte[] ToPemBytes(ReadOnlySpan<byte> data, string name)
        {
            ThrowHelpers.CheckNullOrEempty(nameof(name), name);

            string pem = string.Concat("-----BEGIN ", name, "-----\r\n",
                Convert.ToBase64String(data, Base64FormattingOptions.InsertLineBreaks),
                "\r\n-----END ", name, " -----");

            return Encoding.ASCII.GetBytes(pem);
        }

        public static ReadOnlySpan<byte> FromDerOrPem(ReadOnlySpan<byte> data)
        {
            if (data.Length < 16) return data;

            if (data[0] == 45 && data[1] == 45 && data[2] == 45)
            {
                string pem = Encoding.ASCII.GetString(data);
                Match match = Regex.Match(pem, "-----BEGIN .+?-----(.+?)-----END .+?-----", RegexOptions.Singleline);
                if (!match.Success)
                {
                    throw new ArgumentException("data is not valid PEM");
                }

                return Convert.FromBase64String(match.Groups[1].Value);
            }
            else
            {
                return data;
            }
        }
    }
}
