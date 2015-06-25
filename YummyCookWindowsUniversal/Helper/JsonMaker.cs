using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Helper
{
    public class JsonMaker
    {
        public static string MakeJsonObj(string propertyName,string propertyValue)
        {
            string str = string.Format("\"{0}\":\"{1}\"", propertyName, propertyValue);

            return str;
        }

        public static string MakeJsonObj(string propertyName, int propertyValue)
        {
            string str = string.Format("\"{0}\":{1}", propertyName, propertyValue);

            return str;
        }

        public static string MakeJsonObj(string propertyName, bool propertyValue)
        {
            string str = string.Format("\"{0}\":{1}", propertyName, propertyValue);

            return str;
        }

        public static string MakeJsonString(List<string> objArray)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for(int i=0;i<objArray.Count;i++)
            {
                sb.Append(objArray[i]);
                if(i!=objArray.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
