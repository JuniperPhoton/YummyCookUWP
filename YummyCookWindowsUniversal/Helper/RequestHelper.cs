using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using YummyCookWindowsUniversal.Model;

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
        private const string GetRecipeUrl = "https://api.bmob.cn/1/classes/Recipe?";

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
        /// <param name="data">Stream数据</param>
        /// <returns>返回头像图片的完整URL地址</returns>
        public async static Task<string> UploadAvatar(string userID,Stream data)
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
        public async static Task<ObservableCollection<Recipe>> GetAllRecipes(string start, string number)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add(AppID.First());
                client.DefaultRequestHeaders.Add(AppKey.First());
                var message = await client.GetAsync(new Uri(RequestHelper.GetRecipeUrl+"skip="+start+"&limit="+number+ "&order=-updatedAt"));
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
                                var checklist = JsonParser.GetStringFromJsonObj(item.GetObject()["checklist"]);
                                var content = JsonParser.GetStringFromJsonObj(item.GetObject()["content"]);
                                var username = JsonParser.GetStringFromJsonObj(item.GetObject()["username"]);
                                var id = JsonParser.GetStringFromJsonObj(item.GetObject()["objectId"]);
                                var title = JsonParser.GetStringFromJsonObj(item.GetObject()["title"]);

                                var recipe = new Recipe();
                                recipe.CookUser = new User();
                                recipe.CookUser.UserName = username;
                                recipe.CookUser.Avatar = new BitmapImage();
                                var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets\\Image");
                                var file = await folder.GetFileAsync("1.jpg");
                                using (var stream = await file.OpenStreamForReadAsync())
                                {
                                   await recipe.CookUser.Avatar.SetSourceAsync(stream.AsRandomAccessStream());
                                }
                                recipe.RecipeID = id;
                                recipe.Title = title;
                                recipe.TitleImage = new BitmapImage(new System.Uri("ms-appx:///Assets/Image/Food_Sample (" + i % 15 + ").jpg"));

                                recipe.IngredientList = new ObservableCollection<Ingredient>();
                                if(!string.IsNullOrEmpty(checklist))
                                {
                                    var regredientArray = JsonArray.Parse(checklist);
                                    foreach (var ingredient in regredientArray)
                                    {
                                        var obj = ingredient.GetObject();
                                        var newIngredient = new Ingredient();
                                        newIngredient.IngredientName = JsonParser.GetStringFromJsonObj(obj["name"]);
                                        newIngredient.Quality = JsonParser.GetStringFromJsonObj(obj["quality"]);
                                        newIngredient.IsMain = JsonParser.GetBooleanFromJsonObj(obj["is_main"],true);
                                        recipe.IngredientList.Add(newIngredient);
                                    }
                                }
                                

                                recipe.StepsList = new ObservableCollection<Step>();
                                if(!string.IsNullOrEmpty(content))
                                {
                                    var stepArray = JsonArray.Parse(content);
                                    foreach (var step in stepArray)
                                    {
                                        var obj = step.GetObject();
                                        var newStep = new Step();
                                        newStep.ImageUrl=JsonParser.GetStringFromJsonObj(obj["image_url"]);
                                        newStep.StepContent = JsonParser.GetStringFromJsonObj(obj["content"]);
                                        recipe.StepsList.Add(newStep);
                                    }
                                }

                                listToReturn.Add(recipe);
                                i++;
                            }
                            catch (Exception e)
                            {
                                Messenger.Default.Send(new GenericMessage<string>(e.Message), "toast");
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
    }
}
