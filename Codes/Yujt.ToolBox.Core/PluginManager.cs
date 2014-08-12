using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Threading;
using Yujt.ToolBox.Common.Plugable;

namespace Yujt.ToolBox.Core
{
    public class PluginManager
    {
        private readonly string mPluginDir = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Plugins");

        public PluginManager()
        {
            LoadPlugins();
            //WhatchPluginFolder();
        }

        [ImportMany(typeof(IPlugable))]
        public List<IPlugable> Plugins = null;

        public event EventHandler PluginsChanged;

        #region Private methods

        private void LoadPlugins()
        {
            try
            {
                if (!Directory.Exists(mPluginDir))
                {
                    Directory.CreateDirectory(mPluginDir);
                }

                using (var catalog = new DirectoryCatalog(mPluginDir, "*.dll"))
                {
                    using (var container = new CompositionContainer(catalog))
                    {
                        container.ComposeParts(this);
                    }
                }
            }
            catch (Exception e)
            {

            }

        }

        private void WhatchPluginFolder()
        {
            var thread = new Thread(() =>
            {
                var watcher = new FileSystemWatcher();
                //初始化监听
                watcher.BeginInit();
                //设置监听文件类型
                watcher.Filter = "*.dll";
                //设置是否监听子目录
                watcher.IncludeSubdirectories = false;
                //设置是否启用监听?
                watcher.EnableRaisingEvents = true;
                //设置需要监听的更改类型(如:文件或者文件夹的属性,文件或者文件夹的创建时间;NotifyFilters枚举的内容)
                watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
                //设置监听的路径
                watcher.Path = mPluginDir;
                //注册创建文件或目录时的监听事件
                watcher.Changed += OnPluginsChanged;
                //结束初始化
                watcher.EndInit();
            });

            thread.Start();

        }

        private void OnPluginsChanged(object sender, FileSystemEventArgs e)
        {
            LoadPlugins();

            if (PluginsChanged != null)
            {
                PluginsChanged(this, EventArgs.Empty);
            }
        }

        #endregion

    }
}
