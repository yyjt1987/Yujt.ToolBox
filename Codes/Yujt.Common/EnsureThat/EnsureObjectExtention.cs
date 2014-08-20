using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yujt.common.EnsureThat;

namespace Yujt.Common.EnsureThat
{
    public static class EnsureObjectExtention
    {
        public static Param<object> IsNotNull(this Param<object> param)
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
