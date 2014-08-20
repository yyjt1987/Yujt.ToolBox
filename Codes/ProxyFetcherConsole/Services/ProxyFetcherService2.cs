using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using Yujt.Common.Helper;
using yujt.common.Proxies;
using Yujt.Common.Emails;

namespace ProxyFetcherConsole.Services
{
    //[Export("ProxyFetcherService")]
    public class ProxyFetcherService2 : IProxyFetcherService2
    {
        private readonly string mIpProxyFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), @"Persist\ProxyList.csv");
        private readonly object mLockObj = new object();

        private const string MAIL_USER_NAME = "uosfuid@163.com";
        private const string PASSWORD = "123qaz";

        //[Import]
        private readonly IProxyFetcher mProxyFetcher = new ProxyFetcher();
        private readonly IList<Proxy> mProxies = new List<Proxy>();
        public ProxyFetcherService2()
        {
            //AsynFetchProxies();
        }

        public void FetchProxies()
        {
            var porxiesFromFile = LoadProxiesFromEmail();
            ValidateProxyFromFile(porxiesFromFile);
            EnrichProxiesFromInternet();

            UpdateProxyFile();
        }
        #region Private Methods
        private IEnumerable<Proxy> LoadProxiesFromEmail()
        {
            if (!File.Exists(mIpProxyFilePath))
            {
                FileHelper.CreateFileAndParentDirectory(mIpProxyFilePath);
                return null;
            }
            if (FileHelper.GetFileSize(mIpProxyFilePath) == 0)
            {
                return null;
            }
            try
            {
                var proxyList = new List<Proxy>();
                using (var sr = new StreamReader(mIpProxyFilePath))
                {
                    var reader = new CsvReader(sr);
                    while (reader.Read())
                    {
                        proxyList.Add(reader.GetRecord<Proxy>());
                    }
                }
                return proxyList;
            }
            catch (ProxyInitException)
            {
                File.Delete(mIpProxyFilePath);
                FileHelper.CreateFileAndParentDirectory(mIpProxyFilePath);
                return null;
            }
        }
        private void CopyProxyFileFromEmail()
        {
            IEmail email163 =new Email163(MAIL_USER_NAME, PASSWORD);
            email163.SaveFirstAttachement("", "");
        }

        private void ValidateProxyFromFile(IEnumerable<Proxy> proxiesFromFile)
        {
            if (proxiesFromFile == null)
            {
                return;
            }
            CheckProxies(proxiesFromFile);
        }
        private void EnrichProxiesFromInternet()
        {
            var proxyList = mProxyFetcher.FetchAll();
            CheckProxies(proxyList);
        }
        private void CheckProxies(IEnumerable<Proxy> proxies)
        {
            foreach (var proxy in proxies)
            {
                var count = mProxies.Count(item => item.Host == proxy.Host);
                if (count < 1)
                {
                    long timeSpent;
                    if (IeProxyHelper.IsProxyAvailable(proxy.Host, proxy.Port, out timeSpent))
                    {
                        proxy.TimeSpent = timeSpent;
                        lock (mLockObj)
                        {
                            mProxies.Add(proxy);
                        }
                    }
                }
            }
        }
        private void UpdateProxyFile()
        {
            if (!mIsLoadProxiesFromFileFinished || !mIsLoadProxiesFromInternetFinished)
            {
                return;
            }
            using (var tw = new StreamWriter(mIpProxyFilePath))
            {
                var writer = new CsvWriter(tw);
                writer.WriteRecords(mProxies);
                tw.Flush();
            }
        }
        #endregion
        private class ProxyInitException : Exception
        {
        }
    }
    public interface IProxyFetcherService2
    {
        void FetchProxies();
    }
}