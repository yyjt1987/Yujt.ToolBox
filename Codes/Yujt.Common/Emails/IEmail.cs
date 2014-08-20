using System;
using System.Net.Mail;
using OpenPop.Mime;

namespace Yujt.Common.Emails
{
    public interface IEmail
    {
        Message ReceiveMail(string subject);

        void SaveFirstAttachement(string subject, string targetPath);

        void Send(MailMessage message);

        int GetMailCount();

    }
}
