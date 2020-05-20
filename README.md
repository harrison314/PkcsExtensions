# PKCS Extensions
[![NuGet Status](http://img.shields.io/nuget/v/PkcsExtenions.svg?style=flat)](https://www.nuget.org/packages/PkcsExtenions/)

PKCS extensions for .Net Standard, .Net Core and Blazor BCL without external dependencies.

Code is focused for AOT compilation, IL linking and using with Blazor (small library, avoid reflection, minimalize internal code dependencies).

### Features
- Namespace **PkcsExtenions**:
  - `HashAlgorithmConvertor` - convert [HashAlgorithmName](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithmname?view=netstandard-2.1) to OID, implementation,...
  - `HexConvertor` - convert from/to hexadecimal.
  - `SecureStringHelper` - safe provide [SecureString](https://docs.microsoft.com/en-us/dotnet/api/system.security.securestring?view=netstandard-2.1) to byte array.
- Namespace **PkcsExtenions.Algorithms**:
  - `HashAlgorithmExtensions` - More friendly extensions for [HashAlgorithm](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm?view=netstandard-2.1) types.
  - `DigestRandomGenerator` - Secure random generator based on hash algorithm (inspired from Bauncy Castle). Is helpful for password generation and entropy collection.
  - `SP800-108` - Standard KDF function in counter mode (inspired from Inferno library).
- Namespace **PkcsExtenions.ASN1** - ASN.1 writer and reader from Microsoft corefx repository.
- Namespace **PkcsExtenions.Pkcs1**:
  - `Pkcs1DigestInfo` - Is helpful for RSA signing using SmartCards through PKCS#11.
- Namespace **PkcsExtenions.Pkcs7** - Missing features for [SignedCms](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.pkcs.signedcms?view=dotnet-plat-ext-3.1).
- Namespace **PkcsExtenions.X509Certificates**:
  - `X509Certificate2Extensions` - [X509Certificate2](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate2?view=netstandard-2.1) extensions for determine the usage of certificate.
  - `X509Certificate2EncodeExtensions` - encode (DER/PEM) extension for [X509Certificate2](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate2?view=netstandard-2.1).
  - `X509Certificate2NameInfoExtensions` - [X509Certificate2](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate2?view=netstandard-2.1) extensions for extract values by OID from isser and subject name - method `GetNameInfo`.

 ## PkcsExtensions.Blazor
 Add extensions for Blazor and light [WebCrypto](https://developer.mozilla.org/en-US/docs/Web/API/Web_Crypto_API) interop.

 ### Features
 - Namespace **PkcsExtensions.Blazor**:
   - `IWebCryptoProvider` - provide generate random numbers, generate RSA and ECDSA (as JsonWebKey) key pairs
   - `IEcWebCryptoProvider` - provide methods `GetSharedDhmSecret` for derive bytes using _Diffie Hellman Merkle_ and `GetSharedEphemeralDhmSecret` for ECIES scheme.
- Namespace **PkcsExtenions.Blazor.Jwk** - implementation of __JsonWebKey__
- Namespace **PkcsExtenions.Blazor.Security** - extensions for [System.Security.Cryptography](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography?view=netstandard-2.1)

### Usage
Install package `dotnet add package PkcsExtensions.Blazor` to Blazor WebAssebmly project.

Add to _index.html_:
```html
<script src="_content/PkcsExtenions.Blazor/WebCryptoInterop.js"></script>
```

And register services in _Main_ method:
```cs
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddWebCryptoProvider();

            WebAssemblyHost host = builder.Build();
            await host.RunAsync();
        }
    }
```

### Examples

See [other examples](Examples/BlazorWebAssemblyExamples.md).

### Recommendations
- Avoid use WebCyrpto for hashing, HMAC-ing, encryption, because their implementations has differs between browsers and operating systems. Use _.Net_ implementation.
- Hint: Consider using high performance elliptic curves [Curve25519](https://en.wikipedia.org/wiki/Curve25519),
[Ed25519](https://en.wikipedia.org/wiki/EdDSA#Ed25519) or similar. Use full managed implementation e.g. [Chaos.NaCl library](https://github.com/CodesInChaos/Chaos.NaCl).

## Inspire from
 - [Inferno](https://securitydriven.net/inferno/)
 - [Bouncy Castle](https://github.com/novotnyllc/bc-csharp)
 - [Microsoft ASN1](https://github.com/dotnet/corefx/tree/07e9caf00ea0f1893d4c25a5ee287000903fbbe2/src/Common/src/System/Security/Cryptography)
