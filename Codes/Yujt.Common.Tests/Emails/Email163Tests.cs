using System.IO;
using System.Net.Mail;
using System.Threading;
using NUnit.Framework;
using Yujt.Common.Helper;

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

        [Test]
        public void SaveFirstAttachementTest()
        {
            #region Prepare Data

            const string expectedContent = "Hello World!";
            IEmail mail163 = new Email163(MAIL_USER_NAME, PASSWORD);
            var sendMessage = new MailMessage(MAIL_USER_NAME, MAIL_USER_NAME)
            {
                Subject = "Test Attachement",
                Body = string.Empty,
            };
            var tempPath1 = Path.GetTempFileName();
            using (var fileStream = new FileStream(tempPath1, FileMode.Create))
            {
                using (var fileWriter = new StreamWriter(fileStream))
                {
                    fileWriter.WriteLine(expectedContent);
                }
            }
            sendMessage.Attachments.Add(new Attachment(tempPath1));
            mail163.Send(sendMessage);

            #endregion Prepare Data

            Thread.Sleep(10 * 1000);

            var resultPath = Path.GetTempFileName();
            mail163.SaveFirstAttachement("Test Attachement", resultPath);

            Assert.IsTrue(File.Exists(resultPath));

            var fileSize = FileHelper.GetFileSize(resultPath);
            Assert.IsTrue(fileSize > 0);

            var resultContent = FileHelper.GetContent(resultPath);
            Assert.AreEqual(expectedContent+"\r\n", resultContent);
        }
    }
}
