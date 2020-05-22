using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.UsageTests
{
    internal static class ExampleCertificate
    {
        public static X509Certificate2 Instance
        {
            get;
        }

        static ExampleCertificate()
        {
            string pem = @"-----BEGIN CERTIFICATE-----
MIIDuzCCAqOgAwIBAgIIEpyTrbhR1bswDQYJKoZIhvcNAQELBQAwgZMxCzAJBgNV
BAYTAlNLMRMwEQYDVQQHEwpCcmF0aXNsYXZhMRQwEgYDVQQKEwtoYXJyaXNvbjMx
NDEMMAoGA1UECxMDREVWMRgwFgYDVQQDEw9UZXN0Q2VydGlmaWNhdGUxIDAeBgkq
hkiG9w0BCQEWEWhlbGxvQGV4YW1wbGUuY29tMQ8wDQYDVQRBEwZQU0VVRE8wIBcN
MjAwNTIyMTgyNjAwWhgPMjA3MDA1MjIxODI2MDBaMIGTMQswCQYDVQQGEwJTSzET
MBEGA1UEBxMKQnJhdGlzbGF2YTEUMBIGA1UEChMLaGFycmlzb24zMTQxDDAKBgNV
BAsTA0RFVjEYMBYGA1UEAxMPVGVzdENlcnRpZmljYXRlMSAwHgYJKoZIhvcNAQkB
FhFoZWxsb0BleGFtcGxlLmNvbTEPMA0GA1UEQRMGUFNFVURPMIIBIjANBgkqhkiG
9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0VpQIgrmmEPJg1OQ1LI77YOPTK2JWD7jp9NE
3ZlNtvpWG5Hb4zYOznqZvY9HOcf7RSaFR9sO3lZ/Yy4Qr9Vz3fKxC13SgE3IyVnI
UZ/Ze1iL6JDmSnlZYY2pKNBFj5RSWVXcmTOBrjUIZtaY5/L2RMX4SDy5EVNz+gYK
J0ZYnWKvq0PHDvX0VE4pG4iOtptnxSHpHlawzHHxRADlORp9pPHEa3ewZwnOH90Q
mPNjXH+pt6xY4zUv/++THpz1zFeeY5HG46fvhVYzX3KCHFsUOZW+RTImk/jc3bqC
M0v5tUkKlrLUw5KneQNPoYKr4YUN2lpmTo/MyffcGB/0dQzvmwIDAQABow8wDTAL
BgNVHQ8EBAMCAeowDQYJKoZIhvcNAQELBQADggEBAKjyvEU22M97qW1dsoswZsWP
4Fm6hhY9wMo2Y2bx05x3jsXANHEREBi5+OlXgQi4wX8Shp2+5xMqjfd1+CIaBt2s
uUmF1bwBMhD7cJqD8jwqSxTB6IdcqfFjmowgnjSWRUq0HhINANpjbzkjDCavi5NT
kmOkwOFjfXxE2vfpSB3joaSfDAlgmQOhxnwjnxmg8hX4iJq9J8t0us8W2I8cyjxR
fD2jvwCV816vKiyS3jcx9Ol6UJ5y8ZspHGhOmJHYCMX4mjTagAYRlx1gqsedIoQo
oY1TnCJm0O/7MsC4AqvliZ/H+fsSl/0QK1Br6doy9oJisrXW/+DBKy9tCJZ03EI=
-----END CERTIFICATE-----
";
            Instance = new X509Certificate2(Encoding.ASCII.GetBytes(pem));
        }
    }
}
