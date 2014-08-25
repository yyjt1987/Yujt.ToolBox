using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yujt.Common.Emails
{
    public class EmailSina : BaseEmail
    {
        public EmailSina(string userName, string password) : base(userName, password)
        {
            InitPop3("pop.163.com", 995);
            InitSmtp("smtp.163.com", 25, false);
        }
    }
}
