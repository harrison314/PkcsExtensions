using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PkcsExtensions.Algorithms
{
    public static class HashAlgorithmExtensions
    {
#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static void Update(this HashAlgorithm hashAlgorithm, byte[] input)
        {
            ThrowHelpers.CheckNull(nameof(input), input);

            int offset = 0;
            int size = hashAlgorithm.HashSize / 8;

            while (input.Length - offset >= size)
            {
                offset += hashAlgorithm.TransformBlock(input, offset, size, null, 0);
            }

            if (input.Length - offset > 0)
            {
                hashAlgorithm.TransformBlock(input, offset, input.Length - offset, null, 0);
            }
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static void Update(this HashAlgorithm hashAlgorithm, byte[] input, int start, int length)
        {
            ThrowHelpers.CheckNull(nameof(input), input);
            if (start < 0 || start >= input.Length) throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0 || start + length > input.Length) throw new ArgumentOutOfRangeException(nameof(length));

            int offset = start;
            int size = hashAlgorithm.HashSize / 8;

            while (start + length - offset >= size)
            {
                offset += hashAlgorithm.TransformBlock(input, offset, size, null, 0);
            }

            if (start + length - offset > 0)
            {
                hashAlgorithm.TransformBlock(input, offset, input.Length - offset, null, 0);
            }
        }

        public static async Task Update(this HashAlgorithm hashAlgorithm, Stream stream, CancellationToken cancellationToken = default)
        {
            ThrowHelpers.CheckNull(nameof(stream), stream);

            int size = hashAlgorithm.HashSize / 8;
#if NETCOREAPP
            byte[] buffer = GC.AllocateUninitializedArray<byte>(size, false);
#else
            byte[] buffer = new byte[size];
#endif
            int read;

            while ((read = await stream.ReadAsync(buffer, 0, size, cancellationToken).ConfigureAwait(false)) > 0)
            {
                hashAlgorithm.TransformBlock(buffer, 0, read, null, 0);
            }
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static byte[] DoFinal(this HashAlgorithm hashAlgorithm)
        {
#if NETCOREAPP
            byte[] result = GC.AllocateUninitializedArray<byte>(hashAlgorithm.HashSize / 8, false);
#else
            byte[] result = new byte[hashAlgorithm.HashSize / 8];
#endif
            hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            Buffer.BlockCopy(hashAlgorithm.Hash!, 0, result, 0, result.Length);

            return result;
        }

#if NET6_0 || NET5_0
        [System.Runtime.Versioning.UnsupportedOSPlatform("browser")]
#endif
        public static bool TryDoFinal(this HashAlgorithm hashAlgorithm, Span<byte> hash, out int writeBytes)
        {
            int size = hashAlgorithm.HashSize / 8;
            if (hash.Length < size)
            {
                writeBytes = 0;
                return false;
            }

            hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            (new Span<byte>(hashAlgorithm.Hash)).CopyTo(hash);

            writeBytes = size;
            return true;
        }
    }
}
