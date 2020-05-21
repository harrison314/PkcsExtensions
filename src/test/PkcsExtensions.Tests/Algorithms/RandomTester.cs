using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtensions.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Tests.Algorithms
{
    internal static class RandomTester
    {
        public static void RunChiSquaredTests(IRandomGenerator random)
        {
            if (random == null) throw new ArgumentNullException(nameof(random));

            int passes = 0;

            for (int tries = 0; tries < 100; ++tries)
            {
                double chi2 = MeasureChiSquared(random, 1000);

                // 255 degrees of freedom in test => Q ~ 10.0% for 285
                if (chi2 < 285.0)
                {
                    ++passes;
                }
            }

            Assert.IsTrue(passes > 75, $"Random generator {random.GetType().FullName} generate low ChiSquared.");
        }

        private static double MeasureChiSquared(IRandomGenerator random, int rounds)
        {
            byte[] opts = random.GenerateSeed(2);
            int[] counts = new int[256];

            byte[] bs = new byte[256];
            for (int i = 0; i < rounds; ++i)
            {
                random.NextBytes(bs);

                for (int b = 0; b < 256; ++b)
                {
                    ++counts[bs[b]];
                }
            }

            byte mask = opts[0];
            for (int i = 0; i < rounds; ++i)
            {
                random.NextBytes(bs);

                for (int b = 0; b < 256; ++b)
                {
                    ++counts[bs[b] ^ mask];
                }

                ++mask;
            }

            byte shift = opts[1];
            for (int i = 0; i < rounds; ++i)
            {
                random.NextBytes(bs);

                for (int b = 0; b < 256; ++b)
                {
                    ++counts[(byte)(bs[b] + shift)];
                }

                ++shift;
            }

            int total = 3 * rounds;

            double chi2 = 0;
            for (int k = 0; k < counts.Length; ++k)
            {
                double diff = ((double)counts[k]) - total;
                double diff2 = diff * diff;

                chi2 += diff2;
            }

            chi2 /= total;

            return chi2;
        }
    }
}
