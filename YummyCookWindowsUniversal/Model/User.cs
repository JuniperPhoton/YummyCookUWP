using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.ViewModel;

namespace YummyCookWindowsUniversal.Model
{
    public class User:ViewModelBase
    {
        public string AvatarUrl { get; set; }

        private string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged(() => ID);
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if(_userName!=value)
                {
                    _userName = value;
                    RaisePropertyChanged(() => UserName);
                }
            }
        }

        private BitmapImage _avatar;
        public BitmapImage Avatar
        {
            get
            {
                return _avatar;
            }
            set
            {
                if (_avatar != value)
                {
                    _avatar = value;
                    RaisePropertyChanged(() => Avatar);
                }
            }
        }

        private int _gender;
        public int Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    RaisePropertyChanged(() => Gender);
                }
            }
        }

        private int _provinceID;
        public int ProvinceID
        {
            get
            {
                return _provinceID;
            }
            set
            {
                if(_provinceID!=value)
                {
                    _provinceID = value;
                    RaisePropertyChanged(() => ProvinceID);
                }
            }
        }
        private int _cityID;
        public int CityID
        {
            get
            {
                return _cityID;
            }
            set
            {
                if (_cityID != value)
                {
                    _cityID = value;
                    RaisePropertyChanged(() => CityID);
                }
            }
        }

        private string _regionName;
        public string RegionName
        {
            get
            {
                return _regionName;
            }
            set
            {
                if (_regionName != value)
                {
                    _regionName = value;
                    RaisePropertyChanged(() => RegionName);
                }
            }
        }

        private List<string> _friendsIDList;
        public List<string> FriendsIDList
        {
            get
            {
                return _friendsIDList;
            }
            set
            {
                if(_friendsIDList!=value)
                {
                    _friendsIDList = value;
                    RaisePropertyChanged(() => FriendsIDList);
                }
            }
        }

        private List<string> _favorsIDList;
        public List<string> FavorsIDList
        {
            get
            {
                return _favorsIDList;
            }
            set
            {
                if (_favorsIDList != value)
                {
                    _favorsIDList = value;
                    RaisePropertyChanged(() => FavorsIDList);
                }
            }
        }

        public User()
        {
            FriendsIDList = new List<string>();
            FavorsIDList = new List<string>();
            RegionName = "";
            ID = "";
        }

        public static async Task<User> PopulateInstanceFromJson(IJsonValue value)
        {
            try
            {
                User user = new User();
                user.UserName = JsonParser.GetStringFromJsonObj(value, "username");
                user.Gender = int.Parse(JsonParser.GetStringFromJsonObj(value, "gender"));
                user.ID = JsonParser.GetStringFromJsonObj(value, "objectId");
                user.ProvinceID = int.Parse(JsonParser.GetStringFromJsonObj(value, "province_id"));
                user.CityID = int.Parse(JsonParser.GetStringFromJsonObj(value, "city_id"));
                user.AvatarUrl = JsonParser.GetStringFromJsonObj(value, "avatar_url");

                var friendsList = JsonParser.GetStringFromJsonObj(value, "friends_list");
                if (!string.IsNullOrEmpty(friendsList))
                {
                    var list = friendsList.Split(',');
                    foreach (var follow in list)
                    {
                        user.FriendsIDList.Add(follow);
                    }
                }

                var favorsList = JsonParser.GetStringFromJsonObj(value, "favors_list");
                if (!string.IsNullOrEmpty(favorsList))
                {
                    var list = favorsList.Split(',');
                    foreach (var favor in list)
                    {
                        user.FavorsIDList.Add(favor);
                    }
                }

                var avatarUrl = JsonParser.GetStringFromJsonObj(value, "avatar_url");
                //Get avatar of the user
                if (string.IsNullOrEmpty(avatarUrl))
                {
                    var folder1 = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                    var folder2 = await folder1.GetFolderAsync("Icon");
                    var file = await folder2.GetFileAsync("DefaultAccount.png");
                    using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        user.Avatar = new BitmapImage();
                        await user.Avatar.SetSourceAsync(fileStream);
                    }
                }
                return user;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task DownloadAvatar()
        {
            if(!string.IsNullOrEmpty(this.AvatarUrl))
            {
                var stream = await RequestHelper.GetImageFromUrl(this.AvatarUrl);
                this.Avatar = new BitmapImage();
                await this.Avatar.SetSourceAsync(stream);
            }
            
        }

        public async Task LoadRegionInfo()
        {
            try
            {
                var locator = App.Current.Resources["Locator"] as ViewModelLocator;
                if (locator.UserInfoVM.RegionsList.ProvinceList.Count == 0)
                {
                    await locator.UserInfoVM.InitialProvinceAsync();
                }
                var provinceName = locator.UserInfoVM.RegionsList.ProvinceList.ToList().Find((s) =>
                {
                    if (s.ProvinceID == this.ProvinceID)
                    {
                        return true;
                    }
                    else return false;
                }).ProvinceName;

                await locator.UserInfoVM.RegionsList.LoadCityList(this.ProvinceID);
                var cityName = locator.UserInfoVM.RegionsList.CityList.ToList().Find((c) =>
                {
                    if (c.CityID == this.CityID)
                    {
                        return true;
                    }
                    else return false;
                }).CityName;

                if (cityName == provinceName)
                {
                    this.RegionName = cityName;
                }
                else this.RegionName = provinceName + " " + cityName;
            }
            catch(Exception e)
            {

            }
            
        }

        
    }
}
