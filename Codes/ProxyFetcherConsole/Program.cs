using System;
using log4net;
using ProxyFetcherConsole.Services;
using System.Reflection;
using log4net.Config;

namespace ProxyFetcherConsole
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            try
            {
                IProxyFetcherService fetcher = new ProxyFetcherService();
                fetcher.FetchProxies();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
