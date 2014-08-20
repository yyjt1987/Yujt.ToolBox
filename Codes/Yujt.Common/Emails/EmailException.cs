using System;

namespace Yujt.Common.Emails
{
    public class EmailException : Exception
    {
        public EmailException(string message) : base(message)
        {
            
        }
    }
}
