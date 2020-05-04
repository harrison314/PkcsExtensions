# PKCS Extensions
PKCS extensions for .Net Standrad, .Net Core and Blazor BCL wihthout extenranl dependencies.

Code is focused for AOT compilation, IL linking and using with Blazor (small library, avoid reflection, minimalize internal code dependencies).

### Features
- Namespace **PkcsExtenions**:
  - HashAlgorithmConvertor - convert `HashALgorithmName` to OID, implementation,...
  - HexConvertor - convert from/to hexadecimal.
  - SecureStringHelper - safe provide `SecureString` to byte array.
- Namespace **PkcsExtenions.Algorithms**:
  - HashAlgorithmExtensions - More friendly extensions for `HashAlgorithm`.
  - DigestRandomGenerator - Secure random generator based on hash algorithm (inspired from Bauncy Castle). Is helpfull for password generation and entropy collection.
  - SP800-108 - Standrd KDF function in counter mode (inspired from Inferno library).
- Namespace **PkcsExtenions.ASN1** - ASN.1 writer and reader from Microsoft corefx repository.
- Namespace **PkcsExtenions.Pkcs1**:
  - Pkcs1DigestInfo - Is helpfull for RSA signing using SmartCards through PKCS#11.
- Namespace **PkcsExtenions.Pkcs7** - Missing features for [SignedCms](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.pkcs.signedcms?view=dotnet-plat-ext-3.1).
- Namespace **PkcsExtenions.X509Certificates**:
  - X509Certificate2Extensions - X509Certificate2 extensions for determine the usage of certificate.

 ## PkcsExtensions.Blazor
 Add extensions for Blazor and light [WebCrypto](https://developer.mozilla.org/en-US/docs/Web/API/Web_Crypto_API) interop.

 ### Features
 - Namespace **PkcsExtensions.Blazor**:
   - IWebCryptoProvider - provide generate random numbers, generate RSA and ECDSA (as JsonWebKey) key pairs
- Namespace **PkcsExtenions.Blazor.Jwk** - implementation of __JsonWebKey__
- Namespace **PkcsExtenions.Blazor.Security** - extensions for [System.Security.Cryptography](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography?view=netstandard-2.1)

## Inspire from
 - [Inferno](https://securitydriven.net/inferno/)
 - [Bouncy Castle](https://github.com/novotnyllc/bc-csharp)
 - [Misrosoft ASN1](https://github.com/dotnet/corefx/tree/07e9caf00ea0f1893d4c25a5ee287000903fbbe2/src/Common/src/System/Security/Cryptography)
