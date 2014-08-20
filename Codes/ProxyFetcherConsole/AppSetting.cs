using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyFetcherConsole
{
    public class AppSetting
    {
        public static string UserName
        {
            get { return ConfigurationManager.AppSettings["UserName"]; }
        }

        public static string Password
        {
            get { return ConfigurationManager.AppSettings["Password"]; }
        }

        public static string Subject
        {
            get { return ConfigurationManager.AppSettings["Subject"]; }
        }
    }
}
