using PkcsExtensions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Pkcs7
{
    public class Pkcs7IdAaContentHint : AsnEncodedData
    {
        public string FileName
        {
            get;
            protected set;
        }

        public string ContentType
        {
            get;
            protected set;
        }

        public Pkcs7IdAaContentHint(string fileName, string contentType)
                : base(Pkcs7Oids.IdAaContentHint, CreateRawAsn1(fileName, contentType))
        {
            this.FileName = fileName;
            this.ContentType = contentType;
        }

        public Pkcs7IdAaContentHint(string fileName)
            :this(fileName, "application/octet-stream")
        {

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

        private static byte[] CreateRawAsn1(string fileName, string contentType)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));

            string idDataString = string.Concat("MIME-Version: 1.0\r\nContent-Type: ",
                contentType, 
                "\r\nContent-Disposition: attachment; filename=\"", 
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
