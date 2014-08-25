using System;
using System.ComponentModel;
using System.Windows;
using Yujt.Common.Helper;
using Yujt.ToolBox.Common.Commands;
using Yujt.ToolBox.EmailRegister.Annotations;
using Yujt.ToolBox.EmailRegister.Services;

namespace Yujt.ToolBox.EmailRegister.ViewModels
{
    public class EmailRegisterViewModel : INotifyPropertyChanged
    {
        private IEmailRegister m163EmailRegister = new Email163Register();
        private readonly IEmailPersistentService mEmailPersistentService = new Email163PersistentService();
        private event EventHandler CompleteRegisterEvent;

        public EmailRegisterViewModel()
        {
            CompleteRegisterEvent = CompleteRegister;
            SecondVisibility = Visibility.Hidden;
            RefreshImage();
            Password = "123qaz";
            SwitchIpProxyFrequency = 8;
            RefreshRandomName();//TODO: timeout
            RegisteredEmails = new RegisteredEmailCollection(mEmailPersistentService.GetEmaiList());
        }

        public RegisteredEmailCollection RegisteredEmails { get; set; }

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

        private Visibility mSecondVisibility;
        public Visibility SecondVisibility
        {
            get { return mSecondVisibility; }
            set
            {
                mSecondVisibility = value;
                OnPropertyChanged("SecondVisibility");
            }
        }

        private string mSecondVerifyCodeImagePath;
        public string SecondVerifyCodeImagePath
        {
            get { return mSecondVerifyCodeImagePath; }
            set
            {
                mSecondVerifyCodeImagePath = value;
                OnPropertyChanged("SecondVerifyCodeImagePath");
            }
        }

        private string mSecondVerifyCode;
        public string SecondVerifyCode
        {
            get { return mSecondVerifyCode; }
            set
            {
                mSecondVerifyCode = value;
                OnPropertyChanged("SecondVerifyCode");
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

        public DelegatingCommand RefreshSecondImageCommand
        {
            get { return new DelegatingCommand(RefreshSecondImage);}
        }

        public DelegatingCommand SecondVerifyCommand
        {
            get
            {
                return new DelegatingCommand(SecondCodeVerify);
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

        #endregion

        #region Private Methods
        private void RefreshImage()
        {
            VerifyCodeImagePath = m163EmailRegister.GetFirstVerifyCodeImagePath();
            VerifyCode = string.Empty;
        }

        private void CopyName()
        {
            Clipboard.SetDataObject(SelectedEmail.Name, false);
        }

        private void RegisterEmail()
        {
            if (SecondVisibility == Visibility.Visible)
            {
                return;
            }
            if (string.IsNullOrEmpty(VerifyCode) || string.IsNullOrWhiteSpace(VerifyCode))
            {
                return;
            }
            string secondImagePath;
            var result = m163EmailRegister.Register(Name, Password, VerifyCode, out secondImagePath);

            if (result)
            {
                OnCompleteRegisterEvent();
            }
            else
            {
                if (!string.IsNullOrEmpty(secondImagePath))
                {
                    SecondVerifyCodeImagePath = secondImagePath;
                    SecondVisibility = Visibility.Visible;
                }
                else
                {
                    RegisterFailed();
                    MessageBox.Show("注册失败");
                }
            }

        }

        private void RefreshRandomName()
        {
            var tempRandomName = RandomHelper.GetLowerRandomString(8);
            while (!m163EmailRegister.IsEmailNameAvailable(tempRandomName))
            {
                tempRandomName = RandomHelper.GetLowerRandomString(8);
            }
            Name = tempRandomName;
        }


        private void RefreshSecondImage()
        {
            SecondVerifyCodeImagePath = m163EmailRegister.GetSecondVerifyCodeImagePath();
            SecondVerifyCode = string.Empty;
        }

        private void SecondCodeVerify()
        {
            var result = m163EmailRegister.SecondVerifyPost(SecondVerifyCode);
            if (!result)
            {
                RefreshSecondImage();
                MessageBox.Show("验证码错误，请再次输入。");
                return;
            }

            OnCompleteRegisterEvent();

        }

        protected virtual void OnCompleteRegisterEvent()
        {
            if (CompleteRegisterEvent != null)
            {
                CompleteRegisterEvent(this, EventArgs.Empty);
            }
        }

        private void CompleteRegister(object sender, EventArgs e)
        {
            var email = new Email {Name = Name+"@163.com", Password = Password};
            RegisteredEmails.Add(email);
            mEmailPersistentService.Save(email);
            m163EmailRegister = new Email163Register();
            RefreshImage();
            RefreshRandomName();
            SecondVisibility = Visibility.Hidden;
        }

        private void RegisterFailed()
        {
            m163EmailRegister = new Email163Register();
            RefreshImage();
            RefreshRandomName();
            SecondVisibility = Visibility.Hidden;
        }

        #endregion
    }
}
