using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtenions.Pkcs7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Tests.Pkcs7
{
    [TestClass]
    public class Pkcs7IdAaContentHintTests
    {
        [TestMethod]
        public void Create()
        {
            new Pkcs7IdAaContentHint("hello.txt");
        }

        [TestMethod]
        public void CreateWithContentType()
        {
            new Pkcs7IdAaContentHint("hello.txt", "text/plain");
        }
    }
}
