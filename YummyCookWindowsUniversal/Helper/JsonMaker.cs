using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Helper
{
    public class JsonMaker
    {
        /// <summary>
        /// 把属性和值，格式化为JSON格式，比如 "name":"JuniperPhoton"
        /// </summary>
        /// <param name="propertyName">属性名字</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns></returns>
        public static string MakeJsonObj(string propertyName,string propertyValue)
        {
            string str = string.Format("\"{0}\":\"{1}\"", propertyName, propertyValue);

            return str;
        }

        /// <summary>
        /// 把属性和值，格式化为JSON格式，比如 "gender":1
        /// </summary>
        /// <param name="propertyName">属性名字</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns></returns>
        public static string MakeJsonObj(string propertyName, int propertyValue)
        {
            string str = string.Format("\"{0}\":{1}", propertyName, propertyValue);

            return str;
        }

        /// <summary>
        /// 把属性和值，格式化为JSON格式，比如 "IsCheckd":true
        /// </summary>
        /// <param name="propertyName">属性名字</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns></returns>
        public static string MakeJsonObj(string propertyName, bool propertyValue)
        {
            string str = string.Format("\"{0}\":{1}", propertyName, propertyValue);

            return str;
        }

        /// <summary>
        /// 格式化各属性JSON 格式为 JSON 字符串，比如 {"name":"chao","gender":"1"}
        /// </summary>
        /// <param name="objArray"></param>
        /// <returns></returns>
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
