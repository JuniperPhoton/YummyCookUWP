using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Model
{
    public class NavigationData
    {
        public Dictionary<string, string> paramsData;
        public NavigationData()
        {
            paramsData = new Dictionary<string, string>();
        }

    }
}
