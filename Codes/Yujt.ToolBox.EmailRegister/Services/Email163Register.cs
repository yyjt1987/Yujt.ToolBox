using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Yujt.Common.Helper;

namespace Yujt.ToolBox.EmailRegister.Services
{
    public class Email163Register : IEmailRegister
    {
        private const string REGISTER_PAGE_URL = "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163";
        private const string ENVALUE_PATTERN = "envalue : \"[0-9]*\"";
        private const string SUSPEND_ID_PATTERN = "\"suspendId\" : \"[0-9a-z]*\"";

        private const string DEFAULT_ENCODING = "UTF-8";
        private string mEncode;

        private CookieCollection mHttpCookieCollection;
        private CookieCollection mHttpsCookieCollection;
        private string mSuspendId;
        private string mUid;

        public bool IsEmailNameAvailable(string name)
        {
            try
            {
                var checkNameUrl = "http://reg.email.163.com/unireg/call.do?cmd=urs.checkName";
                var value = string.Format("name={0}", name);
                var cookies = GetHttpCookies();
                var headers = HttpRequestHelper.GetDefaultPostHeaders("UTF-8");
                headers.Add("Accept", "*/*");
                headers.Add("Referer", "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163");

                var response = HttpRequestHelper.Post(checkNameUrl, value, cookies, headers);

                var judgeNameAvailableStr = "\"163.com\":1";
                return response.Contains(judgeNameAvailableStr);
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool Register(string name, string password, string verifyCode, out string secondImageUrl)
        {
            const string registerUrlFormat =
                "https://ssl.mail.163.com/regall/unireg/call.do;jsessionid={0}?cmd=register.start";
            var httpCookies = GetHttpCookies();
            var httpSessionId = httpCookies["jsessionid"];
            var registerUrl = string.Format(registerUrlFormat, httpSessionId.Value);
            var httpsCookies = GetHttpsCookies();

            var headers = HttpRequestHelper.GetDefaultPostHeaders("UTF-8");
            headers.Add("Accept", "*/*");
            headers.Add("Origin", "http://reg.email.163.com");
            headers.Add("Referer", "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163");
            headers.Add("Cache-Control", "no-cache");

            mUid = name + "%40163.com";
            var registerData = new Dictionary<string, string>
            {
                {"name", name},
                {"flow", "main"},
                {"uid", mUid},
                {"password", password},
                {"confirmPassword",password},
                {"mobile", null},
                {"vcode", verifyCode},
                {"from", "163navi"},
            };
            var result = HttpRequestHelper.Post(registerUrl, registerData, httpsCookies, headers);

            if (result.Contains("注册成功"))
            {
                secondImageUrl = null;
                return true;
            }
            if (result.Contains("我们的服务器貌似对您产生了兴趣"))
            {
                secondImageUrl = GetSecondVerifyCodeImagePath();
                InitSuspendId(result);
            }
            else
            {
                secondImageUrl = null;
            }
            return false;
        }

        public bool SecondVerifyPost(string vcode)
        {
            const string secondVerifyUrl = "http://reg.email.163.com/unireg/call.do?cmd=register.resume";
            var httpCookies = GetHttpCookies();

            var headers = HttpRequestHelper.GetDefaultPostHeaders("UTF-8");
            headers.Add("Accept", "*/*");
            headers.Add("Origin", "http://reg.email.163.com");
            headers.Add("Referer", "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163");
            var registerData = new Dictionary<string, string>
            {
                {"uid", mUid},
                {"vcode", vcode},
                {"suspendId", mSuspendId},
            };
            var result = HttpRequestHelper.Post(secondVerifyUrl, registerData, httpCookies, headers);

            if (result.Equals("{\"code\":200}"))
            {
                return true;
            }
            return false;
        }

        public string GetFirstVerifyCodeImagePath()
        {
            var imageUrl = GetVcCodeUrlFromPageContent();
            return GenVerifyCodeImagePath(imageUrl);
        }

        public string GetSecondVerifyCodeImagePath()
        {
            var imageUrl = GetSecondVcCodeUrl();
            return GenVerifyCodeImagePath(imageUrl);
        }

        private string GenVerifyCodeImagePath(string verifyCodeUrl)
        {
            var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Plugins\VerifyCode");
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
            FileHelper.TryDeleteFilesIn(imageDirectory);

            var imagePath = Path.Combine(imageDirectory, Guid.NewGuid() + ".jpg");

            var headers = HttpRequestHelper.GetDefaultHeaders();
            headers.Add("Accept", "image/webp,*/*;q=0.8");
            headers.Add("Referer", "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163");

            try
            {
                using (var response = HttpRequestHelper.GetResponse(verifyCodeUrl, GetHttpCookies(), headers))
                {
                    if (!response.ContentType.Contains("image"))
                    {
                        throw new Exception();
                    }
                    using (Stream inputStream = response.GetResponseStream())
                    using (Stream outputStream = new FileStream(imagePath, FileMode.CreateNew))
                    {
                        byte[] buffer = new byte[4096];
                        int bytesRead;
                        do
                        {
                            bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead != 0);
                    }
                }
                return imagePath;
            }
            catch (Exception)
            {
                return Path.Combine(Path.Combine(Directory.GetCurrentDirectory(),"Plugins/Images/VerifyCodeError.png"));
            }
        }

        private CookieCollection GetHttpCookies()
        {
            if (mHttpCookieCollection != null)
            {
                return mHttpCookieCollection;
            }
            var headers = HttpRequestHelper.GetDefaultHeaders();
            headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");

            mHttpCookieCollection = HttpRequestHelper.GenerateCookiesFromUrl(REGISTER_PAGE_URL, null, headers);

            return mHttpCookieCollection;
        }

        public CookieCollection GetHttpsCookies()
        {
            const string registerHttpsPrepareUrlFormat =
            "https://ssl.mail.163.com/regall/unireg/prepare.jsp?sid={0}&sd={1}";

            var httpCookies = GetHttpCookies();
            if (mHttpsCookieCollection != null)
            {
                return mHttpsCookieCollection;
            }
            var prepareUrl = string.Format(registerHttpsPrepareUrlFormat, httpCookies["jsessionid"].Value, httpCookies["ser_adapter"].Value);

            var headers = HttpRequestHelper.GetDefaultHeaders();
            headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            headers.Add("Referer", "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163");
            
            var cookies = HttpRequestHelper.GenerateCookiesFromUrl(prepareUrl, null, headers);
            mHttpsCookieCollection = new CookieCollection();

            for (int i = 0; i < cookies.Count; i++)
            {
                if (!cookies[i].Secure && cookies[i].Name.ToLower().Equals("jsessionid"))
                {
                    
                }
                else
                {
                    mHttpsCookieCollection.Add(cookies[i]);
                }
            }
            return mHttpsCookieCollection;
        }

        private WebHeaderCollection GetPostHeaders(bool isHttps)
        {
            var headers = HttpRequestHelper.GetDefaultPostHeaders();
            headers.Add("Accept",
                isHttps ? "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" : "*/*");
            headers.Add("Origin", "http://reg.email.163.com");
            headers.Add("Referer", "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163");

            return headers;
        }

        private string GetPageContent()
        {
            var headers = HttpRequestHelper.GetDefaultHeaders("UTF-8");
            headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");

            return HttpRequestHelper.Get(REGISTER_PAGE_URL, GetHttpCookies(), headers);
        }

        public string GetVcCodeUrlFromPageContent()
        {
            const string urlPatter = "http://reg.email.163.com/unireg/call.do?cmd=register.verifyCode&v=common/verifycode/vc_en&env={0}&t={1}";

            if (string.IsNullOrEmpty(mEncode))
            {
                var pageContent = GetPageContent();

                var match = Regex.Match(pageContent, ENVALUE_PATTERN);
                var envalueString = match.Value;

                var firstPosition = envalueString.IndexOf('\"') + 1;
                var length = envalueString.LastIndexOf('\"') - firstPosition;
                var envalue = envalueString.Substring(firstPosition, length);
                mEncode = EnvalueCalc(envalue);
            }
            return string.Format(urlPatter, mEncode, DateTime.Now.GetMillisTime());
        }

        public string GetSecondVcCodeUrl()
        {
            const string urlPattern =
                "http://reg.email.163.com/unireg/call.do?cmd=register.verifyCode&v=common/verifycode/vc_zh&vt=grey&env={0}&t={1}";
            return string.Format(urlPattern, mEncode, DateTime.Now.GetMillisTime());
        }

        private void InitSuspendId(string content)
        {
            if (string.IsNullOrEmpty(mSuspendId))
            {
                var match = Regex.Match(content, SUSPEND_ID_PATTERN);
                var suspendString = match.Value.Split(':')[1];

                var firstPosition = suspendString.IndexOf('\"') + 1;
                var length = suspendString.LastIndexOf('\"') - firstPosition;
                mSuspendId = suspendString.Substring(firstPosition, length);
            }
        }

        public string EnvalueCalc(string envalueStr)
        {
            var j = 10;
            var d = envalueStr;
            //var N = D.Length;
            var y = DateTime.Now.GetMillisTime();
            var x = y % j;
            //var S = (Y - X) / J;
            if (x < 1)
            {
                x = 1;
            }
            x = y % j;
            var w = y % (j * j);
            //S = (Y - W) / J;
            //S = S / J;
            w = (w - x) / j;
            var a = y + "";
            var k = a.ElementAt(j);//A.charAt(J),

            var T = x + "" + w + "" + k;
            var o = Convert.ToInt64(T);// Number(T),
            var v = o * Convert.ToInt64(d);//Number(D),
            var c = v.ToString();//V + "";
            var p = "";
            int m;
            for (m = c.Length - 1; m >= 0; m--)
            {
                var l = c.ElementAt(m);
                p = p + l;
            }
            var r = k + p + w + x;
            var b = r.Length;
            int q;
            var I = "";
            var e = "";
            for (q = 0; q < b; j++)
            {
                I = I + r.ElementAt(q);
                q = q + 2;
            }
            for (q = 1; q < b; q = q + 2)
            {
                e = e + r.ElementAt(q);
            }
            //var F = I + e;
            //F = r;
            var f = r;
            int g;
            var h = "";
            for (g = 0; g < f.Length; g++)
            {
                h = h + f.ElementAt(g);
            }
            return f;
        }
    }
}
