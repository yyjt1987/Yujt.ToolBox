using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Yujt.ToolBox.EmailRegister.ViewModels
{
    public class RegisteredEmailCollection : ObservableCollection<Email>
    {
        public RegisteredEmailCollection(IEnumerable<Email> emails)
            : base(emails)
        {
            
        }
    }

    public class Email
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
