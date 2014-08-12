using System.Runtime.Remoting.Messaging;
using System.Windows.Controls;
using Yujt.ToolBox.Common.Plugable;
using Yujt.ToolBox.EmailRegister.ViewModels;
using Yujt.ToolBox.EmailRegister.Views;

namespace Yujt.ToolBox.EmailRegister.Controller
{
    public class EmailRegisterController : IController
    {
        private readonly UserControl mUserControl = new EmailRegisterControl();
        private readonly EmailRegisterViewModel mViewModel = new EmailRegisterViewModel();

        public EmailRegisterController()
        {
            mUserControl.DataContext = mViewModel;
        }
        public UserControl GetUserControl()
        {
            return mUserControl;
        }
    }
}
