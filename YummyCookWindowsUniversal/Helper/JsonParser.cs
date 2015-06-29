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
        public static string GetStringFromJsonObj(IJsonValue obj)
        {
            try
            {
                return obj.GetString();
            }
            catch(Exception)
            {
                return "";
            }
        }

        public static bool GetBooleanFromJsonObj(IJsonValue obj, bool defaultValue = false)
        {
            try
            {
                return obj.GetBoolean();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
