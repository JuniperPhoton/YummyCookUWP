using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private static Dictionary<string, string> AppID = new Dictionary<string, string>() { { "X-Bmob-Application-Id", "390002dbd38de812c74d20482c14141c" } };
        private static Dictionary<string, string> AppKey = new Dictionary<string, string>() { { "X-Bmob-REST-API-Key", "c2010e28ce88992a83dc68019b32b298" } };
        private static HttpMediaTypeHeaderValue JsonContentType = new HttpMediaTypeHeaderValue("application/json");
        private static HttpMediaTypeHeaderValue ImageContentType = new HttpMediaTypeHeaderValue("image/jpeg");
        private const string UserUrl = "https://api.bmob.cn/1/users/";
        private const string LoginUrl = "https://api.bmob.cn/1/login/";
        private const string UploadFileUrl = "https://api.bmob.cn/1/files/";

        public async static Task<ResponseData> RegisterUser(string username,string password)
        {
            try
            {
                string namestr = JsonMaker.MakeJsonObj(nameof(username), username);
                string passwordstr = JsonMaker.MakeJsonObj(nameof(password), password);
                string param = JsonMaker.MakeJsonString(new List<string> { namestr, passwordstr });

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                var message = await client.PostAsync(new Uri(RequestHelper.UserUrl), 
                    new HttpStringContent(param, Windows.Storage.Streams.UnicodeEncoding.Utf8, JsonContentType.MediaType));
                if (message.IsSuccessStatusCode)
                {
                    var content = await message.Content.ReadAsStringAsync();
                    JsonObject jsonObj = null;
                    bool isParsed = JsonObject.TryParse(content, out jsonObj);
                    if (isParsed)
                    {
                        LocalSettingHelper.AddValue("username", username);
                        LocalSettingHelper.AddValue("password", password);
                        LocalSettingHelper.AddValue("userid", jsonObj["objectId"].GetString());
                        LocalSettingHelper.AddValue("sessiontoken", jsonObj["sessionToken"].GetString());
                        return new ResponseData(true, 400, null);
                    }
                    else return new ResponseData(false, 0, null);
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    JsonObject errorObj = JsonObject.Parse(errorContent);

                    return new ResponseData(false,(int)errorObj["code"].GetNumber(),errorObj["error"].GetString());
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return new ResponseData(false,0,null);
            }
        }

        public async static Task<ResponseData> Login(string username, string password)
        {
            try
            {
                string namestr = JsonMaker.MakeJsonObj(nameof(username), username);
                string passwordstr = JsonMaker.MakeJsonObj(nameof(password), password);
                string param = JsonMaker.MakeJsonString(new List<string> { namestr, passwordstr });

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                var message = await client.PostAsync(new Uri(RequestHelper.LoginUrl),
                    new HttpStringContent(param, Windows.Storage.Streams.UnicodeEncoding.Utf8, JsonContentType.MediaType));
                if (message.IsSuccessStatusCode)
                {
                    var content = await message.Content.ReadAsStringAsync();
                    JsonObject jsonObj = null;
                    bool isParsed = JsonObject.TryParse(content, out jsonObj);
                    if (isParsed)
                    {
                        LocalSettingHelper.AddValue("username", username);
                        LocalSettingHelper.AddValue("password", password);
                        LocalSettingHelper.AddValue("sessiontoken", jsonObj["sessionToken"].GetString());
                        LocalSettingHelper.AddValue("userid", jsonObj["objectId"].GetString());

                        return new ResponseData(true, 400, null);
                    }
                    else return new ResponseData(false, 0, null);
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    JsonObject errorObj = JsonObject.Parse(errorContent);

                    return new ResponseData(false, (int)errorObj["code"].GetNumber(), errorObj["error"].GetString());
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return new ResponseData(false, 0, null);
            }
        }

        public async static Task<ResponseData> UpdateUserInfo(string userID,Dictionary<string,string> dic)
        {
            try
            {
                List<string> paramArray = new List<string>();
                foreach(var item in dic)
                {
                    string info = JsonMaker.MakeJsonObj(item.Key, item.Value);
                    paramArray.Add(info);
                }
                
                string param = JsonMaker.MakeJsonString(paramArray);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                var message = await client.PostAsync(new Uri(RequestHelper.UserUrl+userID),
                    new HttpStringContent(param, Windows.Storage.Streams.UnicodeEncoding.Utf8, JsonContentType.MediaType));
                if (message.IsSuccessStatusCode)
                {
                    return new ResponseData(true, 400, null);
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    JsonObject errorObj = JsonObject.Parse(errorContent);

                    return new ResponseData(false, (int)errorObj["code"].GetNumber(), errorObj["error"].GetString());
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return new ResponseData(false, 0, null);
            }
        }

        public async static Task<ResponseData> UploadAvatar(string userID,Stream data)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                HttpStreamContent streamContent = new HttpStreamContent(data.AsInputStream());
                HttpMultipartFormDataContent content = new HttpMultipartFormDataContent();
                content.Add(streamContent, "file", "image");
                
                var message = await client.PostAsync(new Uri(RequestHelper.UploadFileUrl+userID+".jpg"), content);
                if (message.IsSuccessStatusCode)
                {
                    return new ResponseData(true, 400, null);
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    JsonObject errorObj = JsonObject.Parse(errorContent);

                    return new ResponseData(false, (int)errorObj["code"].GetNumber(), errorObj["error"].GetString());
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return new ResponseData(false, 0, null);
            }
        }
    }
}
