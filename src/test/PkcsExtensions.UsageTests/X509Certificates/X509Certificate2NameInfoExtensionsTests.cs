using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PkcsExtensions.X509Certificates;

namespace PkcsExtensions.UsageTests.X509Certificates
{
    [TestClass]
    public class X509Certificate2NameInfoExtensionsTests
    {
        [TestMethod]
        public void Example_GetNameInfo()
        {
            X509Certificate2 certificate = ExampleCertificate.Instance;
            IReadOnlyList<string> organizationNames = certificate.GetNameInfo("2.5.4.10", X509NameSource.Subject);

            Assert.IsNotNull($"Organization name in subject is {organizationNames.Single()}");
        }
    }
}
