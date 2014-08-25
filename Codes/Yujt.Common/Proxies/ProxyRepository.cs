using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using log4net;
using Yujt.Common.Emails;
using Yujt.Common.Helper;

namespace Yujt.Common.Proxies
{
    public class ProxyRepository
    {
        private static readonly IList<Proxy> mProxies = new List<Proxy>();
        private static readonly ILog Log = LogManager.GetLogger(typeof (ProxyRepository));
        private static readonly Int32 mIndex;

        public static IList<Proxy> GetProxies()
        {
            if (mProxies.Count <= 0)
            {
                InitProxy();
            }
            return mProxies;
        }

        public static Proxy GetProxy()
        {
            if (mProxies.Count <= 0)
            {
                InitProxy();
            }
            if (mIndex < mProxies.Count)
            {
                return mProxies[mIndex];
            }
            return null;
        }


        private static void InitProxy()
        {
            IEmail email163= new Email163(CommonAppSetting.Instatance.UserName,
                                          CommonAppSetting.Instatance.Password);

            #region Fetch Proxy File
            var path = Path.GetTempFileName();
            try
            {
                email163.SaveFirstAttachement(CommonAppSetting.Instatance.Subject, path);
            }
            catch (Exception )
            {
                Log.Warn("No Proxy File can be fund");
                return;
            }

            if (!File.Exists(path) || FileHelper.GetFileSize(path) <= 0)
            {
                return;
            }
            #endregion Fetch Proxy File

            try
            {
                using (var sr = new StreamReader(path))
                {
                    var reader = new CsvReader(sr);
                    while (reader.Read())
                    {
                        mProxies.Add(reader.GetRecord<Proxy>());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warn("The proxy list file format is incorrect!");
                Log.Error(ex);
            }
        }
    }
}
