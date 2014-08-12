using System;
using System.Collections.Generic;
using yujt.common.Exceptions;

namespace yujt.common.Service
{
    public class ServiceLocator
    {
        private static readonly Dictionary<Type, object> mSerivces = new Dictionary<Type, object>();
        public static Dictionary<Type, object> RegisterService<TService>(object service)
        {
            mSerivces.Add(typeof(TService), service);
            return mSerivces;
        }

        public static TService GetInstance<TService>()
        {
            object serviceInstance;
            if (mSerivces.TryGetValue(typeof (TService), out serviceInstance))
            {
                return (TService)serviceInstance;
            }
            throw new ServiceNotExistedException<TService>();
        }

    }
}
