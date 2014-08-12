using System;
using System.Diagnostics;
using NUnit.Framework;

namespace yujt.common.Proxies.Tests
{
    [TestFixture]
    public class ProxyFetcherTests
    {
        [Test]
        public void FetchChineseProxyTest()
        {
            var proxyList = new ProxyFetcher().FetchChineseProxy();

            Assert.IsNotNull(proxyList);
            Assert.Greater(proxyList.Count, 0);
        }

        [Test]
        public void FetchForeignProxyTest()
        {
            var proxyList = new ProxyFetcher().FetchForeignProxy();

            Assert.IsNotNull(proxyList);
            Assert.Greater(proxyList.Count, 0);
        }

        [Test]
        public void FetchAllTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var proxyList = new ProxyFetcher().FetchAll();
            stopWatch.Stop();

            Console.WriteLine(stopWatch.Elapsed);

            Assert.IsNotNull(proxyList);
            Assert.Greater(proxyList.Count, 0);
        }
    }
}
