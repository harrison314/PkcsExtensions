using PkcsExtenions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Blazor.ASN1
{
    public class SubjectPublicKeyInfo : IAsn1Node
    {
        public AlgorithmIdentifier Algorithm
        {
            get;
        }

        public ReadOnlyMemory<byte> PublicKey
        {
            get;
        }

        public SubjectPublicKeyInfo(AsnReader asnReader)
        {
            AsnReader pski = asnReader.ReadSequence();
            this.Algorithm = new AlgorithmIdentifier(pski);
            pski.TryGetPrimitiveOctetStringBytes(out ReadOnlyMemory<byte> publicKeyBytes);
            this.PublicKey = publicKeyBytes;
        }

        public void Write(AsnWriter asnWriter)
        {
            asnWriter.PushSequence();
            this.Algorithm.Write(asnWriter);
            asnWriter.WriteOctetString(this.PublicKey.Span);
            asnWriter.PopSequence();
        }
    }
}
