using System.Collections.ObjectModel;
using Yujt.ToolBox.Common.Plugable;

namespace Yujt.ToolBox.UI
{
    public class MainViewModel
    {
        public ObservableCollection<IPlugable> Plugins { get; set; }

    }
}
