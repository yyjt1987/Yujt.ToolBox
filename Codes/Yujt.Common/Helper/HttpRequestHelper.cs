using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Yujt.Common.Helper
{
    public class HttpRequestHelper
    {
        private const int DEFAULT_TIMEOUT = 6000;
        public static string Get(string url, string encodingStr = "UTF-8")
        {
            return Get(url, null, null, encodingStr);
        }

        public static string Get(string url, CookieCollection cookieCollection, WebHeaderCollection headers,
            string encodingStr = "UTF-8")
        {
            //var request = (HttpWebRequest) WebRequest.Create(url);
            //request.Method = "GET";
            //request.Timeout = 2000;

            //if (cookieCollection != null)
            //{
            //    var cookieContainer = new CookieContainer();
            //    cookieContainer.Add(new Uri(url), cookieCollection);
            //    request.CookieContainer = cookieContainer;
            //}

            //AssginHeaer(request, headers);

            //using (var response = (HttpWebResponse) request.GetResponse())
            //{
            //    if (response.StatusCode != HttpStatusCode.OK)
            //    {
            //        throw new Exception(response.StatusDescription);

            //    }
            //    //读取流信息
            //    using (var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encodingStr)))
            //    {
            //        return sr.ReadToEnd();
            //    }
            //}
            using (var response = GetResponse(url, cookieCollection, headers, encodingStr))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.StatusDescription);

                }
                using (var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encodingStr)))
                {
                    return sr.ReadToEnd();
                }
            }
        }



        public static HttpWebResponse GetResponse(string url, CookieCollection cookieCollection, WebHeaderCollection headers, string encodingStr = "UTF-8")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 2000;

            if (cookieCollection != null)
            {
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(new Uri(url), cookieCollection);
                request.CookieContainer = cookieContainer;
            }

            AssginHeaer(request, headers);
            return (HttpWebResponse)request.GetResponse();
        }

        public static string Post(string url, Dictionary<string, string> value, CookieCollection cookieCollection = null,
            WebHeaderCollection headers = null, string encodingStr = "UTF-8")
        {
            var valueStr = GeneratePostingData(value);
            return Post(url, valueStr, cookieCollection, headers, encodingStr);
        }
        public static string Post(string url, string value, CookieCollection cookieCollection = null, WebHeaderCollection headers = null, string encodingStr = "UTF-8")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = DEFAULT_TIMEOUT;

            if (cookieCollection != null)
            {
                var cookieContainer = new CookieContainer();
                cookieContainer.Add(new Uri(url), cookieCollection);
                request.CookieContainer = cookieContainer;
            }
            AssginHeaer(request, headers);
            //将数据写入请求流
            if (!string.IsNullOrEmpty(value))
            {
                request.Method = "POST";
                var buffer = Encoding.GetEncoding(encodingStr).GetBytes(value);
                request.ContentLength = buffer.Length;
                //写入流信息
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                }
            }
            //定义返回流
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(response.StatusDescription);
                }
                //读取流信息
                using (var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encodingStr)))
                {
                    return sr.ReadToEnd();
                }
            }

        }

        public static CookieCollection GenerateCookies(string cookieStr)
        {
            if (string.IsNullOrEmpty(cookieStr))
            {
                return null;
            }
            //var cookieContainer = new CookieContainer();
            var cookieColllection = new CookieCollection();
            var cookieArray = cookieStr.Split(';');
            foreach (var c in cookieArray)
            {
                if (!string.IsNullOrWhiteSpace(c))
                {
                    var cArray = c.Trim().Split('=');
                    cookieColllection.Add(new Cookie(cArray[0], cArray[1]));
                }
            }
            return cookieColllection;
        }

        public static CookieCollection GenerateCookiesFromUrl(string fromUrl)
        {
            var request = (HttpWebRequest)WebRequest.Create(fromUrl);
            request.Method = "GET";
            request.Timeout = DEFAULT_TIMEOUT;

            var response = (HttpWebResponse)request.GetResponse();

            var orignalCookieStr = response.Headers["Set-Cookie"];
            var cArry = orignalCookieStr.Split(',');

            var cookieCollection = new CookieCollection();
            foreach (var c in cArry)
            {
                if (!string.IsNullOrWhiteSpace(c))
                {
                    var position = c.IndexOf(';');//find firt ';'
                    var cookieitem = position > 0 ? c.Substring(0, position) : c;

                    var cookiePair = cookieitem.Split('=');
                    cookieCollection.Add(new Cookie(cookiePair[0], cookiePair[1]));
                }
            }

            return cookieCollection;
        }

        public static bool IsUrlAvailable(string url, string proxy = null)
        {
            const int urlConnectionTimeout = 2000;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = urlConnectionTimeout;
            try
            {
                if (!string.IsNullOrEmpty(proxy))
                {
                    request.Proxy = new WebProxy(proxy);
                }
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GeneratePostingData(Dictionary<string, string> values)
        {
            var valueList = new List<string>();
            foreach (var pair in values)
            {
                var str = string.Format("{0}={1}", pair.Key, pair.Value);
                valueList.Add(str);
            }

            return string.Join("&", valueList);
        }

        public static WebHeaderCollection GetDefaultHeaders(string encodingStr = null)
        {
            var headers = new WebHeaderCollection
            {
                {"Accept", "*/*"},
                {"Accept-Encoding", "gzip,deflate,sdch"},
                {"Accept-Language", "zh-CN,zh;q=0.8,en;q=0.6,zh-TW;q=0.4"},
                {"User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36"}
            };

            if (!string.IsNullOrEmpty(encodingStr))
            {
                headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=" + encodingStr);
            }
            return headers;
        }

        public static WebHeaderCollection GetDefaultPostHeaders(string encodingStr = null)
        {
            var headers = GetDefaultHeaders(encodingStr);
            headers.Add("X-Requested-With", "XMLHttpRequest");

            return headers;
        }

        #region Private Methods
        private static void AssginHeaer(HttpWebRequest request, WebHeaderCollection headers)
        {
            if (headers == null)
            {
                return;
            }
            foreach (var key in headers.AllKeys)
            {
                switch (key)
                {
                    case "Accept":
                        request.Accept = headers[key];
                        break;
                    case "Connection":
                        request.Connection = headers[key];
                        break;
                    case "Content-Length":
                        request.ContentLength = Convert.ToInt64(headers[key]);
                        break;
                    case "Content-Type":
                        request.ContentType = headers[key];
                        break;
                    //case"Date":
                    //    request.Date = headers[key];
                    //    break;
                    case "Expect":
                        request.Expect = headers[key];
                        break;
                    case "Host":
                        request.Host = headers[key];
                        break;
                    //case "If-Modified-Since":
                    //    request.IfModifiedSince = headers[key];
                    //    break;
                    //case "Range":
                    case "Referer":
                        request.Referer = headers[key];
                        break;
                    case "Transfer-Encoding":
                        request.TransferEncoding = headers[key];
                        break;
                    case "User-Agent":
                        request.UserAgent = headers[key];
                        break;
                    //case "Proxy-Connection":
                    //    request.Proxy = headers[key];
                    //    break;
                    case "Timeout":
                        request.Timeout = Convert.ToInt32(headers[key]);
                        break;
                    case "Proxy":
                        request.Proxy = new WebProxy(headers["Proxy"]);
                        break;
                    default:
                        request.Headers.Add(key, headers[key]);
                        break;
                }
            }
        }

        #endregion

    }
}
