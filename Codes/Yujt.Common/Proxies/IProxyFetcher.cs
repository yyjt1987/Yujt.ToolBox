using System.Collections.Generic;

namespace yujt.common.Proxies
{
    public interface IProxyFetcher
    {
        List<Proxy> FetchChineseProxy();

        List<Proxy> FetchForeignProxy();

        List<Proxy> FetchAll();
    }
}
