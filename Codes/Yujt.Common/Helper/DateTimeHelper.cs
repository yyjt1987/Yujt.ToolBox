using System;

namespace Yujt.Common.Helper
{
    public static class DateTimeHelper
    {
        private static DateTime mStartDateTime = new DateTime(1970, 1, 1);
        public static long GetMillisTime(this DateTime dateTime)
        {
            var timeSpan = new TimeSpan(dateTime.ToUniversalTime().Ticks - mStartDateTime.Ticks);

            return Convert.ToInt64(timeSpan.TotalMilliseconds);
        }
    }
}
