using System.Runtime.InteropServices;
using NUnit.Framework;
using yujt.common.Proxies;

namespace yujt.common.Helper.Tests
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
