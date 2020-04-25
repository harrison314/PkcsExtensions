using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions
{
    internal static class ThrowHelpers
    {
        public static void NotImplemented(string className, [CallerMemberName] string methodName = null)
        {
            throw new NotImplementedException($"{className}.{methodName} is not implement in this version PkcsExtenions.");
        }
    }
}
