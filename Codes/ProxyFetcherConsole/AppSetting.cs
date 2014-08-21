using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yujt.Common.Encrypts;

namespace ProxyFetcherConsole
{
    public class AppSetting
    {
        public static string UserName
        {
            get { return DesEncyptor.Decrypt(ConfigurationManager.AppSettings["UserName"]); }
        }

        public static string Password
        {
            get { return DesEncyptor.Decrypt(ConfigurationManager.AppSettings["Password"]); }
        }

        public static string Subject
        {
            get { return ConfigurationManager.AppSettings["Subject"]; }
        }
    }
}
