using System.Collections.Generic;
using System.Net;

namespace Yujt.Common.Helper
{
    public class WxHttpRequestHelper
    {
        public static string Get(string url, string cookieStr, IList<string> headers = null, string encodingStr = "UTF-8")
        {
            var cookieCollection = HttpRequestHelper.GenerateCookies(cookieStr);

            return HttpRequestHelper.Get(url, cookieCollection, GetHeader(headers), encodingStr);
        }

        public static string Post(string url,
                                  Dictionary<string, string> values,
                                  IList<string> headers,
                                  string cookieStr,
                                  string encodingStr = "UTF-8")
        {
            var cookieCollection = HttpRequestHelper.GenerateCookies(cookieStr);
            var valueStr = HttpRequestHelper.GeneratePostingData(values);
            var headerCollection = GetHeader(headers);

            return HttpRequestHelper.Post(url, valueStr, cookieCollection, headerCollection, encodingStr);
        }

        private static WebHeaderCollection GetHeader(IEnumerable<string> headers )
        {
            var headerCollection =  new WebHeaderCollection
            {
                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"},
                {"Accept-Charset", "utf-8, iso-8859-1, utf-16, *;q=0.7"},
                {"Accept-Encoding", "gzip,deflate"},
                {"Accept-Language", "zh-CN, en-US"},
                {
                    "User-Agent",
                    "Mozilla/5.0 (Linux; U; Android 4.1.2; zh-cn; SHV-E120L Build/JZO54K) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30 MicroMessenger/5.3.1.67_r745169.462 NetType/WIFI"
                },
                {"X-Requested-With", "com.tencent.mm"}
            };

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    var array = item.Split(':');
                    headerCollection.Add(array[0].Trim(), array[1].Trim());
                }
            }

            return headerCollection;
        }
    }
}
