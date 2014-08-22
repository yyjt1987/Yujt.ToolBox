using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Reflection;
using Yujt.Common.Encrypts;

namespace Yujt.Common
{
    public class CommonAppSetting
    {
        public static CommonAppSetting Instatance = new CommonAppSetting();

        private static object mAppSetting;
        public CommonAppSetting()
        {

            var dllPath = Assembly.GetExecutingAssembly().Location;

            if (File.Exists(dllPath + ".config"))
            {
                var appConfig = ConfigurationManager.OpenExeConfiguration(dllPath);
                mAppSetting = appConfig.AppSettings.Settings;
            }
            else
            {
                mAppSetting = ConfigurationManager.AppSettings;
            }
        }
        public string ChineseProxyProvider
        {
            get { return DesEncyptor.Decrypt(GetSetting("ChineseProxyProvider")); }
        }

        public string ForeignProxyProvider
        {
            get { return DesEncyptor.Decrypt(GetSetting("ForeignProxyProvider")); }
        }

        public string UserName
        {
            get { return DesEncyptor.Decrypt(GetSetting("UserName")); }
        }

        public string Password
        {
            get { return DesEncyptor.Decrypt(GetSetting("Password")); }
        }

        public string Subject
        {
            get { return GetSetting("Subject"); }
        }


        private string GetSetting(string key)
        {
            if (mAppSetting is KeyValueConfigurationCollection)
            {
                return ((KeyValueConfigurationCollection)mAppSetting)[key].Value;
                
            }
            return ((NameValueCollection)mAppSetting)[key];
        }
    }
}
