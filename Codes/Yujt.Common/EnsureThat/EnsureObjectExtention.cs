using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yujt.Common.EnsureThat;

namespace Yujt.Common.EnsureThat
{
    public static class EnsureObjectExtention
    {
        public static Param<T> IsNotNull<T>(this Param<T> param) where T : class 
        {
            if (param.Value == null)
            {
                param.IsSatisfied = false;
                param.ExceptionType = typeof(ArgumentNullException);
                return param;
            }
            param.IsSatisfied = true;
            return param;
        }
    }
}
