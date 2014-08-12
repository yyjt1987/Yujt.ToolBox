using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Yujt.Common.Helper
{
    public class ThreadHelper
    {
        private int mThreadCount;
        private ThreadHelper()
        { }

        private ThreadHelper(int threadCount)
        {
            mThreadCount = threadCount;
        }

        public static ThreadHelper ThreadCount(int threadCount)
        {
            return new ThreadHelper(threadCount);
        }

        public void Run<T>(Action<T> action, T obj)
        {
            while (mThreadCount > 0)
            {
                mThreadCount--;
                var thread = new Thread(() => action(obj));
                thread.Start();
            }
        }
    }
}
