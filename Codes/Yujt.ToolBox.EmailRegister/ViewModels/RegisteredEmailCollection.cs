using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Yujt.ToolBox.EmailRegister.ViewModels
{
    public class RegisteredEmailCollection : ObservableCollection<Email>
    {

    }

    public class Email
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
