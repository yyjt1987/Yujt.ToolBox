using NUnit.Framework;
namespace Yujt.Common.Tests
{
    [TestFixture()]
    public class CommonAppSettingTests
    {
        [Test()]
        public void CommonAppSettingTest()
        {
            const string expected = "http://cn-proxy.com/";
            var result = CommonAppSetting.Instatance.ChineseProxyProvider;

            Assert.IsNotNullOrEmpty(result);
            Assert.AreEqual(expected, result);
        }
    }
}
