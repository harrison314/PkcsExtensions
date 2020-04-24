using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Algorithms
{
    public static class RandomGeneratorExtensions
    {
        public static byte[] GenerateSeed(this IRandomGenerator generator, int length)
        {
            byte[] result = new byte[length];
            generator.NextBytes(result);
            return result;
        }

        public static void GenerateSeed(this IRandomGenerator generator, DateTime? dateTime = null)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            byte[] data = BitConverter.GetBytes(dateTime.Value.Ticks);
            generator.AddSeedMaterial(data);
        }
    }
}
