# PKCS Extensions
[![NuGet Status](http://img.shields.io/nuget/v/PkcsExtensions.svg?style=flat)](https://www.nuget.org/packages/PkcsExtensions/)

PKCS extensions for .Net Standard, .Net Core and Blazor BCL without external dependencies.

Code is focused for AOT compilation, IL linking and using with Blazor (small library, avoid reflection, minimalize internal code dependencies).
## Usage
Install package `dotnet add package PkcsExtensions`.

## Features
- Namespace **PkcsExtensions**:
  - `ECDsaExtensions` - export keys to DER/PEM format on [ECDsa](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.ecdsa?view=netstandard-2.1).
  - `HashAlgorithmConvertor` - convert [HashAlgorithmName](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithmname?view=netstandard-2.1) to OID, implementation,...
  - `HexConvertor` - convert from/to hexadecimal.
  - `PemFormater` - helper class to convert DER to PEM and back.
  - `RSAExtensions` - export keys to DER/PEM format on [RSA](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsa?view=netstandard-2.1).
  - `SecureStringHelper` - safe provide [SecureString](https://docs.microsoft.com/en-us/dotnet/api/system.security.securestring?view=netstandard-2.1) to byte array.
- Namespace **PkcsExtensions.Algorithms**:
  - `DigestRandomGenerator` - Secure random generator based on hash algorithm (inspired from Bauncy Castle). Is helpful for password generation and entropy collection.
  - `HashAlgorithmExtensions` - More friendly extensions for [HashAlgorithm](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm?view=netstandard-2.1) types.
  - `RngRandomGenerator` - wraper to [RandomNumberGenerator](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator?view=netstandard-2.1)
  - `SP800_108` - Standard KDF function in counter mode (inspired from Inferno library).
- Namespace **PkcsExtensions.ASN1** - ASN.1 writer and reader from Microsoft corefx repository.
- Namespace **PkcsExtensions.Pkcs1**:
  - `Pkcs1DigestInfo` - Is helpful for RSA signing using SmartCards through PKCS#11.
- Namespace **PkcsExtensions.Pkcs7** - Missing features for [SignedCms](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.pkcs.signedcms?view=dotnet-plat-ext-3.1).
- Namespace **PkcsExtensions.X509Certificates**:
  - `X509Certificate2Extensions` - [X509Certificate2](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate2?view=netstandard-2.1) extensions for determine the usage of certificate.
  - `X509Certificate2EncodeExtensions` - encode (DER/PEM) extension for [X509Certificate2](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate2?view=netstandard-2.1).
  - `X509Certificate2NameInfoExtensions` - [X509Certificate2](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate2?view=netstandard-2.1) extensions for extract values by OID from isser and subject name - method `GetNameInfo`.

## Examples
See [code examples](src/test/PkcsExtensions.UsageTests) in test project. 

## Inspire from
 - [Inferno](https://securitydriven.net/inferno/)
 - [Bouncy Castle](https://github.com/novotnyllc/bc-csharp)
 - [Microsoft ASN1](https://github.com/dotnet/corefx/tree/07e9caf00ea0f1893d4c25a5ee287000903fbbe2/src/Common/src/System/Security/Cryptography)
