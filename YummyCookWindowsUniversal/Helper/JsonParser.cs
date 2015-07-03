using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace YummyCookWindowsUniversal.Helper
{
    public class JsonParser
    {
        public static string GetStringFromJsonObj(IJsonValue obj,string propertyName)
        {
            try
            {
                return obj.GetObject()[propertyName].GetString();
            }
            catch(Exception)
            {
                return "";
            }
        }

        public static bool GetBooleanFromJsonObj(IJsonValue obj, string propertyName,bool defaultValue = false)
        {
            try
            {
                return obj.GetObject()[propertyName].GetBoolean();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
