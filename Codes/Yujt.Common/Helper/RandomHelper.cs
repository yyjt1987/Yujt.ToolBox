using System;
using System.Text;

namespace Yujt.Common.Helper
{
    public class RandomHelper
    {
        private const string KEYS_LOWER= "qwertyuioplkjhgfdsazxcvbnm";
        private const string KEYS_UPPER = "QWERTYUIOPASDFGHJKLMNBVCXZ";
        private const string KEYS_NUM = "0123456789";

        public static int GetRandomNumber(int length)
        {
            var maxNumber = (int)Math.Pow(10, length + 1) - 1;
            var random = new Random();
            return random.Next(maxNumber);
        }

        public static string GetLowerRandomString(int length)
        {
            const string keys = KEYS_LOWER + KEYS_NUM;
            return GetRandomString(length, keys);
        }

        public static string GetRandomString(int length)
        {
            const string keys = KEYS_LOWER + KEYS_UPPER + KEYS_NUM;
            return GetRandomString(length, keys);
        }

        private static string GetRandomString(int length, string keys)
        {
            var stringBuilder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                var position = random.Next(keys.Length);
                stringBuilder.Append(keys[position]);
            }
            return stringBuilder.ToString();
        }
    }
}
