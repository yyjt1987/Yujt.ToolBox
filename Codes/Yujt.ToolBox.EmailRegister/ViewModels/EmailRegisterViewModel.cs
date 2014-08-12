using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using Yujt.Common.Helper;
using Yujt.ToolBox.Common.Commands;
using Yujt.ToolBox.EmailRegister.Annotations;
using Yujt.ToolBox.EmailRegister.Services;

namespace Yujt.ToolBox.EmailRegister.ViewModels
{
    public class EmailRegisterViewModel : INotifyPropertyChanged
    {
        private IEmailRegister m163EmailRegister = new Email163Register();

        private event EventHandler RegisterCompleted;

        public EmailRegisterViewModel()
        {
            Password = "123qaz";
            SwitchIpProxyFrequency = 8;
            RegisterCompleted += CompleteCallback;
            RefreshRandomName();
            RefreshImage();
        }

        public RegisteredEmailCollection RegisteredEeails { get; set; }

        public Email SelectedEmail { get; set; }

        private string mName;
        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                OnPropertyChanged("Name");
            }
        }

        private string mPassword;
        public string Password {
            get { return mPassword; }
            set
            {
                mPassword = value;
                OnPropertyChanged("Password");
            }
        }

        private string mVerifyCode;
        public string VerifyCode
        {
            get { return mVerifyCode; }
            set
            {
                mVerifyCode = value;
                OnPropertyChanged("VerifyCode");
            }
        }

        private string mVerifyCodeImagePath;
        public string VerifyCodeImagePath
        {
            get { return mVerifyCodeImagePath; }
            set
            {
                mVerifyCodeImagePath = value;
                OnPropertyChanged("VerifyCodeImagePath");
            }
        }

        private int mSwitchIpProxyFrequency;
        public int SwitchIpProxyFrequency
        {
            get { return mSwitchIpProxyFrequency; }
            set
            {
                mSwitchIpProxyFrequency = value;
                OnPropertyChanged("SwitchIpProxyFrequency");
            }
        }

        #region Commands

        public DelegatingCommand CopyNameCommand
        {
            get { return new DelegatingCommand(CopyName); }
        }

        public DelegatingCommand RefreshImageCommand
        {
            get { return new DelegatingCommand(RefreshImage);}
        }

        public DelegatingCommand RegisterEmailCommand
        {
            get { return new DelegatingCommand(RegisterEmail); }
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

        #endregion

        #region Private Methods
        private void RefreshImage()
        {
            VerifyCodeImagePath = m163EmailRegister.GetVerifyCodeImagePath();
        }

        private void CopyName()
        {
            Clipboard.SetDataObject(Name, false);
        }

        private void RegisterEmail()
        {
            if (string.IsNullOrEmpty(VerifyCode) || string.IsNullOrWhiteSpace(VerifyCode))
            {
                MessageBox.Show("请输入验证码");
                return;
            }
            m163EmailRegister.Register(Name, Password, VerifyCode);
            RefreshRandomName();
            RefreshImage();
        }

        private void RefreshRandomName()
        {
            var tempRandomName = RandomHelper.GetRandomString(8);
            while (!m163EmailRegister.IsEmailNameAvailable(tempRandomName))
            {
                tempRandomName = RandomHelper.GetRandomString(8);
            }
            Name = tempRandomName;
        }

        private void CompleteCallback(object sender, EventArgs e)
        {
            m163EmailRegister = new Email163Register();
            RefreshImage();
            RefreshRandomName();
        }

        protected virtual void OnRegisterCompleted()
        {
            if (RegisterCompleted != null) RegisterCompleted(this, EventArgs.Empty);
        }


        private void SaveEmail(string name, string password)
        {
            var config = new CsvConfiguration {QuoteAllFields = true};
            var tw = new StreamWriter(@"C:\a.csv");
            var writer = new CsvWriter(tw, config);

            writer.WriteRecord(new Email
            {
                Name = name + "@163.com", 
                Password = password
            });
            tw.Flush();
            tw.Close();

        }

        #endregion
    }
}
