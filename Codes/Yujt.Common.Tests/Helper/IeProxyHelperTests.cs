using NUnit.Framework;
using Yujt.Common.Proxies;

namespace Yujt.Common.Helper.Tests
{
    [TestFixture]
    public class IeProxyHelperTests
    {
        [Test]
        public void IsProxyAvailableTest()
        {
            var host = "119.188.46.42";
            var  port ="8080";
            var actualResult = IeProxyHelper.IsProxyAvailable(host, port);

            Assert.IsTrue(actualResult);
        }
    }
}
