using System;

namespace Yujt.Common.EnsureThat
{
    public class Param<T>
    {
        public readonly T Value;
        internal Type ExceptionType { get; set; }
        internal bool IsSatisfied { get; set; }
        internal Param(T value)
        {
            Value = value;
        }

        public void IfNotSatisfyThrow(string errMsg)
        {
            if (!IsSatisfied)
            {
                throw EnsureExceptionFactory.CreateException(ExceptionType, errMsg);
            }
        }
        public void IfNotSatisfyThrow(string errMsgFormat, string[] args)
        {
            var errMsg = string.Format(errMsgFormat, args);
            IfNotSatisfyThrow(errMsg);
        }
        public void IfNotSatisfyThrow(Exception ex)
        {
            throw ex;
        }

        public void IfNotSatisfyCall(Action<T> action )
        {
            action(Value);
        }
    }

}
