using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using Yujt.Common.Encrypts;

namespace Yujt.Common
{
    public class CommonAppSetting
    {
        public static CommonAppSetting Instatance = new CommonAppSetting();
        private static NameValueCollection mAppSetings;


        private static Configuration mAppConfig;
        public CommonAppSetting()
        {

            var dllPath = Assembly.GetExecutingAssembly().Location;


            var path2 = Directory.GetCurrentDirectory();
            if (File.Exists(dllPath + ".config"))
            {
                var appConfig = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                var section = ConfigurationManager.GetSection("appSettings");
            }
        }
        public string ChineseProxyProvider
        {
            get { return DesEncyptor.Decrypt(mAppConfig.AppSettings.Settings["ChineseProxyProvider"].Value); }
        }

        public string ForeignProxyProvider
        {
            get { return DesEncyptor.Decrypt(ConfigurationManager.AppSettings["ForeignProxyProvider"]); }
        }
    }
}
