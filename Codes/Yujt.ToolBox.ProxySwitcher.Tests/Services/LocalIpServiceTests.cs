using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yujt.ToolBox.ProxySwitcher.Services;
using NUnit.Framework;
namespace Yujt.ToolBox.ProxySwitcher.Services.Tests
{
    [TestFixture()]
    public class LocalIpServiceTests
    {
        [Test()]
        public void GetLocalIpInfoTest()
        {
            const string cannotLocateYourIp = "无法定位您的IP与地址";
            var service = new LocalIpService();

            var ipInfo = service.GetLocalIpInfo();

            Assert.AreNotEqual(ipInfo, cannotLocateYourIp);
        }
    }
}
