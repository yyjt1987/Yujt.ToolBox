namespace Yujt.Common.Emails
{
    public class Email163 : BaseEmail
    {

        public Email163(string userName, string password) : base(userName, password)
        {
            InitPop3("pop.163.com", 995);
            InitSmtp("smtp.163.com", 25, false);
        }
    }
}
