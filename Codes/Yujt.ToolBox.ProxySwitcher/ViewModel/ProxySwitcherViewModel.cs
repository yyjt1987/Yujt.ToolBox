using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Yujt.Common.Helper;
using Yujt.Common.Proxies;
using Yujt.ToolBox.Common;
using Yujt.ToolBox.Common.Commands;
using Yujt.ToolBox.ProxySwitcher.Annotations;
using Yujt.ToolBox.ProxySwitcher.Services;

namespace Yujt.ToolBox.ProxySwitcher.ViewModel
{
    public class ProxySwitcherViewModel : INotifyPropertyChanged
    {
        private readonly LocalIpService mService = new LocalIpService();
        public ProxySwitcherViewModel()
        {
            LocalIpInfo = mService.GetLocalIpInfo();
        }

        private string mLocalIpInfo;
        public string LocalIpInfo 
        {
            get { return mLocalIpInfo; }
            set
            {
                mLocalIpInfo = value;
                OnPropertyChanged("LocalIpInfo");
            }
        }

        public ObservableCollection<Proxy> Proxies { get; set; }

        public Proxy SelectedProxy { get; set; }

        public ICommand SwitchCommand
        {
            get { return new DelegatingCommand(SwitchProxy); }
        }

        public ICommand CheckCommand
        {
            get { return new DelegatingCommand(CheckProxy); }
        }

        public ICommand CopyCommand
        {
            get { return new DelegatingCommand(CopyProxy); }
        }

        public ICommand ResetCommand
        {
            get { return new DelegatingCommand(ResetProxy); }
        }

        private void CopyProxy()
        {
            var proxy = string.Format("{0}:{1}", SelectedProxy.Host, SelectedProxy.Port);
            //Clipboard.SetText(proxy);
            Clipboard.SetDataObject(proxy, false); 
        }

        private void SwitchProxy()
        {
            IeProxyHelper.SwitchProxy(SelectedProxy.Host, SelectedProxy.Port);
            LocalIpInfo = mService.GetLocalIpInfo();
        }

        private void ResetProxy()
        {
            IeProxyHelper.DisableIeProxy();
            LocalIpInfo = mService.GetLocalIpInfo();
            IeProxyHelper.EnableIeProxy();
        }

        private void CheckProxy()
        {
            if (IeProxyHelper.IsProxyAvailable(SelectedProxy.Host, SelectedProxy.Port))
            {
                MessageBox.Show("目标服务器连接可用！");
            }
            else
            {
                MessageBox.Show("目标服务器连接不可用！");
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
