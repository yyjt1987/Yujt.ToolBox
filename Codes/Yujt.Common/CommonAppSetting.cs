using System.ComponentModel;
using System.Configuration;
using Yujt.Common.Encrypts;

namespace Yujt.Common
{
    public class CommonAppSetting
    {
        public static CommonAppSetting Instatance = new CommonAppSetting();

        public string ChineseProxyProvider
        {
            get { return DesEncyptor.Decrypt(ConfigurationManager.AppSettings["ChineseProxyProvider"]); }
        }

        public string ForeignProxyProvider {
            get { return DesEncyptor.Decrypt(ConfigurationManager.AppSettings["ForeignProxyProvider"]); }
        }
    }
}
