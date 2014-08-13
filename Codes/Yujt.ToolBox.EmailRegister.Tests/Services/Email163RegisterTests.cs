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
        public void GetVerifyCodeImageTest()
        {
            var emailRegister = new Email163Register();

            emailRegister.GetFirstVerifyCodeImagePath();
        }

        [Test()]
        public void GetHttpsCookiesTest()
        {
            var emailRegister = new Email163Register();
            var cookies = emailRegister.GetHttpsCookies();

            Assert.AreEqual(3, cookies.Count);
        }

        [Test()]
        public void GetVcCodeUrlFromPageContentTest()
        {
            var emailRegister = new Email163Register();
            var url = emailRegister.GetVcCodeUrlFromPageContent();

            Assert.IsNotNull(url);
        }

        [Test()]
        public void IsEmailNameAvailableTest()
        {
            var emailRegister = new Email163Register();
            var result = emailRegister.IsEmailNameAvailable("slkdjhflksdhflkjf");
            Assert.IsTrue(result);
        }

        [Test()]
        public void EnvalueCalcTest()
        {
            var emailRegister = new Email163Register();
            var envalue = emailRegister.EnvalueCalc("537472");
        }

        [Test()]
        public void GetVcCodeUrlFromPageContentTest1()
        {
            var emailRegister = new Email163Register();
            var envalue = emailRegister.GetVcCodeUrlFromPageContent();
        }
    }
}
