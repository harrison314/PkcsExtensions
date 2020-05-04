using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Algorithms
{
    public class HashAlgorithmWrapper
    {
        private readonly HashAlgorithm hashAlgorithm;

        public int HashSizeInBytes
        {
            get => this.hashAlgorithm.HashSize / 8;
        }

        public HashAlgorithmWrapper(HashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm ?? throw new ArgumentNullException(nameof(hashAlgorithm));
        }

        public void Clear()
        {
            this.hashAlgorithm.Clear();
        }

        public void Update(byte[] input)
        {
            ThrowHelpers.CheckNull(nameof(input), input);

            int offset = 0;
            int size = this.hashAlgorithm.HashSize / 8;

            while (input.Length - offset >= size)
            {
                offset += this.hashAlgorithm.TransformBlock(input, offset, size, null, 0);
            }

            if (input.Length - offset > 0)
            {
                this.hashAlgorithm.TransformBlock(input, offset, input.Length - offset, null, 0);
            }
        }

        public void Update(byte[] input, int start, int length)
        {
            ThrowHelpers.CheckNull(nameof(input), input);
            if (start < 0 || start >= input.Length) throw new ArgumentOutOfRangeException(nameof(start));
            if (length < 0 || start + length > input.Length) throw new ArgumentOutOfRangeException(nameof(length));

            int offset = start;
            int size = this.hashAlgorithm.HashSize / 8;

            while (start + length - offset >= size)
            {
                offset += this.hashAlgorithm.TransformBlock(input, offset, size, null, 0);
            }

            if (start + length - offset > 0)
            {
                this.hashAlgorithm.TransformBlock(input, offset, input.Length - offset, null, 0);
            }
        }

        public byte[] DoFinal()
        {
            byte[] result = new byte[this.hashAlgorithm.HashSize / 8];
            this.hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            Buffer.BlockCopy(this.hashAlgorithm.Hash, 0, result, 0, result.Length);

            return result;
        }

        public bool TryDoFinal(Span<byte> hash, out int writeBytes)
        {
            int size = this.hashAlgorithm.HashSize / 8;
            if (hash.Length < size)
            {
                writeBytes = 0;
                return false;
            }

            this.hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            (new Span<byte>(this.hashAlgorithm.Hash)).CopyTo(hash);

            writeBytes = size;
            return true;
        }
    }
}
