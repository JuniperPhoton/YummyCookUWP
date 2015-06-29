using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Helper
{
    public static class ConfigHelper
    {
        public static void ConfigAppSetting()
        {

        }

        public static bool CheckUserCache()
        {
            if(LocalSettingHelper.IsExist("username"))
            {
                if (LocalSettingHelper.GetValue("username") != null) return true;
            }
            return false;
        }

        
    }
}
