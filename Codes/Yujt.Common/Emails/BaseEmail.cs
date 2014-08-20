using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime;
using OpenPop.Mime;
using OpenPop.Pop3;
using yujt.common.EnsureThat;
using Yujt.Common.EnsureThat;
using Yujt.Common.Helper;

namespace Yujt.Common.Emails
{
    public abstract class BaseEmail : IEmail
    {
        private readonly string mUserName;
        private readonly string mPassword;
        private ConnectionStruct mPop3ConnectionStruct;
        private ConnectionStruct mSmtpConnectionStruct;

        protected BaseEmail(string userName, string password)
        {
            mUserName = userName;
            mPassword = password;
        }

        protected void InitPop3(string hostName, int port, bool useSsl = true)
        {

            mPop3ConnectionStruct = new ConnectionStruct()
            {
                HostName = hostName,
                Port = port,
                UseSsl = useSsl
            };

        }

        protected void InitSmtp(string hostName, int port, bool useSsl = true)
        {
            mSmtpConnectionStruct = new ConnectionStruct()
            {
                HostName = hostName,
                Port = port,
                UseSsl = useSsl
            };
        }

        public Message ReceiveMail(string subject)
        {
            using (var pop3Client = GetPop3Client())
            {
                var count = pop3Client.GetMessageCount();
                while (count > 0)
                {
                    var message = pop3Client.GetMessage(count);

                    if (message.Headers.Subject.Equals(subject))
                    {
                        return message;
                    }
                    count--;
                }
                return null;
            }
        }

        public void SaveFirstAttachement(string subject, string targetPath)
        {
            var message = ReceiveMail(subject);
            Ensure.That(message).IsNotNull()
                  .IfNotSatisfyThrow("No Mail can be found with the subject of {0}", new[] { subject });
            var attachement = message.FindAllAttachments().FirstOrDefault();

            Ensure.That(attachement).IsNotNull()
                  .IfNotSatisfyThrow("There is no mail contain attachement whose subject is{0}", new[] { subject });
            
            if (!File.Exists(targetPath))
            {
                FileHelper.CreateParentDirectory(targetPath);
            }

            using (var saveStream = new FileStream(targetPath, FileMode.Create))
            {
                attachement.Save(saveStream);
            }
        }

        public void Send(MailMessage message)
        {
            using (var smtpClient = GetSmtpClient())
            {
                smtpClient.Send(message);
            }
        }

        public int GetMailCount()
        {
            using (var pop3Client = GetPop3Client())
            {
                return pop3Client.GetMessageCount();
            }
        }


        #region Private Methods

        private Pop3Client GetPop3Client()
        {
            var pop3Client = new Pop3Client();
            pop3Client.Connect(mPop3ConnectionStruct.HostName,
                               mPop3ConnectionStruct.Port,
                               mPop3ConnectionStruct.UseSsl);
            pop3Client.Authenticate(mUserName, mPassword);

            return pop3Client;
        }

        private SmtpClient GetSmtpClient()
        {
            var smtpClient = new SmtpClient(mSmtpConnectionStruct.HostName, mSmtpConnectionStruct.Port)
            {
                Credentials = new NetworkCredential(mUserName, mPassword),
                EnableSsl = mSmtpConnectionStruct.UseSsl
            };
            return smtpClient;
        }

        #endregion
    }


    internal class ConnectionStruct
    {
        public string HostName;
        public int Port;
        public bool UseSsl;
    }
}
