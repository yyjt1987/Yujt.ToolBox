using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPop.Pop3;

namespace Yujt.Common.Emails
{
    public abstract class BaseEmail : IEmail
    {
        protected Pop3Client mPop3Client;

        protected BaseEmail(string userName, string password)
        {
            mPop3Client = new Pop3Client();
            

        }

        protected virtual void Connect()
        { }

        public virtual void ReceiveMail()
        {
            
        }
    }
}
