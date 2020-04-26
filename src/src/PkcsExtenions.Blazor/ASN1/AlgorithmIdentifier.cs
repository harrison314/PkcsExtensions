using PkcsExtenions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Blazor.ASN1
{
    public class AlgorithmIdentifier : IAsn1Node
    {
        public string Algorithm
        {
            get;
            protected set;
        }

        public AlgorithmIdentifier(AsnReader asnReader)
        {
            AsnReader algorithm = asnReader.ReadSequence();
            this.Algorithm = algorithm.ReadObjectIdentifierAsString();
            algorithm.ReadNull();
        }

        public AlgorithmIdentifier(string algorithmOid)
        {
            this.Algorithm = algorithmOid;
        }

        public void Write(AsnWriter asnWriter)
        {
            asnWriter.PushSequence();
            asnWriter.WriteObjectIdentifier(this.Algorithm);
            asnWriter.WriteNull();
            asnWriter.PopSequence();
        }
    }
}
