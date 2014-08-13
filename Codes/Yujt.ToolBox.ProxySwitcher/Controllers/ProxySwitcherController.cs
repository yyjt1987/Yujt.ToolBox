using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using yujt.common.Proxies;
using Yujt.ToolBox.Common.Plugable;
using Yujt.ToolBox.ProxySwitcher.Services;
using Yujt.ToolBox.ProxySwitcher.ViewModel;
using Yujt.ToolBox.ProxySwitcher.Views;

namespace Yujt.ToolBox.ProxySwitcher.Controllers
{
    [Export("ProxySwitcherController")]
    public class ProxySwitcherController : IController
    {
        private readonly ProxySwitcherViewModel mProxySwitcherViewModel = new ProxySwitcherViewModel();

        private readonly ProxySwitcherControl mProxySwitcherControl = new ProxySwitcherControl();

        private readonly IProxyFetcherService mProxyFetcherService = new ProxyFetcherService();

        private readonly Dispatcher mDispatcher;

        public ProxySwitcherController()
        {
            mProxySwitcherViewModel.Proxies = new ObservableCollection<Proxy>();
            mProxySwitcherControl.DataContext = mProxySwitcherViewModel;

            mDispatcher = mProxySwitcherControl.Dispatcher;

            mProxyFetcherService.NewProxyFundEvent += AddNewProxy;
            //mProxyFetcherService.AsynFetchProxies();
        }

        private void AddNewProxy(object sender, EventArgs e)
        {
            mDispatcher.Invoke(new Action(() =>
            {
                var proxy = (Proxy) sender;
                var existedProxies =
                    mProxySwitcherViewModel.Proxies.Where(p => p.Host.Equals(proxy.Host) && p.Port.Equals(proxy.Port)).ToArray();
                if (existedProxies.Length <= 0)
                {
                    mProxySwitcherViewModel.Proxies.Add((Proxy) sender);
                }
            }));
        }

        public UserControl GetUserControl()
        {

            return mProxySwitcherControl;
        }
    }
}
