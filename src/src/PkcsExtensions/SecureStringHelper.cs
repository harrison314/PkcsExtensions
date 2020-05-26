using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions
{
    public static class SecureStringHelper
    {
        public static void ExecuteWithSecureString(SecureString secureString, Encoding encoding, Action<byte[]?> action)
        {
            ThrowHelpers.CheckNull(nameof(encoding), encoding);
            ThrowHelpers.CheckNull(nameof(action), action);

            if (secureString == null)
            {
                action.Invoke(null);
            }
            else
            {
                ExecuteWithSecureStringInternal<int, int>(secureString, encoding, 0, (buffer, ctx) =>
                {
                    action.Invoke(buffer);
                    return 0;
                });
            }
        }

        public static TResult ExecuteWithSecureString<TResult>(SecureString secureString, Encoding encoding, Func<byte[]?, TResult> action)
        {
            ThrowHelpers.CheckNull(nameof(encoding), encoding);
            ThrowHelpers.CheckNull(nameof(action), action);

            if (secureString == null)
            {
                return action.Invoke(null);
            }
            else
            {
                return ExecuteWithSecureStringInternal<TResult, int>(secureString, encoding, 0, (buffer, ctx) => action(buffer));
            }
        }

        public static TResult ExecuteWithSecureString<TResult, TContext>(SecureString secureString, Encoding encoding, TContext context, Func<byte[]?, TContext, TResult> action)
        {
            ThrowHelpers.CheckNull(nameof(encoding), encoding);
            ThrowHelpers.CheckNull(nameof(action), action);

            if (secureString == null)
            {
                return action.Invoke(null, context);
            }
            else
            {
                return ExecuteWithSecureStringInternal<TResult, TContext>(secureString, encoding, context, action);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private unsafe static TReturn ExecuteWithSecureStringInternal<TReturn, TContext>(SecureString secureString, Encoding encoding, TContext context, Func<byte[]?, TContext, TReturn> action)
        {
            int maxBytes = encoding.GetMaxByteCount(secureString.Length);
            IntPtr buffer = IntPtr.Zero;
            IntPtr secureStringPtr = IntPtr.Zero;
            try
            {
                secureStringPtr = Marshal.SecureStringToBSTR(secureString);
                buffer = Marshal.AllocHGlobal(maxBytes);
                int encodedBytesLenght = encoding.GetBytes((char*)secureStringPtr.ToPointer(), secureString.Length, (byte*)buffer.ToPointer(), maxBytes);
                Marshal.ZeroFreeBSTR(secureStringPtr);
                secureStringPtr = IntPtr.Zero;

                byte[] pinByteArray = new byte[encodedBytesLenght];
                GCHandle pinByteArrayPined = GCHandle.Alloc(pinByteArray, GCHandleType.Pinned);
                try
                {
                    Marshal.Copy(buffer, pinByteArray, 0, encodedBytesLenght);
                    ClearBuffer(new Span<byte>(buffer.ToPointer(), maxBytes));
                    Marshal.FreeHGlobal(buffer);
                    buffer = IntPtr.Zero;

                    return action.Invoke(pinByteArray, context);

                }
                finally
                {
                    ClearBuffer(pinByteArray);
                    pinByteArrayPined.Free();
                }
            }
            finally
            {
                if (secureStringPtr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(secureStringPtr);
                }

                if (buffer != IntPtr.Zero)
                {
                    ClearBuffer(new Span<byte>(buffer.ToPointer(), maxBytes));
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void ClearBuffer(Span<byte> buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0x00;
            }
        }
    }
}
