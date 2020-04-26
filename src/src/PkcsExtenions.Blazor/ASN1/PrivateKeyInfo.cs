using PkcsExtenions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Blazor.ASN1
{
    public class PrivateKeyInfo : IAsn1Node
    {
        public int Version
        {
            get;
            protected set;
        }

        public AlgorithmIdentifier Algorithm
        {
            get;
            protected set;
        }

        public ReadOnlyMemory<byte> PrivateKey
        {
            get;
            protected set;
        }

        public PrivateKeyInfo(AsnReader asnReader)
        {
            AsnReader pski = asnReader.ReadSequence();
            pski.TryReadInt32(out int version);
            this.Version = version;

            this.Algorithm = new AlgorithmIdentifier(pski);
            pski.TryGetPrimitiveOctetStringBytes(out ReadOnlyMemory<byte> publicKeyBytes);
            this.PrivateKey = publicKeyBytes;
        }

        public void Write(AsnWriter asnWriter)
        {
            asnWriter.PushSequence();
            asnWriter.WriteInteger(this.Version);
            this.Algorithm.Write(asnWriter);
            asnWriter.WriteOctetString(this.PrivateKey.Span);
            asnWriter.PopSequence();
        }
    }
}
