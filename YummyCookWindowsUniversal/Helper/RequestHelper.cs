using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using JP.Utils.Data.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using YummyCookWindowsUniversal.Model;
using YummyCookWindowsUniversal.ViewModel;

namespace YummyCookWindowsUniversal.Helper
{
    public class RequestHelper
    {

        //用于请求头
        private static Dictionary<string, string> AppID = new Dictionary<string, string>() { { "X-Bmob-Application-Id", "390002dbd38de812c74d20482c14141c" } };
        private static Dictionary<string, string> AppKey = new Dictionary<string, string>() { { "X-Bmob-REST-API-Key", "c2010e28ce88992a83dc68019b32b298" } };

        private static HttpMediaTypeHeaderValue JsonContentType = new HttpMediaTypeHeaderValue("application/json");
        private static HttpMediaTypeHeaderValue ImageContentType = new HttpMediaTypeHeaderValue("image/jpeg");

        //各请求的URL
        private const string UserUrl = "https://api.bmob.cn/1/users/";
        private const string LoginUrl = "https://api.bmob.cn/1/login/";
        private const string UploadFileUrl = "https://api.bmob.cn/1/files/";
        private const string RecipeUrl = "https://api.bmob.cn/1/classes/Recipe/";

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="username">用户名 </param>
        /// <param name="password">密码（明文）</param>
        /// <returns>返回一个数据，包含是否成功，失败时包含错误代码</returns>
        public async static Task<ResponseData> RegisterUser(string username, string password)
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
                    else return new ResponseData(false, 0, content);
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

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名 </param>
        /// <param name="password">密码（明文）</param>
        /// <returns>返回一个数据，包含是否成功，失败时包含错误代码</returns>
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
                var message = await client.GetAsync(new Uri(RequestHelper.LoginUrl + "?username=" + username + "&password=" + password));
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
                    else return new ResponseData(false, 0, content);
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

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="dic">包含更新的属性的字典</param>
        /// <returns>返回一个数据，包含是否成功，失败时包含错误代码</returns>
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
                client.DefaultRequestHeaders.Add("X-Bmob-Session-Token", LocalSettingHelper.GetValue("sessiontoken"));
                var message = await client.PutAsync(new Uri(RequestHelper.UserUrl+userID),
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

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="streamData">Stream数据</param>
        /// <returns>返回头像图片的完整URL地址</returns>
        public async static Task<string> UploadAvatar(string userID,Stream streamData)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());

                byte[] srcData = new byte[streamData.Length];
                await streamData.ReadAsync(srcData, 0, (int)streamData.Length);
                var bufferData=WindowsRuntimeBufferExtensions.AsBuffer(srcData);

                HttpBufferContent bufferContent = new HttpBufferContent(bufferData);
                
                var message = await client.PostAsync(new Uri(RequestHelper.UploadFileUrl+userID+".jpg"), bufferContent);
                if (message.IsSuccessStatusCode)
                {
                    JsonObject job = JsonObject.Parse(await message.Content.ReadAsStringAsync());
                    return "http://file.bmob.cn/"+job["url"].GetString();
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    JsonObject errorObj = JsonObject.Parse(errorContent);

                    return null;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 获得首页的亨饪攻略
        /// </summary>
        /// <param name="start">分页开始</param>
        /// <param name="number">加载数量</param>
        /// <returns>返回ObservableCollection<Recipe>列表，用于数据绑定</returns>
        public async static Task<ObservableCollection<Recipe>> GetAllRecipesAsync(string start, string number)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                var message = await client.GetAsync(new Uri(RequestHelper.RecipeUrl+"?skip="+start+"&limit="+number+ "&order=-updatedAt"+"&a="+new Random().Next()));
                if (message.IsSuccessStatusCode)
                {

                    JsonObject job = JsonObject.Parse(await message.Content.ReadAsStringAsync());
                    ObservableCollection<Recipe> listToReturn = new ObservableCollection<Recipe>();
                    JsonArray array = job["results"].GetArray();
                    if (array.Count != 0)
                    {
                        int i = 0;
                        foreach (var item in array)
                        {
                            try
                            {
                                var recipe = await Recipe.PopulateInstanceFromJson(item);

                                listToReturn.Add(recipe);
                                i++;
                            }
                            catch (Exception e)
                            {
                                Messenger.Default.Send(new GenericMessage<string>(e.Message), MessengerToken.ToastToken);
                                return null;
                            }
                        }
                        return listToReturn;
                    }
                    else return listToReturn;
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    JsonObject errorObj = JsonObject.Parse(errorContent);

                    return null;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="nameOrID">用户名字/ID</param>
        /// <param name="isName">是否为用户名字</param>
        /// <returns></returns>
        /// 在返回后，有需要的话，可以调用 DownloadAvatar() 获取头像
        public async static Task<User> GetUserInfoAsync(string nameOrID,bool isName=true)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                string requestJson = null;
                if (isName)
                {
                    requestJson = JsonMaker.MakeJsonObj("username", nameOrID);
                }
                else
                {
                    requestJson = JsonMaker.MakeJsonObj("objectId", nameOrID);
                }

                var message = await client.GetAsync(new Uri(RequestHelper.UserUrl + "?where="+JsonMaker.MakeJsonString(new List<string> { requestJson })+"&a="+new Random().Next()));
                if (message.IsSuccessStatusCode)
                {
                    var content = await message.Content.ReadAsStringAsync();
                    var obj = JsonObject.Parse(content);
                    var value = obj["results"].GetArray().FirstOrDefault();

                    var user = await User.PopulateInstanceFromJson(value);

                    return user;
                }
                else
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    JsonObject errorObj = JsonObject.Parse(errorContent);

                    return null;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async static Task<Recipe> GetRecipeAsync(string recipeID)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                var message = await client.GetAsync(new Uri(RequestHelper.RecipeUrl + "/"+recipeID));
                if (message.IsSuccessStatusCode)
                {
                    JsonObject job = JsonObject.Parse(await message.Content.ReadAsStringAsync());
                    if(job.GetObject()!=null)
                    {
                        var recipe = await Recipe.PopulateInstanceFromJson(job.GetObject());
                        return recipe;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async static Task<ObservableCollection<Recipe>> GetRecipesByUserAsync(string username)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                var message = await client.GetAsync(new Uri(RequestHelper.RecipeUrl + "?order=-updatedAt" + "&where=" + JsonMaker.MakeJsonString(new List<string> { JsonMaker.MakeJsonObj("username", username) }) + "&a=" + new Random().Next()));
                if (message.IsSuccessStatusCode)
                {

                    JsonObject job = JsonObject.Parse(await message.Content.ReadAsStringAsync());
                    ObservableCollection<Recipe> listToReturn = new ObservableCollection<Recipe>();
                    JsonArray array = job["results"].GetArray();
                    if (array.Count != 0)
                    {
                        int i = 0;
                        foreach (var item in array)
                        {
                            try
                            {
                                var recipe = await Recipe.PopulateInstanceFromJson(item);

                                listToReturn.Add(recipe);
                                i++;
                            }
                            catch (Exception e)
                            {
                                Messenger.Default.Send(new GenericMessage<string>(e.Message), MessengerToken.ToastToken);
                                return null;
                            }
                        }
                    }
                    return listToReturn;
                }
                else return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 上传图像
        /// </summary>
        /// <param name="fileNameWithExt">完整的文件名</param>
        /// <param name="srcData">byte[] 类型的二进制数据</param>
        /// <returns>返回图片的URL 地址</returns>
        public async static Task<string> UploadImageAsync(string fileNameWithExt, byte[] srcData)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());

                var bufferData = WindowsRuntimeBufferExtensions.AsBuffer(srcData);

                HttpBufferContent bufferContent = new HttpBufferContent(bufferData);

                var respMsg = await client.PostAsync(new Uri(RequestHelper.UploadFileUrl + fileNameWithExt+"?a="+new Random().Next()), bufferContent);

                if (respMsg.IsSuccessStatusCode)
                {
                    JsonObject job = JsonObject.Parse(await respMsg.Content.ReadAsStringAsync());
                    return "http://file.bmob.cn/" + job["url"].GetString();
                }
                else
                {
                    var errorContent = await respMsg.Content.ReadAsStringAsync();
                    JsonObject errorObj = JsonObject.Parse(errorContent);

                    return null;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 发表攻略
        /// </summary>
        /// <param name="jsonStr">合成的JSON 数据</param>
        /// <returns></returns>
        public async static Task<bool> PublishRecipeAsync(string jsonStr)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());

                var message = await client.PostAsync(new Uri(RequestHelper.RecipeUrl),
                    new HttpStringContent(jsonStr, Windows.Storage.Streams.UnicodeEncoding.Utf8, JsonContentType.MediaType));
                if (message.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(e.Message), MessengerToken.ToastToken);
                return false;
            }
        }

        /// <summary>
        /// 根据URL 获取图片流
        /// </summary>
        /// <param name="url">URL 地址</param>
        /// <returns>返回IRandomAccessStream</returns>
        public static async Task<IRandomAccessStream> GetImageFromUrl(string url)
        {
            HttpClient clientImage = new HttpClient();
            var response = await clientImage.GetAsync(new Uri(url));
            if (response.IsSuccessStatusCode)
            {
                var buffer = await response.Content.ReadAsBufferAsync();
                var streamImage = buffer.AsStream();

                return streamImage.AsRandomAccessStream();
            }
            else return null;
        }

    }
}
