using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;
using Yujt.Common;
using Yujt.Common.Helper;


namespace yujt.common.Proxies
{
    //[Export]
    public class ProxyFetcher : IProxyFetcher
    {
        private const string IP_ADDRESS_PATTERN = @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))";
        public List<Proxy> FetchChineseProxy()
        {
            var proxies = new List<Proxy>();
            int count = 0;
            while (count < 5)
            {
                count++;
                try
                {
                    var proxyHtml = HttpRequestHelper.Get(CommonAppSetting.Instatance.ChineseProxyProvider,
                                                          null,
                                                          GetHeaders());
                    var doc = new HtmlDocument();
                    doc.LoadHtml(proxyHtml);

                    foreach (var tr in doc.DocumentNode.SelectNodes("//tr"))
                    {
                        var tds = tr.ChildNodes.Where(item => item.NodeType != HtmlNodeType.Text).ToArray();

                        var host = tds[0].InnerText.RemoveWhiteSpace();
                        if (Regex.IsMatch(host, IP_ADDRESS_PATTERN))
                        {
                            proxies.Add(new Proxy
                            {
                                Host = host,
                                Port = tds[1].InnerText.RemoveWhiteSpace(),
                                Area = tds[2].InnerText.Trim().Split(' ')[0],
                                Address = tds[2].InnerText.Trim()
                            });
                        }
                    }//Find all of proxies in table
                    return proxies;
                }
                catch (Exception)
                {
                }
                Thread.Sleep(1000);
            }
            return proxies;

        }

        public List<Proxy> FetchForeignProxy()
        {
            var proxies = new List<Proxy>();
            int count = 0;
            while (count < 5)
            {
                count++;
                try
                {

                    var proxyHtml = HttpRequestHelper.Get(CommonAppSetting.Instatance.ForeignProxyProvider,
                                                          null,
                                                          GetHeaders());
                    var doc = new HtmlDocument();
                    doc.LoadHtml(proxyHtml);

                    foreach (var tr in doc.DocumentNode.SelectNodes("//tr"))
                    {
                        var tds = tr.ChildNodes.Where(item => item.NodeType != HtmlNodeType.Text).ToArray();

                        var host = tds[0].InnerText.RemoveWhiteSpace();
                        if (Regex.IsMatch(host, IP_ADDRESS_PATTERN))
                        {
                            proxies.Add(new Proxy
                            {
                                Host = host,
                                Port = tds[1].InnerText.RemoveWhiteSpace(),
                                Area = "国外",
                                Address = tds[3].InnerText.Trim()
                            });
                        }
                    }
                    return proxies;
                }
                catch (Exception)
                {
                }
                Thread.Sleep(1000);
            }
            return proxies;
        }

        public List<Proxy> FetchAll()
        {
            var proxyList = FetchChineseProxy();
            proxyList.AddRange(FetchForeignProxy());
            return proxyList;
        }


        private WebHeaderCollection GetHeaders()
        {
            var headers = new WebHeaderCollection
            {
                {"Timeout", "10000"}
            };
            return headers;
        }


    }
}
