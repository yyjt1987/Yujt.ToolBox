using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using CsvHelper;
using Yujt.Common.Helper;
using yujt.common.Proxies;
using Yujt.Common.Emails;

namespace ProxyFetcherConsole.Services
{
    //[Export("ProxyFetcherService")]
    public class ProxyFetcherService : IProxyFetcherService
    {
        private readonly IEmail mEmail163;
        private string mLocalProxiesPath = FileHelper.GenFileNameInAssemblyDir("Proxies/ProxyList.csv");

        private readonly IProxyFetcher mProxyFetcher = new ProxyFetcher();
        private readonly IList<Proxy> mProxies = new List<Proxy>();
        public ProxyFetcherService()
        {
            mEmail163 = new Email163(AppSetting.UserName, AppSetting.Password);
        }

        public void FetchProxies()
        {
            var porxiesFromEmail = LoadProxiesFromEmail();
            ValidateProxyFromEmail(porxiesFromEmail);
            EnrichProxiesFromInternet();

            UpdateProxyToEmail();
        }
        #region Private Methods
        private IEnumerable<Proxy> LoadProxiesFromEmail()
        {
            #region Fetch Proxy File
            var path = Path.GetTempFileName();
            try
            {
                mEmail163.SaveFirstAttachement(AppSetting.Subject, path);
            }
            catch (Exception)
            {
                path = FileHelper.GenFileNameInAssemblyDir(mLocalProxiesPath);
            }

            if (!File.Exists(path) || FileHelper.GetFileSize(path) <= 0)
            {
                return null;
            }
            #endregion Fetch Proxy File

            try
            {
                var proxyList = new List<Proxy>();
                using (var sr = new StreamReader(path))
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
                //TODO: Log
                return null;
            }
        }

        private void ValidateProxyFromEmail(IEnumerable<Proxy> proxiesFromEmail)
        {
            if (proxiesFromEmail == null)
            {
                return;
            }
            CheckProxies(proxiesFromEmail);
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
                        mProxies.Add(proxy);
                    }
                }
            }
        }

        private void UpdateProxyToEmail()
        {
            if (File.Exists(mLocalProxiesPath))
            {
                var bakupFilePath = mLocalProxiesPath +"_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                File.Move(mLocalProxiesPath, bakupFilePath);
            }

            using (var tw = new StreamWriter(mLocalProxiesPath, false, Encoding.UTF8))
            {
                var writer = new CsvWriter(tw);
                writer.WriteRecords(mProxies);
                tw.Flush();
            }

            var sendMsg = new MailMessage(AppSetting.UserName, AppSetting.UserName)
            {
                Subject = AppSetting.Subject,
                Body = AppSetting.Subject
            };
            sendMsg.Attachments.Add(new Attachment(mLocalProxiesPath));

            mEmail163.Send(sendMsg);

        }
        #endregion
        private class ProxyInitException : Exception
        {
        }
    }
    public interface IProxyFetcherService
    {
        void FetchProxies();
    }
}