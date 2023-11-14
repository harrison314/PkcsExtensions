using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PkcsExtensions.X509Certificates;

namespace PkcsExtensions.Tests.X509Certificate
{
    [TestClass]
    public class X509Certificate2NameInfoExtensionsTests
    {

        [DataTestMethod]
        [DataRow("hello world,O=harrison", "2.5.4.3", "hello world")]
        [DataRow("hello world,O=harrison,O=organization2", "2.5.4.10", "harrison,organization2")]
        [DataRow("hello world,O=harrison,C=SK", "2.5.4.45", "")]
        public void GetNameInfo_ForSubject(string subject, string oid, string values)
        {
            string[] nameValues = values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            X509Certificate2 certificate = CertificateGenerator.Create(subject);

            IReadOnlyList<string> resultList1 = certificate.GetNameInfo(oid, false);
            IReadOnlyList<string> resultList2 = certificate.GetNameInfo(oid, X509NameSource.Subject);

            CollectionAssert.AreEquivalent(nameValues, resultList1.ToArray(), "Error with bool variant.");
            CollectionAssert.AreEquivalent(nameValues, resultList2.ToArray(), "Error with X509NameSource variant.");
        }

        [DataTestMethod]
        [DataRow("hello world,O=harrison", "2.5.4.3", "hello world")]
        [DataRow("hello world,O=harrison,O=organization2", "2.5.4.10", "harrison,organization2")]
        [DataRow("hello world,O=harrison,C=SK", "2.5.4.45", "")]
        public void GetNameInfo_ForIssuer(string subject, string oid, string values)
        {
            string[] nameValues = values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            X509Certificate2 issuerCertificate = CertificateGenerator.Create(subject, X509KeyUsageFlags.KeyCertSign);
            X509Certificate2 certificate = CertificateGenerator.Create("CN=end", signedCertificate: issuerCertificate);

            IReadOnlyList<string> resultList1 = certificate.GetNameInfo(oid, true);
            IReadOnlyList<string> resultList2 = certificate.GetNameInfo(oid, X509NameSource.Issuer);

            CollectionAssert.AreEquivalent(nameValues, resultList1.ToArray(), "Error with bool variant.");
            CollectionAssert.AreEquivalent(nameValues, resultList2.ToArray(), "Error with X509NameSource variant.");
        }

        [DataTestMethod]
        [DataRow("hello world,O=harrison", "2.5.4.3", "hello world")]
        [DataRow("hello world,O=harrison,O=organization2", "2.5.4.10", "harrison,organization2")]
        public void GetNameInfos_ForSubject(string subject, string oid, string values)
        {
            string[] nameValues = values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            X509Certificate2 certificate = CertificateGenerator.Create(subject);

            IReadOnlyList<NameInfo> resultList1 = certificate.GetNameInfo(false);
            IReadOnlyList<NameInfo> resultList2 = certificate.GetNameInfo(X509NameSource.Subject);

            CollectionAssert.AreEquivalent(nameValues, resultList1.First(t => t.Oid == oid).Values.ToArray(), "Error with bool variant.");
            CollectionAssert.AreEquivalent(nameValues, resultList2.First(t => t.Oid == oid).Values.ToArray(), "Error with X509NameSource variant.");
        }

        [DataTestMethod]
        [DataRow("hello world,O=harrison", "2.5.4.3", "hello world")]
        [DataRow("hello world,O=harrison,O=organization2", "2.5.4.10", "harrison,organization2")]
        public void GetNameInfos_ForIssuer(string subject, string oid, string values)
        {
            string[] nameValues = values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            X509Certificate2 certificate = CertificateGenerator.Create(subject);

            IReadOnlyList<NameInfo> resultList1 = certificate.GetNameInfo(true);
            IReadOnlyList<NameInfo> resultList2 = certificate.GetNameInfo(X509NameSource.Issuer);

            CollectionAssert.AreEquivalent(nameValues, resultList1.First(t => t.Oid == oid).Values.ToArray(), "Error with bool variant.");
            CollectionAssert.AreEquivalent(nameValues, resultList2.First(t => t.Oid == oid).Values.ToArray(), "Error with X509NameSource variant.");
        }
    }
}
