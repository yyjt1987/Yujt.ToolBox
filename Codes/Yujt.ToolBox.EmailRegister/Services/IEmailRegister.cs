using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Yujt.ToolBox.EmailRegister.Services
{
    public interface IEmailRegister
    {
        bool IsEmailNameAvailable(string name);

        bool Register(string name, string password, string verifyCode);

        string GetVerifyCodeImagePath();
    }
}
