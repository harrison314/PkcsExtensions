using Microsoft.VisualStudio.TestTools.UnitTesting;
using PkcsExtenions.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtenions.Tests.Algorithms
{
    [TestClass]
    public class SP800_108Tests
    {
        [DataTestMethod]
        [DataRow("1f5942ea0ab514227e339d14743b1df7707a868483725d6166d850d57cd33420", "", "", 2)]
        [DataRow("85429dcd7b3c0113e18beefaa2c4ecc7ea692c54cc81d11d54cbf9b8ba29f85f", "", "", 5)]
        [DataRow("9e3003a812282e438024efb3aea47b87b713f784a31fa0cbafee55e67784a3cf", "", "", 78)]
        [DataRow("2456cee24129e4da9f23b4a906286f512cdb77e3185530d3b04e3e7fd3d2f4e1", "b10997f2fbd16e70", "", 2)]
        [DataRow("2456cee24129e4da9f23b4a906286f512cdb77e3185530d3b04e3e7fd3d2f4e1", "b7fb1340affe6611", "ef53ec919afd48d9604b8aa2", 2)]
        [DataRow("2456cee24129e4da9f23b4a906286f512cdb77e3185530d3b04e3e7fd3d2f4e1", "8795d281dac6270a", "ef", 4)]
        [DataRow("2456cee24129e4da9f23b4a906286f512cdb77e3185530d3b04e3e7fd3d2f4e1", "78c03b7627fdd67c57a5782a21e3b3704093b6628b597d1f535f4ef1fc33c38d94dc22d748e5edddbdc8678dbd4103e2999ea2a59182ea3b75dd2d888c0caae7", "e6262153b96129f94fbe34c991097ccdb23a09984cec6b31c7c5ca0735270362", 4)]
        public void CompareOriginalSha256(string hexKey, string hexLabel, string hexContent, int iterations)
        {
            byte[] key = HexConvertor.GetBytes(hexKey);
            byte[] label = HexConvertor.GetBytes(hexLabel);
            byte[] content = HexConvertor.GetBytes(hexContent);
            Func<HMAC> hmacfactory = () => new HMACSHA256();

            byte[] resultOriginal = new byte[124];
            byte[] resultCustom = new byte[124];

            SecurityDriven.Inferno.Kdf.SP800_108_Ctr.DeriveKey(hmacfactory,
                key,
                label.Length == 0 ? null : label,
                content.Length == 0 ? null : content,
                resultOriginal,
                (uint)iterations);

            SP800_108.DeriveKey(hmacfactory,
                key,
                label.Length == 0 ? Span<byte>.Empty : label,
                content.Length == 0 ? Span<byte>.Empty : content,
                resultCustom,
                (uint)iterations);

            Assert.AreEqual(HexConvertor.GetString(resultOriginal), HexConvertor.GetString(resultCustom));
        }

        [TestMethod]
        public void DeriveKeyWithString()
        {
            byte[] key = HexConvertor.GetBytes("1f5942ea0ab514227e339d14743b1df7707a868483725d6166d850d57cd33420");
            byte[] resultOriginal = new byte[124];

            SP800_108.DeriveKey("HMACSHA256", key, derivedOutput: resultOriginal);
        }
    }
}
