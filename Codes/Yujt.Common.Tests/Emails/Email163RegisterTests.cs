using NUnit.Framework;
namespace Yujt.ToolBox.EmailRegister.Services.Tests
{
    [TestFixture]
    public class Email163RegisterTests
    {
        [Test]
        public void IsEmailNameAvailableTest()
        {
            var email163Register = new Email163Register();
             var result = email163Register.IsEmailNameAvailable("jsfhkaljsdfhkljhf");

            Assert.IsTrue(result);
        }
    }
}
