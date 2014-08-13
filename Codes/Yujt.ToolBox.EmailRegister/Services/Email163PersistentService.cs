using System.Collections.Generic;
using System.IO;
using System.Net;
using CsvHelper;
using CsvHelper.Configuration;
using Yujt.Common.Helper;
using Yujt.ToolBox.EmailRegister.ViewModels;

namespace Yujt.ToolBox.EmailRegister.Services
{
    public class Email163PersistentService : IEmailPersistentService
    {
        private readonly string mEmailListFile = Path.Combine(Directory.GetCurrentDirectory(), @"Persist\Email.csv");
        private readonly CsvConfiguration mCsvConfiguration = new CsvConfiguration {QuoteAllFields = true};
        private bool mFirstAddEmail;
        public void Save(Email email)
        {
            if (!File.Exists(mEmailListFile))
            {
                FileHelper.CreateFileAndParentDirectory(mEmailListFile);
                mFirstAddEmail = true;
            }

            if (FileHelper.GetFileSize(mEmailListFile) == 0)
            {
                mFirstAddEmail = true;
            }

            using (var sw = new StreamWriter(mEmailListFile))
            {
                var writer = new CsvWriter(sw, mCsvConfiguration);
                if (mFirstAddEmail)
                {
                    var list = new List<Email> {email};
                    writer.WriteRecords(list);
                }
                else
                {
                    writer.WriteRecord(email);
                }
            }
        }

        public IList<Email> GetEmaiList()
        {
            var emailList = new List<Email>();
            if (!File.Exists(mEmailListFile))
            {
                return emailList;
            }

            using (var sr = new StreamReader(mEmailListFile))
            {
                var reader = new CsvReader(sr, mCsvConfiguration);
                while (reader.Read())
                {
                    var email = reader.GetRecord<Email>();
                    emailList.Add(email);
                }
                return emailList;
            }
        }
    }
}
