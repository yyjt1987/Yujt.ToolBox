using NUnit.Framework;
namespace Yujt.Common.Proxies.Tests
{
    [TestFixture()]
    public class ProxyRepositoryTests
    {
        [Test()]
        public void GetProxiesTest()
        {
            var proxies = ProxyRepository.GetProxies();

            Assert.IsNotNull(proxies);
            Assert.IsTrue(proxies.Count > 0);
        }

        [Test()]
        public void GetProxyTest()
        {
            for (int i = 0; i < ProxyRepository.GetProxies().Count; i++)
            {
                var proxy = ProxyRepository.GetProxy();

                Assert.IsNotNull(proxy);
            }
        }
    }
}
