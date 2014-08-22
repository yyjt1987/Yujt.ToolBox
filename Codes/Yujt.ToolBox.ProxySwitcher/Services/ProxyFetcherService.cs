using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using Yujt.Common.Helper;
using yujt.common.Proxies;
namespace Yujt.ToolBox.ProxySwitcher.Services
{
    //[Export("ProxyFetcherService")]
    public class ProxyFetcherService : IProxyFetcherService
    {
        public IList<Proxy> GetProxies()
        {
            
        }
    }
    public interface IProxyFetcherService
    {
        //Proxy GetSingleproxy(int index);
    }
}