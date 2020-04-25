using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Algorithms
{
    public static class RandomGeneratorExtensions
    {
        public static byte[] GenerateSeed(this IRandomGenerator generator, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

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

        public static int Next(this IRandomGenerator generator, int minValue, int maxValue)
        {
            ThrowHelpers.CheckRange(nameof(minValue), minValue, nameof(maxValue), maxValue);

            Span<byte> intBytes = stackalloc byte[sizeof(int)];
            generator.NextBytes(intBytes);

            int randomValue = BitConverter.ToInt32(intBytes);
            return (randomValue % (maxValue - minValue)) + minValue;
        }

        public static T Next<T>(this IRandomGenerator generator)
            where T : struct
        {
            Span<byte> structBytes = stackalloc byte[Marshal.SizeOf<T>()];
            generator.NextBytes(structBytes);

            Span<T> result = MemoryMarshal.Cast<byte, T>(structBytes);
            return result[0];
        }
    }
}
