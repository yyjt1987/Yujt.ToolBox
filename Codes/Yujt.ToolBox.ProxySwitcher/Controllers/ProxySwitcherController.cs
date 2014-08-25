using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using Yujt.Common.Proxies;
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


        private readonly Dispatcher mDispatcher;

        public ProxySwitcherController()
        {
            mProxySwitcherViewModel.Proxies = new ObservableCollection<Proxy>(ProxyRepository.GetProxies());
            mProxySwitcherControl.DataContext = mProxySwitcherViewModel;

            mDispatcher = mProxySwitcherControl.Dispatcher;
        }


        public UserControl GetUserControl()
        {

            return mProxySwitcherControl;
        }
    }
}
