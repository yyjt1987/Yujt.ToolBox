using System.Windows.Controls;

namespace Yujt.ToolBox.Common.Plugable
{
    public interface IController
    {
        UserControl GetUserControl();
    }
}
