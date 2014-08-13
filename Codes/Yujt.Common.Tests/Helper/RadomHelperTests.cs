using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yujt.Common.Helper;
using NUnit.Framework;
namespace Yujt.Common.Helper.Tests
{
    [TestFixture()]
    public class RadomHelperTests
    {
        [Test()]
        public void GetRandomNumberTest()
        {
            for (int i = 0; i < 5; i++)
            {
                var randomNum = RandomHelper.GetRandomNumber(9);

                Console.WriteLine(randomNum);
            }

        }
    }
}
