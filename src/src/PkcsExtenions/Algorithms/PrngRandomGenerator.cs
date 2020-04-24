using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Algorithms
{
    public class PrngRandomGenerator : IRandomGenerator
    {
        public PrngRandomGenerator()
        {

        }

        public void AddSeedMaterial(byte[] inSeed)
        {

        }

        public void NextBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            using RandomNumberGenerator prng = RandomNumberGenerator.Create();
            prng.GetBytes(buffer);
        }

        public void NextBytes(byte[] buffer, int start, int len)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            using RandomNumberGenerator prng = RandomNumberGenerator.Create();
            prng.GetBytes(buffer, start, len);
        }

        public void NextBytes(Span<byte> buffer)
        {
            using RandomNumberGenerator prng = RandomNumberGenerator.Create();
            prng.GetBytes(buffer);
        }
    }
}
