using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Tests
{
    [TestClass]
    public class PemFormaterTests
    {
        [TestMethod]
        public void ToPem()
        {
            string result = PemFormater.ToPem(new byte[200], "NULLS");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ToPemBytes()
        {
            byte[] result = PemFormater.ToPemBytes(new byte[200], "NULLS");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FromDerOrPem()
        {
            string certificatePem = @"-----BEGIN CERTIFICATE-----
MIIG4jCCBMqgAwIBAgIJAJjguYVnU08GMA0GCSqGSIb3DQEBBQUAMIGmMQswCQYD
VQQGEwJVUzEOMAwGA1UECBMFVGV4YXMxFDASBgNVBAcTC1NhbiBBbnRvbmlvMRow
GAYDVQQKExFHbG9iYWxTQ0FQRSwgSW5jLjEUMBIGA1UECxMLRW5naW5lZXJpbmcx
FTATBgNVBAMTDG1pa2Utcm9vdC1jYTEoMCYGCSqGSIb3DQEJARYZbWhhbWJpZGdl
QGdsb2JhbHNjYXBlLmNvbTAeFw0xMDExMTgyMTE5NDdaFw0xNTExMTcyMTE5NDda
MIGmMQswCQYDVQQGEwJVUzEOMAwGA1UECBMFVGV4YXMxFDASBgNVBAcTC1NhbiBB
bnRvbmlvMRowGAYDVQQKExFHbG9iYWxTQ0FQRSwgSW5jLjEUMBIGA1UECxMLRW5n
aW5lZXJpbmcxFTATBgNVBAMTDG1pa2Utcm9vdC1jYTEoMCYGCSqGSIb3DQEJARYZ
bWhhbWJpZGdlQGdsb2JhbHNjYXBlLmNvbTCCAiIwDQYJKoZIhvcNAQEBBQADggIP
xYK3mO1034kBdDxmVoBeEwfjWWPyC/uyFGwCNZCzoAQGMxNAnj33NBiCLHJRo1Z5
BxirSSMxOT4LEkmkOhuTyKB0TJZf+8wP8pK5BsO3xjO+uM1K3LY=
-----END CERTIFICATE-----";

            byte[] pemData = Encoding.UTF8.GetBytes(certificatePem);

            ReadOnlySpan<byte> der = PemFormater.FromDerOrPem(pemData);
            Assert.AreNotEqual(0, der.Length);
        }
    }
}
