using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Algorithms
{
    /// <summary>
    /// The secure digest random generator.
    /// </summary>
    public class DigestRandomGenerator : IRandomGenerator
    {
        private readonly HashAlgorithm digest;
        private readonly bool isDigestOwner;
        private readonly object syncRoot;
        private const long CYCLE_COUNT = 10;

        private long stateCounter;
        private long seedCounter;
        private byte[] state;
        private byte[] seed;

        public int HashSize
        {
            get => this.digest.HashSize;
        }

        public DigestRandomGenerator(HashAlgorithm hashAlgorithm)
        {
            ThrowHelpers.CheckNull(nameof(hashAlgorithm), hashAlgorithm);

            this.digest = hashAlgorithm;
            
            this.seed = new byte[this.digest.HashSize / 8];
            this.seedCounter = 1;
            this.state = new byte[this.digest.HashSize / 8];
            this.stateCounter = 1;
            this.isDigestOwner = false;

            this.syncRoot = new object();
        }

        public DigestRandomGenerator(HashAlgorithmName hashAlgorithmName)
            : this(HashAlgorithmConvertor.ToHashAlgorithm(hashAlgorithmName))
        {
            this.isDigestOwner = true;
        }

        public void AddSeedMaterial(byte[] inSeed)
        {
            ThrowHelpers.CheckNull(nameof(inSeed), inSeed);

            lock (this.syncRoot)
            {
                this.DigestUpdate(inSeed);
                this.DigestUpdate(this.seed);
                this.DigestDoFinal(this.seed);
            }
        }

        public void NextBytes(byte[] buffer)
        {
            ThrowHelpers.CheckNull(nameof(buffer), buffer);

            this.NextBytes(buffer, 0, buffer.Length);
        }

        public void NextBytes(byte[] bytes, int start, int len)
        {
            ThrowHelpers.CheckNull(nameof(bytes), bytes);
            if (start < 0 || start >= bytes.Length) throw new ArgumentOutOfRangeException(nameof(start));
            if (start + len > bytes.Length) throw new ArgumentOutOfRangeException(nameof(len));

            lock (this.syncRoot)
            {
                int stateOff = 0;

                this.GenerateState();

                int end = start + len;
                for (int i = start; i < end; ++i)
                {
                    if (stateOff == this.state.Length)
                    {
                        this.GenerateState();
                        stateOff = 0;
                    }
                    bytes[i] = this.state[stateOff++];
                }
            }
        }

        public void NextBytes(Span<byte> buffer)
        {
            lock (this.syncRoot)
            {
                int stateOff = 0;

                this.GenerateState();

                for (int i = 0; i < buffer.Length; ++i)
                {
                    if (stateOff == this.state.Length)
                    {
                        this.GenerateState();
                        stateOff = 0;
                    }
                    buffer[i] = this.state[stateOff++];
                }
            }
        }

        public void Dispose()
        {
            if (this.isDigestOwner)
            {
                this.digest.Dispose();
            }
        }

        private void CycleSeed()
        {
            this.DigestUpdate(this.seed);
            this.DigestAddCounter(this.seedCounter++);
            this.DigestDoFinal(this.seed);
        }

        private void GenerateState()
        {
            this.DigestAddCounter(this.stateCounter++);
            this.DigestUpdate(this.state);
            this.DigestUpdate(this.seed);
            this.DigestDoFinal(this.state);

            if ((this.stateCounter % CYCLE_COUNT) == 0)
            {
                this.CycleSeed();
            }
        }

        private void DigestAddCounter(long seedVal)
        {
            byte[] bytes = new byte[8];
            BitConverter.TryWriteBytes(bytes, seedVal);
            this.digest.TransformBlock(bytes, 0, bytes.Length, null, 0);
        }

        private void DigestUpdate(byte[] inSeed)
        {
            int offset = 0;
            int size = this.digest.HashSize / 8;

            while (inSeed.Length - offset >= size)
            {
                offset += this.digest.TransformBlock(inSeed, offset, size, null, 0);
            }

            if (inSeed.Length - offset >= 0)
            {
                this.digest.TransformBlock(inSeed, offset, inSeed.Length - offset, null, 0);
            }
        }

        private void DigestDoFinal(byte[] result)
        {
            this.digest.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            Buffer.BlockCopy(this.digest.Hash!, 0, result, 0, this.digest.HashSize / 8);
        }
    }
}
