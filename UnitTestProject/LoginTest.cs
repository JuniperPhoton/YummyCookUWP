using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using YummyCookWindowsUniversal.ViewModel;
using YummyCookWindowsUniversal.Helper;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public async Task TestLogin()
        {
            var result1=await Login("Chao", "0000");
            Assert.IsTrue(result1);
            var result2=await Login("Chao", "0001");
            Assert.IsTrue(!result2);
        }

        private async Task<bool> Login(string userName,string password)
        {
            var resp = await RequestHelper.Login(userName, password);
            try
            {
                Assert.IsNotNull(resp);
                Assert.IsTrue(resp.IsSuccess);
                return true;
            }
            catch(AssertFailedException)
            {
                return false;
            }
        }
        
        [TestMethod]
        public async Task TestRegisterFail()
        {
            var resp = await RequestHelper.RegisterUser("dwc", "");
            Assert.IsNotNull(resp);
            Assert.IsTrue(!resp.IsSuccess);
        }
    }
}
