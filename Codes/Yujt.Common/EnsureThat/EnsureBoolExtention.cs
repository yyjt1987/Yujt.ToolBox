using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yujt.common.EnsureThat;

namespace Yujt.Common.EnsureThat
{
    public static class EnsureBoolExtention
    {
        public static Param<bool> IsTrue(this Param<bool> param)
        {
            if (!param.Value)
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
