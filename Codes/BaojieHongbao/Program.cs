using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Yujt.Common.Helper;

namespace BaojieHongbao
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var obj1 = new BaojieObj
            {
                OpenId = "ocJOVjunIX3BkEPi-L566gjdg8oE",
                Referer =
                    "Referer: http://app.weibopie.com/wxapp/LivingartistApp/www/index.php?a=CRFRedHire&m=showCode&MONITOR_DATA=54%3B546%3B1%3BocJOVjunIX3BkEPi-L566gjdg8oE%3B2%3B9%3B0%3B0%3B0%3B1%3B4%3B6%3B2%3Bscan&openid=ocJOVjunIX3BkEPi-L566gjdg8oE&apiKey=8a674fcd013ccbc5e0fc206d3b1771c4&timestamp=1406073104&sig=8dc8ca793ff31aa952c40f0447ca9dba&MFROM=msg&OAUTHED=0&FACK_ID=O_140723075144264"
            };
            var obj2 = new BaojieObj
            {
                OpenId = "ocJOVjs1avvXyOMIQXmzZyqiG1Kg",
                Referer =
                    "Referer: http://app.weibopie.com/wxapp/LivingartistApp/www/index.php?a=CRFRedHire&m=showCode&MONITOR_DATA=54%3B546%3B2%3BocJOVjs1avvXyOMIQXmzZyqiG1Kg%3B3%3B6736%3B2634%3B0%3B0%3B1%3B0%3B0%3B3%3B&openid=ocJOVjs1avvXyOMIQXmzZyqiG1Kg&apiKey=8a674fcd013ccbc5e0fc206d3b1771c4&timestamp=1406211085&sig=bcc57124cf011ae4633064c122a6ac97&MFROM=msg&OAUTHED=0&FACK_ID=O_140724221125278"
            };

            ThreadHelper.ThreadCount(2).Run(BaoJieExecute, obj1);
            ThreadHelper.ThreadCount(2).Run(BaoJieExecute, obj2);

            Console.Read();
        }

        public static void BaoJieExecute(object obj)
        {
            LoopHelper.Count(10).Execute(
                () =>
                {
                    var baojieObj = (BaojieObj)obj;

                    var url = "http://app.weibopie.com/wxapp/LivingartistApp/www/index.php?a=CRFRedHire&m=sendWhich";
                    var cookieStr = "BIGipServerpool_qiye=1510320320.20480.0000";

                    var valueDict = new Dictionary<string, string>
                    {
                        {"openid", baojieObj.OpenId},
                        {"passCode", "8096"}
                    };
                    //"ocJOVjs1avvXyOMIQXmzZyqiG1Kg"
                    var headerList = new List<string>
                    {
                        "Origin: http://app.weibopie.com",
                        baojieObj.Referer,
                        "Content-Type: application/x-www-form-urlencoded; charset=UTF-8"
                    };

                    var response = WxHttpRequestHelper.Post(url, valueDict, headerList, cookieStr);
                    Console.WriteLine(response);

                    LoopHelper.Count(2).Execute(() =>
                    {
                        var getUrl =
                            "http://app.weibopie.com/wxapp/LivingartistApp/www/index.php?a=CRFRedHire&m=scoreNotice&openid=" +
                            baojieObj.OpenId;

                        var headerList2 = new List<string>
                        {
                            "Origin: http://app.weibopie.com",
                            baojieObj.Referer,
                            "Content-Type: application/x-www-form-urlencoded; charset=UTF-8"
                        };

                        var response2 = WxHttpRequestHelper.Get(getUrl, cookieStr, headerList2);
                        Console.WriteLine(response2);
                        Thread.Sleep(700);
                    });
                }
            );

        }

        public class BaojieObj
        {
            public string OpenId { get; set; }

            public string Referer { get; set; }
        }
    }
}
