using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.Algorithms
{
    /// <summary>
    /// The secure random generator interface.
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public interface IRandomGenerator : IDisposable
    {
        void NextBytes(byte[] buffer);

        void NextBytes(byte[] bytes, int start, int len);

        void NextBytes(Span<byte> buffer);

        void AddSeedMaterial(byte[] inSeed);
    }
}
