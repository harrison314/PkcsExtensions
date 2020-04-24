using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Algorithms
{
    // TODO: IDisposable and reimplement random generators
    public interface IRandomGenerator
    {
        void NextBytes(byte[] buffer);

        void NextBytes(byte[] bytes, int start, int len);

        void NextBytes(Span<byte> buffer);

        void AddSeedMaterial(byte[] inSeed);
    }
}
