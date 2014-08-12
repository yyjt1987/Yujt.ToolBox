namespace Yujt.Common.Helper
{
    public static class StringHelper
    {
        private const string WHITE_SPACE = " ";

        public static string RemoveWhiteSpace(this string str)
        {
            return str.Replace(WHITE_SPACE, string.Empty);
        }
    }
}
