using System;
using ProxyFetcherConsole.Services;

namespace ProxyFetcherConsole
{
    class Program
    {
        static void Main(string[] args)
        {
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
