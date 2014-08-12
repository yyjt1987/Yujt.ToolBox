using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yujt.ToolBox.EmailRegister.Services;
using NUnit.Framework;
namespace Yujt.ToolBox.EmailRegister.Services.Tests
{
    [TestFixture()]
    public class Email163RegisterTests
    {
        [Test()]
        public void EnvalueCalcTest()
        {
            var emailRegister = new Email163Register();
            var s = emailRegister.EnvalueCalc("588927");

        }
    }
}
