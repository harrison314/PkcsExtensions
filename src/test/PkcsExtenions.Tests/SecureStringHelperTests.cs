using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Tests
{
    [TestClass]
    public class SecureStringHelperTests
    {
        [DataTestMethod]
        [DataRow("UTF8", null)]
        [DataRow("UTF8", "")]
        [DataRow("UTF8", "t")]
        [DataRow("UTF8", "Password!")]
        [DataRow("ASCII", "Password!")]
        [DataRow("UTF8", "žrať vrátnik mať ~+-=")]
        [DataRow("UTF32", "žrať vrátnik mať ~+-=")]
        [DataRow("Unicode", "žrať vrátnik mať ~+-=")]
        [DataRow("UTF8", "дантист")]
        [DataRow("UTF8", "Microsoft.VisualStudio.TestTools.UnitTesting.Microsoft.VisualStudio.TestTools.UnitTesting.Microsoft.VisualStudio.TestTools.UnitTesting")]
        public void ExecuteWithSecureStringAction(string encodingname, string password)
        {
            SecureString secureString = this.Create(password);
            Encoding encoding = this.GetEncoding(encodingname);
            byte[] passwordBytes = password == null ? null : encoding.GetBytes(password);

            SecureStringHelper.ExecuteWithSecureString(secureString,
                encoding,
                array =>
                {
                    CollectionAssert.AreEquivalent(passwordBytes, array);
                });
        }

        [DataTestMethod]
        [DataRow("UTF8", null)]
        [DataRow("UTF8", "Password!")]
        [DataRow("UTF32", "žrať vrátnik mať ~+-=")]
        public void ExecuteWithSecureStringFunction(string encodingname, string password)
        {
            SecureString secureString = this.Create(password);
            Encoding encoding = this.GetEncoding(encodingname);
            byte[] passwordBytes = password == null ? null : encoding.GetBytes(password);

            int result = SecureStringHelper.ExecuteWithSecureString<int>(secureString,
                 encoding,
                 array =>
                 {
                     CollectionAssert.AreEquivalent(passwordBytes, array);
                     return 13;
                 });

            Assert.AreEqual(13, result);
        }

        [DataTestMethod]
        [DataRow("UTF8", null)]
        [DataRow("UTF8", "Password!")]
        [DataRow("UTF32", "žrať vrátnik mať ~+-=")]
        public void ExecuteWithSecureStringWithContext(string encodingname, string password)
        {
            SecureString secureString = this.Create(password);
            Encoding encoding = this.GetEncoding(encodingname);
            byte[] passwordBytes = password == null ? null : encoding.GetBytes(password);

            int result = SecureStringHelper.ExecuteWithSecureString<int, int>(secureString,
                 encoding,
                 13,
                 (array, ctx) =>
                 {
                     CollectionAssert.AreEquivalent(passwordBytes, array);
                     Assert.AreEqual(13, ctx);
                     return 42;
                 });

            Assert.AreEqual(42, result);
        }

        private SecureString Create(string text)
        {
            if (text == null)
            {
                return null;
            }

            SecureString secureString = new SecureString();
            for (int i = 0; i < text.Length; i++)
            {
                secureString.AppendChar(text[i]);
            }

            return secureString;
        }

        private Encoding GetEncoding(string name)
        {
            return name switch
            {
                "UTF8" => Encoding.UTF8,
                "Unicode" => Encoding.Unicode,
                "ASCII" => Encoding.ASCII,
                "UTF32" => Encoding.UTF32,
                _ => throw new NotSupportedException()
            };
        }
    }
}
