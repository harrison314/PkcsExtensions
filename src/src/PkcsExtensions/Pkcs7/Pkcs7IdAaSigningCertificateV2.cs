using PkcsExtensions.ASN1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Pkcs7
{
    public class Pkcs7IdAaSigningCertificateV2 : AsnEncodedData
    {
        public IReadOnlyCollection<X509Certificate2> SigningCertificates
        {
            get;
            protected set;
        }

        public HashAlgorithmName DigestAlgorithm
        {
            get;
            protected set;
        }

        public Pkcs7IdAaSigningCertificateV2(X509Certificate2 signingCertificate)
            : this(signingCertificate, HashAlgorithmName.SHA256)
        {

        }

        public Pkcs7IdAaSigningCertificateV2(X509Certificate2 signingCertificate, HashAlgorithmName digestAlgorithm)
            : base(Pkcs7Oids.SigningCertificateV2, CreateRawAsn1(signingCertificate, digestAlgorithm))
        {
            this.SigningCertificates = new List<X509Certificate2>()
            {
                signingCertificate
            };

            this.DigestAlgorithm = digestAlgorithm;
        }

        public Pkcs7IdAaSigningCertificateV2(IEnumerable<X509Certificate2> signingCertificates)
            : this(signingCertificates, HashAlgorithmName.SHA256)
        {

        }

        public Pkcs7IdAaSigningCertificateV2(IEnumerable<X509Certificate2> signingCertificates, HashAlgorithmName digestAlgorithm)
            : base(Pkcs7Oids.SigningCertificateV2, CreateRawAsn1(signingCertificates, digestAlgorithm))
        {
            this.SigningCertificates = new List<X509Certificate2>(signingCertificates);
            this.DigestAlgorithm = digestAlgorithm;
        }

        public override void CopyFrom(AsnEncodedData asnEncodedData)
        {
            ThrowHelpers.NotImplemented(nameof(Pkcs7IdAaContentHint));
        }

        public override string Format(bool multiLine)
        {
            ThrowHelpers.NotImplemented(nameof(Pkcs7IdAaContentHint));
            return default;
        }

        private static byte[] CreateRawAsn1(IEnumerable<X509Certificate2> signingCertificates, HashAlgorithmName hashAlgorithmName)
        {
            if (signingCertificates == null) throw new ArgumentNullException(nameof(signingCertificates));

            using HashAlgorithm hasher = HashAlgorithmConvertor.ToHashAlgorithm(hashAlgorithmName);
            Span<byte> hash = stackalloc byte[hasher.HashSize / 8];


            using AsnWriter asnWriter = new AsnWriter(AsnEncodingRules.DER);
            asnWriter.PushSequence();
            asnWriter.PushSequence();

            foreach (X509Certificate2 signingCertificate in signingCertificates)
            {
                hasher.TryComputeHash(signingCertificate.RawData, hash, out _);

                // Begin essCertIDv2 
                asnWriter.PushSequence();

                // Begin algorithm identifier
                asnWriter.PushSequence();
                asnWriter.WriteObjectIdentifier(HashAlgorithmConvertor.ToOid(hashAlgorithmName));
                asnWriter.WriteNull();
                asnWriter.PopSequence();
                // End Algorithm identifier

                asnWriter.WriteOctetString(hash);
                asnWriter.PopSequence();
                // End essCertIDv2 
            }

            asnWriter.PopSequence();
            asnWriter.PopSequence();

            return asnWriter.Encode();
        }

        private static byte[] CreateRawAsn1(X509Certificate2 signingCertificate, HashAlgorithmName hashAlgorithmName)
        {
            if (signingCertificate == null) throw new ArgumentNullException(nameof(signingCertificate));

            return CreateRawAsn1(new X509Certificate2[] { signingCertificate }, hashAlgorithmName);
        }
    }
}
