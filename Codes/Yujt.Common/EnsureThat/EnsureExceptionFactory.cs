﻿using System;

namespace Yujt.Common.EnsureThat
{

    public class EnsureExceptionFactory
    {
        public static Exception CreateException(Type exceptionType, string errMsg)
        {
            var constructor = exceptionType.GetConstructor(new[] {typeof(string)});
            return (Exception)constructor.Invoke(new object[]{errMsg});
        }
    }
}
