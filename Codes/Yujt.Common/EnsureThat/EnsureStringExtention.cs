using System;

namespace Yujt.Common.EnsureThat
{
    public static class EnsureStringExtention
    {
        public static Param<string> IsNotNullOrEmpty(this Param<string> param)
        {
            if (string.IsNullOrEmpty(param.Value))
            {
                param.IsSatisfied = false;
                param.ExceptionType = typeof (ArgumentNullException);
                return param;
            }
            param.IsSatisfied = true;
            return param;
        }

        public static Param<string> IsNotNullOrWhiteSpace(this Param<string> param)
        {
            if (string.IsNullOrEmpty(param.Value.Trim()))
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
