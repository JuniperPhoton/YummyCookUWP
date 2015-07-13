using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Model;
using YummyCookWindowsUniversal.ViewModel;

namespace UnitTestProject
{
    [TestClass]
    public class RecipeTest
    {
        [TestMethod]
        public async Task GetAllRecipesTest()
        {
            var data = await RequestHelper.GetAllRecipesAsync("0", "10");
            Assert.IsNotNull(data);
        }
        [TestMethod]
        public async Task GetRecipeTest()
        {
            var resp = await RequestHelper.GetRecipeAsync("d1a00667e7");
            Assert.IsNotNull(resp);
        }
        
    }
}
