using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Yujt.Common.Helper;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj1 = new HaoShengYinObj
            {
                Url = @"http://amx.3g.qq.com/api/draw",
                CookieStr =
                    "641010895_check_register=cWQGpVJnkdzd8jyzfYlmsPALODwO26iPir1JnhORqug%3D; 641010895_check_auth=cWQGpVJnkdzd8jyzfYlmsPALODwO26iPir1JnhORqug%3D; 641010895_openid=cWQGpVJnkdzd8jyzfYlmsPALODwO26iPir1JnhORqug%3D"
            };
            var thread1 = new Thread(Execute);
            thread1.Start(obj1);
            var thread2 = new Thread(Execute);
            thread2.Start(obj1);

            var obj2 = new HaoShengYinObj
            {
                Url = @"http://amx.3g.qq.com/api/draw",
                CookieStr =
                    "641010895_check_register=o9NS9Z3J8NdGBdpZbpLB5JHf%2FunNBDezAFIjXi553Xg%3D; 641010895_check_auth=o9NS9Z3J8NdGBdpZbpLB5JHf%2FunNBDezAFIjXi553Xg%3D; 641010895_openid=o9NS9Z3J8NdGBdpZbpLB5JHf%2FunNBDezAFIjXi553Xg%3D"
            };

            ThreadHelper.ThreadCount(2).Run(Execute, obj1);
            ThreadHelper.ThreadCount(2).Run(Execute, obj2);

            Console.Read();
        }

        public static void Execute(object obj)
        {
            var haoshenyin = (HaoShengYinObj) obj;
            LoopHelper.Count(50).Execute(() =>
            {
                var response = WxHttpRequestHelper.Get(haoshenyin.Url, haoshenyin.CookieStr);
                Console.WriteLine(response);
                Thread.Sleep(700);
            });
        }

    }

    class HaoShengYinObj
    {
        public string Url{ get; set; }
        public string CookieStr { get; set; }
    }
}
