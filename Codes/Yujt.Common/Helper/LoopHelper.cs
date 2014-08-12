using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Yujt.Common.Helper
{
    public class LoopHelper
    {
        private int mLoopCount;

        private LoopHelper()
        {
            
        }
        private LoopHelper(int count)
        {
            mLoopCount = count;
        }

        public static LoopHelper Count(int count)
        {
            return  new LoopHelper(count);
        }

        public void Execute(Action action)
        {
            while (mLoopCount > 0)
            {
                try
                {
                    action();
                    mLoopCount--;
                }catch(Exception)
                { }
            }
        }
    }
}
