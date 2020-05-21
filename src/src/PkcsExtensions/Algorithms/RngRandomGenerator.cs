﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Algorithms
{
    public class RngRandomGenerator : IRandomGenerator
    {
        private readonly RandomNumberGenerator prng;

        public RngRandomGenerator()
        {
            this.prng = RandomNumberGenerator.Create();
        }

        public void AddSeedMaterial(byte[] inSeed)
        {
            ThrowHelpers.CheckNull(nameof(inSeed), inSeed);
        }

        public void NextBytes(byte[] buffer)
        {
            ThrowHelpers.CheckNull(nameof(buffer), buffer);

            this.prng.GetBytes(buffer);
        }

        public void NextBytes(byte[] buffer, int start, int len)
        {
            ThrowHelpers.CheckNull(nameof(buffer), buffer);

            this.prng.GetBytes(buffer, start, len);
        }

        public void NextBytes(Span<byte> buffer)
        {
            this.prng.GetBytes(buffer);
        }

        public void Dispose()
        {
            this.prng.Dispose();
        }
    }
}
