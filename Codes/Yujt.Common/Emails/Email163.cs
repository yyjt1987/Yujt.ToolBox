using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPop.Pop3;


namespace Yujt.Common.Emails
{
    public class Email163 : IEmail
    {
        private Pop3Client mPop3Client;

        public Email163(string userName, string password)
        {
            mPop3Client = new Pop3Client();
        }

        public void ReceiveMail()
        {

        }
    }
}
