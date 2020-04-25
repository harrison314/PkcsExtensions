using Microsoft.JSInterop;
using PkcsExtenions.Blazor.Jwk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PkcsExtenions.Blazor.WebCrypto
{
    public class WebCryptoProvider : IWebCryptoProvider
    {
        private readonly IJSRuntime jsRuntime;

        public WebCryptoProvider(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        public async ValueTask<ECDsa> GenerateEcdsaKeyPair(WebCryptoCurveName curveName, CancellationToken cancellationToken = default)
        {
            string namedCurve = this.TranslateToCurveName(curveName);

            Dictionary<string, string> jwkFields = await this.jsRuntime.InvokeAsync<Dictionary<string, string>>("PkcsExtensionsBlazor_generateKeyEcdsa",
                cancellationToken: cancellationToken,
                args: new object[] { namedCurve });

            ECParameters ecParameters = new ECParameters();
            ecParameters.Curve = this.TranslateToEcCurve(curveName);

            ecParameters.D = Base64Url.EncodeFromString(jwkFields["d"]);
            ecParameters.Q.X = Base64Url.EncodeFromString(jwkFields["x"]);
            ecParameters.Q.Y = Base64Url.EncodeFromString(jwkFields["y"]);
            ecParameters.Validate();

            ECDsa ecdsa = ECDsa.Create(ecParameters);
            return ecdsa;
        }

        public async ValueTask<RSA> GenerateRsaKeyPair(int keySize, CancellationToken cancellationToken = default)
        {
            if (keySize <= 0 || keySize % 1024 != 0)
            {
                throw new ArgumentException("Invalid RSA key size.", nameof(keySize));
            }

            string b64Pkcs8 = await this.jsRuntime.InvokeAsync<string>("PkcsExtensionsBlazor_generateKeyRsa",
                cancellationToken: cancellationToken,
                args: new object[] { keySize });

            RSA rsa = RSA.Create();
            try
            {
                rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(b64Pkcs8), out _);

                return rsa;
            }
            catch
            {
                rsa.Dispose();
                throw;
            }
        }

        public async ValueTask<byte[]> GetRandomBytes(int count, CancellationToken cancellationToken = default)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return new byte[0];
            }

            string base64RandomData = await this.jsRuntime.InvokeAsync<string>("PkcsExtensionsBlazor_getRandomValues",
                cancellationToken: cancellationToken,
                args: new object[] { count });

            return Convert.FromBase64String(base64RandomData);
        }

        private string TranslateToCurveName(WebCryptoCurveName curveName)
        {
            return curveName switch
            {
                WebCryptoCurveName.NistP256 => "P-256",
                WebCryptoCurveName.NistP384 => "P-384",
                WebCryptoCurveName.NistP521 => "P-521",
                _ => throw new NotImplementedException()
            };
        }

        private ECCurve TranslateToEcCurve(WebCryptoCurveName curveName)
        {
            return curveName switch
            {
                WebCryptoCurveName.NistP256 => ECCurve.NamedCurves.nistP256,
                WebCryptoCurveName.NistP384 => ECCurve.NamedCurves.nistP384,
                WebCryptoCurveName.NistP521 => ECCurve.NamedCurves.nistP521,
                _ => throw new NotImplementedException()
            };
        }
    }
}
