using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Yujt.Common.Helper;

namespace yujt.common.Proxies
{
    public class IeProxyHelper
    {
        private const string PROXY_KEY_PATH = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\";
        private const string PROXY_ENABLE = "ProxyEnable";
        private const string PROXY_SERVER = "ProxyServer";
        private const string CHECKING_URL = @"http://www.baidu.com";

        //private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_PROXY_SETTINGS_CHANGED = 95;

        public static void SwitchProxy(string host, string port)
        {
            string proxy = string.Format("{0}:{1}", host, port);
            SwitchProxy(proxy);
        }

        public static void SwitchProxy(string proxy)
        {
            //ResetIeProxy();

            var registryHelper = new RegistryHelper(PROXY_KEY_PATH, true);
            registryHelper.SetStringValue(PROXY_SERVER, proxy);
            registryHelper.SetIntValue(PROXY_ENABLE, 1);
            registryHelper.Close();

            RefreshIeProxySetting();
        }
        public static void DisableIeProxy()
        {
            var registryHelper = new RegistryHelper(PROXY_KEY_PATH, true);
            //registryHelper.SetStringValue(PROXY_SERVER, String.Empty);
            registryHelper.SetIntValue(PROXY_ENABLE, 0);
            registryHelper.Close();

            RefreshIeProxySetting();
        }

        public static void EnableIeProxy()
        {
            var registryHelper = new RegistryHelper(PROXY_KEY_PATH, true);
            //registryHelper.SetStringValue(PROXY_SERVER, String.Empty);
            registryHelper.SetIntValue(PROXY_ENABLE, 1);
            registryHelper.Close();

            RefreshIeProxySetting();
        }

        /// <summary>
        /// Proxy format 127.0.0.1:80
        /// </summary>
        public static bool IsProxyAvailable(string host, string port)
        {
            var proxy = string.Format("{0}:{1}", host, port);
            return HttpRequestHelper.IsUrlAvailable(CHECKING_URL, proxy);

        }

        public static bool IsProxyAvailable(string host, string port, out long timeSpan)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = IsProxyAvailable(host, port);
            stopwatch.Stop();
            timeSpan = stopwatch.ElapsedMilliseconds;
            return result;
        }


        #region private methods

        private static void RefreshIeProxySetting()
        {
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY_SETTINGS_CHANGED, IntPtr.Zero, 0);
        }

        #endregion

        #region Win API

        [DllImport("wininet.dll", EntryPoint = "InternetSetOptionA", SetLastError = true, PreserveSig = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        #endregion
    }
}
