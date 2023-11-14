using System.Collections.Generic;
using System.Text;

namespace PkcsExtensions.X509Certificates
{
    public class NameInfo
    {
        public string Oid
        {
            get;
        }

        public IReadOnlyList<string> Values
        {
            get;
        }

        public NameInfo(string oid, IReadOnlyList<string> values)
        {
            this.Oid = oid;
            this.Values = values;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string value in this.Values)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }

                sb.Append(this.Oid);
                sb.Append(": ");
                sb.Append(value);
                sb.Append(',');
            }

            return sb.ToString();
        }
    }
}
