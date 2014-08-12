using System;

namespace yujt.common.Exceptions
{
    public class ServiceNotExistedException<TService> : Exception
    {
        private const string DEFAULT_EXCEPTION_MESSAGE = "Can not find this service: {0}";

        private readonly string mMessage;

        public ServiceNotExistedException()
        {
            mMessage = string.Format(DEFAULT_EXCEPTION_MESSAGE, typeof(TService));

        }


        public override string Message
        {
            get { return mMessage; }
        }
    }
}
