using NUnit.Framework;

namespace Yujt.Common.Helper.Tests
{
    [TestFixture]
    public class HttpRequsetHelperTests
    {
        [Test]
        public void GetContentTest()
        {
            var ipAddressProviderUrl = @"http://www.baidu.com";

            var response = HttpRequestHelper.Get(ipAddressProviderUrl, "GBK");

            Assert.NotNull(response);
        }
    }
}
