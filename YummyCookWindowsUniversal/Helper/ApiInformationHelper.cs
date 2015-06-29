using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace YummyCookWindowsUniversal.Helper
{
    public class ApiInformationHelper
    {
        public static bool CheckHardwareButton()
        {
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                return true;
            }
            else return false;
        }

        public static bool CheckStatusBar()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                return true;
            }
            else return false;
        }
    }
}
