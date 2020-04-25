using PkcsExtenions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Pkcs7
{
    public class Pkcs7IdAaContentHint : AsnEncodedData
    {
        public string FileName
        {
            get;
            protected set;
        }

        public string ContentDisposition
        {
            get;
            protected set;
        }

        public Pkcs7IdAaContentHint(string fileName, string contentDisposition = "application/octet-stream")
                : base(Pkcs7Oids.IdAaContentHint, CreateRawAsn1(contentDisposition, fileName))
        {
            this.FileName = fileName;
            this.ContentDisposition = contentDisposition;
        }

        public override void CopyFrom(AsnEncodedData asnEncodedData)
        {
            ThrowHelpers.NotImplemented(nameof(Pkcs7IdAaContentHint));
        }

        public override string Format(bool multiLine)
        {
            ThrowHelpers.NotImplemented(nameof(Pkcs7IdAaContentHint));
            return null;
        }

        private static byte[] CreateRawAsn1(string fileName, string contentDisposition)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (contentDisposition == null) throw new ArgumentNullException(nameof(contentDisposition));

            string idDataString = string.Concat("MIME-Version: 1.0\r\nContent-Type: ",
                contentDisposition, 
                "\r\nContent-Disposition: attachment;	filename=\"", 
                fileName, 
                "\"");

            using AsnWriter asnWriter = new AsnWriter(AsnEncodingRules.DER);
            asnWriter.PushSequence();
            asnWriter.WriteCharacterString(UniversalTagNumber.UTF8String, idDataString);
            asnWriter.WriteObjectIdentifier(Pkcs7Oids.IdData);
            asnWriter.PopSequence();

            return asnWriter.Encode();
        }
    }
}
