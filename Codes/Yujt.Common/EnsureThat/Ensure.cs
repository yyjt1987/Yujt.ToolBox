namespace yujt.common.EnsureThat
{
    public class Ensure
    {
        public static Param<T> That<T>(T value)
        {
            return new Param<T>(value);
        }
    }
}
