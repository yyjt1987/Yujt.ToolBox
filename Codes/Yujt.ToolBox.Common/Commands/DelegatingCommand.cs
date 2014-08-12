using System;

namespace Yujt.ToolBox.Common.Commands
{
    public class DelegatingCommand : BaseCommand
    {
        private readonly Action mDelegateAction;

        public DelegatingCommand(Action delegateAction)
        {
            mDelegateAction = delegateAction;
        }

        public override void Execute(object parameter)
        {
            mDelegateAction();
        }
    }
}
