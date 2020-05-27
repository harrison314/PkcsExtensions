using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Tests
{
    [TestClass]
    public class ECDsaExtensionsTests
    {
        [DataTestMethod]
        [DataRow(AsnFormat.Der)]
        [DataRow(AsnFormat.Pem)]
        public void ExportSubjectPublicKeyInfo(AsnFormat format)
        {
            using ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            byte[] exported = ecdsa.ExportSubjectPublicKeyInfo(format);

            if (format == AsnFormat.Der)
            {
                SubjectPublicKeyInfo info = SubjectPublicKeyInfo.GetInstance(Asn1Object.FromByteArray(exported));
                Assert.IsNotNull(info);
                Assert.IsNotNull(info.AlgorithmID);
                Assert.IsNotNull(info.PublicKeyData);
            }

            if (format == AsnFormat.Pem)
            {
                using MemoryStream ms = new MemoryStream(exported);
                using TextReader tr = new StreamReader(ms, Encoding.ASCII);
                PemReader pemReader = new PemReader(tr);
                object obj = pemReader.ReadObject();
                Assert.IsNotNull(obj);
            }

            this.CheckFormat(format, exported);
        }

        [DataTestMethod]
        [DataRow(AsnFormat.Der)]
        [DataRow(AsnFormat.Pem)]
        public void ExportRSAPrivateKey(AsnFormat format)
        {
            using ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            byte[] exported = ecdsa.ExportECPrivateKey(format);

            if (format == AsnFormat.Der)
            {
                using ECDsa ecdsaImport = ECDsa.Create();
                ecdsaImport.ImportECPrivateKey(exported, out _);
            }

            if (format == AsnFormat.Pem)
            {
                using MemoryStream ms = new MemoryStream(exported);
                using TextReader tr = new StreamReader(ms, Encoding.ASCII);
                PemReader pemReader = new PemReader(tr);
                object obj = pemReader.ReadObject();
                Assert.IsNotNull(obj);
            }

            this.CheckFormat(format, exported);
        }

        [DataTestMethod]
        [DataRow(AsnFormat.Der)]
        [DataRow(AsnFormat.Pem)]
        public void ExportPkcs8PrivateKey(AsnFormat format)
        {
            using ECDsa ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
            byte[] exported = ecdsa.ExportPkcs8PrivateKey(format);

            if (format == AsnFormat.Der)
            {
                var info = PrivateKeyFactory.CreateKey(exported);
                Assert.IsNotNull(info);
            }

            if (format == AsnFormat.Pem)
            {
                using MemoryStream ms = new MemoryStream(exported);
                using TextReader tr = new StreamReader(ms, Encoding.ASCII);
                PemReader pemReader = new PemReader(tr);
                object obj = pemReader.ReadObject();
                Assert.IsNotNull(obj);
            }

            this.CheckFormat(format, exported);
        }

        private void CheckFormat(AsnFormat format, byte[] data)
        {
            byte[] pemStart = Encoding.ASCII.GetBytes("-----BEGIN ");
            if (format == AsnFormat.Der)
            {
                if (this.StatWith(data, pemStart))
                {
                    Assert.Fail("Excepted data format is {0} but is not match to {1}", format, Convert.ToBase64String(data));
                }
            }

            if (format == AsnFormat.Pem)
            {
                if (!this.StatWith(data, pemStart))
                {
                    Assert.Fail("Excepted data format is {0} but is not match to {1}", format, Convert.ToBase64String(data));
                }
            }
        }

        private bool StatWith(byte[] data, byte[] start)
        {
            if (data.Length < start.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < start.Length; i++)
                {
                    if (data[i] != start[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
