using System;
using System.Collections.Generic;
using NUnit.Framework;
using Yujt.Common.Helper;

namespace Yujt.ToolBox.Test
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void WeiXinTest()
        {
            const string url = @"http://amx.3g.qq.com/api/draw";
            const string cookieStr = "641010895_check_register=cWQGpVJnkdzd8jyzfYlmsPALODwO26iPir1JnhORqug%3D; 641010895_check_auth=cWQGpVJnkdzd8jyzfYlmsPALODwO26iPir1JnhORqug%3D; 641010895_openid=cWQGpVJnkdzd8jyzfYlmsPALODwO26iPir1JnhORqug%3D";
            var response = WxHttpRequestHelper.Get(url, cookieStr);
            Assert.IsNotNull(response);
        }

        [Test]
        public void BaoJieTest()
        {
            var url = "http://app.weibopie.com/wxapp/LivingartistApp/www/index.php?a=CRFRedHire&m=sendWhich";
            var cookieStr = "BIGipServerpool_qiye=1510320320.20480.0000";

            var valueDict = new Dictionary<string, string>
            {
                {"openid", "ocJOVjunIX3BkEPi-L566gjdg8oE"},
                {"passCode", "8088"}
            };
            //"ocJOVjs1avvXyOMIQXmzZyqiG1Kg"
            var headerList = new List<string> 
            {
                "Origin: http://app.weibopie.com", 
                "Referer: http://app.weibopie.com/wxapp/LivingartistApp/www/index.php?a=CRFRedHire&m=showCode&MONITOR_DATA=54%3B546%3B1%3BocJOVjunIX3BkEPi-L566gjdg8oE%3B2%3B9%3B0%3B0%3B0%3B1%3B4%3B6%3B2%3Bscan&openid=ocJOVjunIX3BkEPi-L566gjdg8oE&apiKey=8a674fcd013ccbc5e0fc206d3b1771c4&timestamp=1406073104&sig=8dc8ca793ff31aa952c40f0447ca9dba&MFROM=msg&OAUTHED=0&FACK_ID=O_140723075144264",
                "Content-Type: application/x-www-form-urlencoded; charset=UTF-8"
            };

            var response = WxHttpRequestHelper.Post(url, valueDict, headerList,cookieStr);

            Assert.IsNotNull(response);
        }


        [Test]
        public void DateTimeTest()
        {
            var now = DateTime.Now.Ticks;
        }
    }
}
