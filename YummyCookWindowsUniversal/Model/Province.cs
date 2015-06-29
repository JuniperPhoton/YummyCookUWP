using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Model
{
    public class Province
    {
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public Province(int id,string name)
        {
            ProvinceID = id;
            ProvinceName = name;
        }
    }
}
