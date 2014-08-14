using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using Yujt.Common.Helper;
using yujt.common.Proxies;

namespace ProxyFetcherConsole.Services
{
    //[Export("ProxyFetcherService")]
    public class ProxyFetcherService2 : IProxyFetcherService2
    {
        private readonly string mIpProxyFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), @"Persist\ProxyList.csv");
        private readonly object mLockObj = new object();
        private bool mIsLoadProxiesFromFileFinished;
        private bool mIsLoadProxiesFromInternetFinished;
        //[Import]
        private readonly IProxyFetcher mProxyFetcher = new ProxyFetcher();
        private readonly IList<Proxy> mProxies = new List<Proxy>();
        public ProxyFetcherService2()
        {
            //AsynFetchProxies();
        }
        public Proxy GetSingleproxy(int index)
        {
            while (mProxies.Count < 1 || index >= mProxies.Count)
            {
                if (mIsLoadProxiesFromFileFinished && mIsLoadProxiesFromInternetFinished)
                {
                    return null;
                }
                Thread.Sleep(200);
            }
            return mProxies[index];
        }

        public void AsynFetchProxies()
        {
            var porxiesFromFile = LoadProxiesFromEmail();
            ValidateProxyFromFile(porxiesFromFile);
            EnrichProxiesFromInternet();
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
        private void ValidateProxyFromFile(IEnumerable<Proxy> proxiesFromFile)
        {
            var thread = new Thread(() =>
            {
                if (proxiesFromFile == null)
                {
                    return;
                }
                CheckProxies(proxiesFromFile);

                mIsLoadProxiesFromFileFinished = true;
                UpdateProxyFile();
            });
            thread.IsBackground = true;
            thread.Start();
        }
        private void EnrichProxiesFromInternet()
        {
            var thread = new Thread(() =>
            {
                var proxyList = mProxyFetcher.FetchAll();
                CheckProxies(proxyList);
                mIsLoadProxiesFromInternetFinished = true;
                UpdateProxyFile();
            });
            thread.IsBackground = true;
            thread.Start();
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
                        OnNewProxyFundEvent(proxy);
                    }
                }
            }//Validate all proxy from internet
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
        void AsynFetchProxies();
        Proxy GetSingleproxy(int index);
    }
}