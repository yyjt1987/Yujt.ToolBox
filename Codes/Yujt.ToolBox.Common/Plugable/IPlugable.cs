using System;
using System.Windows.Controls;

namespace Yujt.ToolBox.Common.Plugable
{
    public interface IPlugable
    {
        Guid Id { get; }

        string Name { get; }

        UserControl TabItemContentControl { get; }

        string IcoPath { get; }
    }
}
