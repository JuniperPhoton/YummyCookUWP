using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using YummyCookWindowsUniversal.Model;

namespace YummyCookWindowsUniversal.Helper
{
    public class RequestHelper
    {
        private static Dictionary<string, string> AppID = new Dictionary<string, string>() { { "X-Bmob-Application-Id", "215000c1f0f2b281e15eb7c41c545323" } };
        private static Dictionary<string, string> AppKey = new Dictionary<string, string>() { { "X-Bmob-REST-API-Key", "65835223f62d4618ccd7d66b73cc9ddc" } };
        private static HttpMediaTypeHeaderValue ContentType = new HttpMediaTypeHeaderValue("application/json");
        private const string UserUrl = "https://api.bmob.cn/1/users";

        public async static Task<JsonObject> RegisterUsesr(string param)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                var message = await client.PostAsync(new Uri(RequestHelper.UserUrl), 
                    new HttpStringContent(param, Windows.Storage.Streams.UnicodeEncoding.Utf8, ContentType.MediaType));
                if (message.IsSuccessStatusCode)
                {
                    var content = await message.Content.ReadAsStringAsync();
                    JsonObject jsonObj = null;
                    bool isParsed = JsonObject.TryParse(content, out jsonObj);
                    if (isParsed)
                    {
                        return jsonObj;
                    }
                    else return null;
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    return JsonObject.Parse(errorContent);
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
