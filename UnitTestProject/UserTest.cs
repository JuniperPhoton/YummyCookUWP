using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyCookWindowsUniversal.Helper;

namespace UnitTestProject
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public async Task GetUserTest()
        {
            var user = await RequestHelper.GetUserInfoAsync("Chao");
            Assert.IsNotNull(user);
        }

        public async Task GetUserTestFail()
        {
            var user = await RequestHelper.GetUserInfoAsync("Chao2");
            Assert.IsNull(user);
        }
    }
}
