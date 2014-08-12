using System;
using System.Linq;
using HtmlAgilityPack;
using Yujt.Common.Helper;

namespace Yujt.ToolBox.ProxySwitcher.Services
{
    public class LocalIpService
    {
        private const string IP_ADDRESS_PROVIDER_URL = @"http://www.ip138.com/";
        private const string CANNOT_LOCATE_YOUR_IP = "连接目标服务器超时，请再次尝试或打开网页测试连接！";
        public string GetLocalIpInfo()
        {
            try
            {
                var homePageContent = HttpRequestHelper.Get(IP_ADDRESS_PROVIDER_URL, "GBK");
                var doc = new HtmlDocument();
                doc.LoadHtml(homePageContent);

                var ipFrame = doc.DocumentNode.SelectNodes("//iframe").First();
                var ipAddressPage = ipFrame.GetAttributeValue("src", string.Empty);

                var ipContent = HttpRequestHelper.Get(ipAddressPage, encodingStr:"GBK");
                doc.LoadHtml(ipContent);

                var ipInfo = doc.DocumentNode.SelectNodes("//center").First().InnerText;

                return ipInfo;
            }
            catch (Exception)
            {
                return CANNOT_LOCATE_YOUR_IP;
            }

        }
    }
}
