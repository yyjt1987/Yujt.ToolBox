using NUnit.Framework;
using Yujt.ToolBox.Common.Plugable;

namespace Yujt.ToolBox.ProxySwitcher.Controllers.Tests
{
    [TestFixture()]
    public class ProxySwitcherControllerTests
    {
        [Test()]
        public void GetUserControlTest()
        {
            IController controller = new ProxySwitcherController();

            var userControl = controller.GetUserControl();

            Assert.IsNotNull(userControl);
        }
    }
}
