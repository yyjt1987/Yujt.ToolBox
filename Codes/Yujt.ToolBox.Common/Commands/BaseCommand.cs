using System;
using System.Windows.Input;

namespace Yujt.ToolBox.Common.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public virtual void Execute(object parameter)
        {
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
