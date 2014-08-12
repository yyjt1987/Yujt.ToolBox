using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Yujt.Common.Helper;

namespace Yujt.ToolBox.EmailRegister.Services
{
    public class Email163Register : IEmailRegister
    {
        private const string REGISTER_PAGE_URL = "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163";
        private const string DEFAULT_ENCODING = "UTF-8";

        private string mEncode;

        private CookieCollection mCookieCollection;
        
        public bool IsEmailNameAvailable(string name)
        {
            try
            {
                var checkNameUrl = "http://reg.email.163.com/unireg/call.do?cmd=urs.checkName";
                var value = string.Format("name={0}", name);
                var cookies = GetCookies();
                var headers = GetPostHeaders();

                var response = HttpRequestHelper.Post(checkNameUrl, value, cookies, headers);

                var judgeNameAvailableStr = "\"163.com\":1";
                return response.Contains(judgeNameAvailableStr);
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public bool Register(string name, string password, string verifyCode)
        {
            var registerUrlFormat =
                "https://ssl.mail.163.com/regall/unireg/call.do;jsessionid={0}?cmd=register.start";
            var cookies = GetCookies();
            var sessionId = cookies["jsessionid"];
            var registerUrl = string.Format(registerUrlFormat, sessionId);
            var headers = GetPostHeaders();

            var registerData = new Dictionary<string, string>
            {
                {"name", name},
                {"flow", "main"},
                {"uid",name+"@163.com"},
                {"password", password},
                {"confirmPassword",password},
                {"mobile", string.Empty},
                {"vcode", verifyCode},
                {"from", "163navi"},
            };
            var result = HttpRequestHelper.Post(registerUrl, registerData, cookies, headers);


            return false;
        }

        public string GetVerifyCodeImagePath()
        {
            var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Plugins\VerifyCode");
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
            FileHelper.TryDeleteFilesIn(imageDirectory);

            var imagePath = Path.Combine(imageDirectory, Guid.NewGuid() + ".jpg");

            var verifyCodeUrlFormat =
                "http://reg.email.163.com/unireg/call.do?cmd=register.verifyCode&v=common/verifycode/vc_en&env={0}&t={1}";

            var verifyCodeUrl = string.Format(verifyCodeUrlFormat,
                                              RandomHelper.GetRandomNumber(12),
                                              RandomHelper.GetRandomNumber(13));

            var headers = HttpRequestHelper.GetDefaultHeaders();
            headers.Add("Accept", "image/webp,*/*;q=0.8");
            try
            {
                using (var response = HttpRequestHelper.GetResponse(verifyCodeUrl, GetCookies(), headers))
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

        private CookieCollection GetCookies()
        {
            if (mCookieCollection != null)
            {
                return mCookieCollection;
            }
            mCookieCollection = HttpRequestHelper.GenerateCookiesFromUrl(REGISTER_PAGE_URL);
            return mCookieCollection;
        }

        private WebHeaderCollection GetPostHeaders()
        {
            var headers = HttpRequestHelper.GetDefaultPostHeaders(DEFAULT_ENCODING);
            headers.Add("Origin", "http://reg.email.163.com");
            headers.Add("Referer", "http://reg.email.163.com/unireg/call.do?cmd=register.entrance&from=163navi&regPage=163");
            return headers;
        }

        private string GetPageContent()
        {
            var headers = HttpRequestHelper.GetDefaultHeaders();
            headers.Add("Accept", "image/webp,*/*;q=0.8");
            return HttpRequestHelper.Get(REGISTER_PAGE_URL, GetCookies(), headers);
        }


        public string GetUrl()
        {
            const string envaluePattern = "envalue : \"[0-9]*\"";
            const string urlPatter = "{0},{1}";

            if (string.IsNullOrEmpty(mEncode))
            {
                var pageContent = GetPageContent();

                var match = Regex.Match(pageContent, envaluePattern);
                var envalueString = match.NextMatch().Value;

                var firstPosition = envalueString.IndexOf('\"') + 1;
                var length = envalueString.LastIndexOf('\"') - firstPosition + 1;
                var envalue = envalueString.Substring(firstPosition + 1, length);
                mEncode = EnvalueCalc(envalue);
            }
            return string.Format(urlPatter, EnvalueCalc(mEncode), DateTime.Now.GetMillisTime());
        }

        public string EnvalueCalc(string envalueStr)
        {
            var J = 10;
            var D = envalueStr;
            var N = D.Length;
            var Y = DateTime.Now.GetMillisTime();
            var X = Y % J;
            var S = (Y - X) / J;
            if (X < 1)
            {
                X = 1;
            }
            X = Y % J;
            var W = Y % (J * J);
            S = (Y - W) / J;
            S = S / J;
            W = (W - X) / J;
            var A = Y + "";
            var K = A.ElementAt(J);//A.charAt(J),

            var T = X + "" + W + "" + K;
            var O = Convert.ToInt64(T);// Number(T),
            var V = O * Convert.ToInt64(D);//Number(D),
            var C = V.ToString();//V + "";
            var P = "";
            int M;
            for (M = C.Length - 1; M >= 0; M--)
            {
                var L = C.ElementAt(M);
                P = P + L;
            }
            var R = K + P + W + X;
            var B = R.Length;
            int Q;
            var I = "";
            var E = "";
            for (Q = 0; Q < B; J++)
            {
                I = I + R.ElementAt(Q);
                Q = Q + 2;
            }
            for (Q = 1; Q < B; Q = Q + 2)
            {
                E = E + R.ElementAt(Q);
            }
            var F = I + E;
            F = R;
            var G = 0;
            var H = "";
            for (G = 0; G < F.Length; G++)
            {
                H = H + F.ElementAt(G);
            }
            return F;
        }
        
    }
}
