using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Controls;
using Yujt.ToolBox.Common.Plugable;
using Yujt.ToolBox.ProxySwitcher.Controllers;

namespace Yujt.ToolBox.ProxySwitcher
{
    [Export(typeof(IPlugable))]
    public class ProxySwitcherPlugin : IPlugable
    {
        //[Import("ProxySwitcherController")]
        private readonly IController mController = new ProxySwitcherController();
        public Guid Id
        {
            get
            {
                return new Guid("535AF12D-59F3-4532-80E7-0DE61E165E41");
            }
        }

        public string Name
        {
            get { return Resource.PluginName; }
        }

        

        public UserControl TabItemContentControl
        {
            get { return mController.GetUserControl(); }
            //get { return new ProxySwitcherControl(); }
        }

        public string IcoPath {
            get { return "/Yujt.ToolBox.ProxySwitcher;component/Images/Tools.ico"; }
        }
    }
}
