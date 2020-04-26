using Microsoft.JSInterop;
using PkcsExtenions.Blazor.Jwk;
using PkcsExtenions.Blazor.Pkcs8;
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

        public virtual async ValueTask<RSA> GenerateRsaKeyPair(int keySize, CancellationToken cancellationToken = default)
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
                rsa.ManagedImportPkcs8PrivateKey(Convert.FromBase64String(b64Pkcs8));
                return rsa;
            }
            catch
            {
                rsa.Dispose();
                throw;
            }
        }

        public virtual async ValueTask<byte[]> GetRandomBytes(int count, CancellationToken cancellationToken = default)
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
    }
}
