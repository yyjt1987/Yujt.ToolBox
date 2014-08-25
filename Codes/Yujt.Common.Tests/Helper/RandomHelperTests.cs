using NUnit.Framework;
namespace Yujt.Common.Helper.Tests
{
    [TestFixture()]
    public class RandomHelperTests
    {
        [Test()]
        public void GetRandomStringTest()
        {
            var password = RandomHelper.GetRandomString(12);
        }
    }
}
