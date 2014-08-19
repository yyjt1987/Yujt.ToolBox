using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Yujt.Common.Emails;
using NUnit.Framework;
namespace Yujt.Common.Emails.Tests
{
    [TestFixture()]
    public class Email163Tests
    {
        private const string MAIL_USER_NAME = "uosfuid@163.com";
        private const string PASSWORD = "123qaz";

        [Test()]
        public void ReceiveMailTest()
        {
            #region Prepare Data
            IEmail mail163 = new Email163(MAIL_USER_NAME, PASSWORD);
            var sendMessage = new MailMessage(MAIL_USER_NAME, MAIL_USER_NAME)
            {
                Subject = "Test",
                Body = "Test Body"
            };

            mail163.Send(sendMessage);

            Thread.Sleep(10 * 1000);
            #endregion 

            var receiveMessage = mail163.ReceiveMail("Test");
            Assert.IsNotNull(receiveMessage);
        }

        [Test]
        public void SentMailTest()
        {
            IEmail mail163 = new Email163(MAIL_USER_NAME, PASSWORD);
            var expectCount = mail163.GetMailCount() + 1;
            var message = new MailMessage(MAIL_USER_NAME, MAIL_USER_NAME)
            {
                Subject = "Test",
                Body = "Test Body"
            };

            mail163.Send(message);

            Thread.Sleep(10 * 1000);

            var actualCount = mail163.GetMailCount();

            Assert.AreEqual(expectCount, actualCount);

        }
    }
}
