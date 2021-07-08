using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions
{
    internal static class ThrowHelpers
    {
        public static void NotImplemented(string className, [CallerMemberName] string methodName = "")
        {
            throw new NotImplementedException($"{className}.{methodName} is not implement in this version PkcsExtensions.");
        }

        [DoesNotReturn]
        public static T NotImplemented<T>(string className, [CallerMemberName] string methodName = "")
        {
            throw new NotImplementedException($"{className}.{methodName} is not implement in this version PkcsExtensions.");
        }

        public static void CheckRange(string parameter1Name, int parameter1, string parameter2Name, int parameter2)
        {
            if (parameter1 >= parameter2)
            {
                throw new ArgumentOutOfRangeException($"Argument {parameter1Name} mus by less than argument {parameter2Name}.");
            }
        }

        public static void CheckRange(string parameterName, int value, int min, int max)
        {
            if (value < min || value > max)
            {
                throw new ArgumentOutOfRangeException($"Argument {parameterName} is out of range.");
            }
        }

        public static void CheckNullOrEempty(string name, string? value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(name);
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Argument {name} is empty string.");
            }
        }

        public static void CheckNull(string name, object? value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowArgumentException(string message)
        {
            throw new ArgumentException(message);
        }

        [DoesNotReturn]
        public static T NotSupport<T, TEnum>(TEnum value)
        {
            throw new NotSupportedException($"Enum value {value} is not supported.");
        }
    }
}
