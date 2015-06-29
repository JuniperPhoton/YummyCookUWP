using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Model
{
    public class City
    {
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int ProvinceID { get; set; }

        public City(int id,string name,int province_id)
        {
            CityID = id;
            CityName = name;
            ProvinceID = province_id;
        }
    }
}
