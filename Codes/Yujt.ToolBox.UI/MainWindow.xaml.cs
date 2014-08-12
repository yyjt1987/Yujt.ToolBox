using System.Collections.ObjectModel;
using System.Windows;
using Yujt.ToolBox.Common.Plugable;
using Yujt.ToolBox.Core;

namespace Yujt.ToolBox.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PluginManager mPluginManager = new PluginManager();
        private readonly MainViewModel mMainViewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();


            //mPluginManager.PluginsChanged += PluginsChanged;

            if (mPluginManager.Plugins == null)
            {
                mMainViewModel.Plugins = new ObservableCollection<IPlugable>();
            }
            mMainViewModel.Plugins = new ObservableCollection<IPlugable>(mPluginManager.Plugins);

            DataContext = mMainViewModel;
        }

        //private void PluginsChanged(object sender, EventArgs e)
        //{
        //    mMainViewModel.Plugins = new ObservableCollection<IPlugable>(mPluginManager.Plugins);
        //}
    }
}
