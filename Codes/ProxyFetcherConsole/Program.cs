using System;
using log4net;
using ProxyFetcherConsole.Services;

namespace ProxyFetcherConsole
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        static void Main(string[] args)
        {
            Log.Error("Test");
            try
            {
                IProxyFetcherService fetcher = new ProxyFetcherService();
                fetcher.FetchProxies();
            }
            catch (Exception)
            {
                //TODO log
            }
        }
    }
}
