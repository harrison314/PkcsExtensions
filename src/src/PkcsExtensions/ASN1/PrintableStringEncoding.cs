namespace PkcsExtensions.ASN1
{
    internal class PrintableStringEncoding : RestrictedAsciiStringEncoding
    {
        // T-REC-X.680-201508 sec 41.4
        internal PrintableStringEncoding()
            : base("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 '()+,-./:=?")
        {
        }
    }
}
