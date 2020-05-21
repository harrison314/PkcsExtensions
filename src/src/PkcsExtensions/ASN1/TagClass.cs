using System.Threading.Tasks;

namespace PkcsExtensions.ASN1
{
    // Uses a masked overlay of the tag class encoding.
    // T-REC-X.690-201508 sec 8.1.2.2
    public enum TagClass : byte
    {
        Universal = 0,
        Application = 0b0100_0000,
        ContextSpecific = 0b1000_0000,
        Private = 0b1100_0000,
    }
}
