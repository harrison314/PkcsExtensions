using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.UsageTests
{
    [TestClass]
    public class SecureStringHelperTests
    {
        [TestMethod]
        public void Example_SecureStringHelper()
        {
            SecureString pin = this.ObtrainPin();

            SecureStringHelper.ExecuteWithSecureString(pin, Encoding.UTF8, pin =>
            {
                SetPinToDevice(pin);
            });
        }

        private SecureString ObtrainPin()
        {
            // Only for example

            SecureString pin = new SecureString();
            pin.AppendChar('4');
            pin.AppendChar('5');
            pin.AppendChar('6');
            pin.AppendChar('1');
            pin.AppendChar('2');
            pin.AppendChar('3');
            pin.AppendChar('7');
            pin.AppendChar('8');
            pin.AppendChar('9');

            return pin;
        }

        private static void SetPinToDevice(byte[]? pin)
        {
            // Only for example
        }
    }
}
