using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Yujt.ToolBox.Common.Plugable;
using Yujt.ToolBox.EmailRegister.Controller;

namespace Yujt.ToolBox.EmailRegister
{
    [Export(typeof(IPlugable))]
    public class EmailRegisterPlugin : IPlugable
    {
        private readonly IController mController = new EmailRegisterController();
        public Guid Id
        {
            get
            {
                return new Guid("80FE8CF9-F694-43BE-B41C-D4BBC07A5D7A");
            }
        }

        public string Name
        {
            get { return "163邮箱注册机"; }
        }

        public UserControl TabItemContentControl
        {
            get { return mController.GetUserControl(); }
        }

        public string IcoPath {
            get { return "/Yujt.ToolBox.ProxySwitcher;component/Images/Tools.ico"; }
        }
    }
}
