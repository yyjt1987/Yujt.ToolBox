using System.Collections.Generic;

namespace Yujt.Common.Proxies
{
    public interface IProxyFetcher
    {
        List<Proxy> FetchChineseProxy();

        List<Proxy> FetchForeignProxy();

        List<Proxy> FetchAll();
    }
}
