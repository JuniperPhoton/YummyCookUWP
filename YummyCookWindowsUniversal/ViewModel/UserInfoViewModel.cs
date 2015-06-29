using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.Model;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Popups;
using GalaSoft.MvvmLight.Messaging;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using JP.Utils.Data;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class UserInfoViewModel:ViewModelBase, INavigable
    {
        private bool isToLogin = false;

        private StorageFile tempFile;

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if(_title!=value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        private Visibility _showConfirmVisibility;
        public Visibility ShowConfirmVisibility
        {
            get
            {
                return _showConfirmVisibility;
            }
            set
            {
                if (_showConfirmVisibility != value)
                {
                    _showConfirmVisibility = value;
                    RaisePropertyChanged(() => ShowConfirmVisibility);
                }
            }
        }

        private Visibility _showLoadingVisibility;
        public Visibility ShowLoadingVisibility
        {
            get
            {
                return _showLoadingVisibility;
            }
            set
            {
                if(_showLoadingVisibility!=value)
                {
                    _showLoadingVisibility = value;
                    RaisePropertyChanged(() => ShowLoadingVisibility);
                }
            }
        }

        private string _loginBtnContent;
        public string LoginBtnContent
        {
            get
            {
                return _loginBtnContent;
            }
            set
            {
                if (_loginBtnContent != value)
                {
                    _loginBtnContent = value;
                    RaisePropertyChanged(() => LoginBtnContent);
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

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if(_password!=value)
                {
                    _password = value;
                    RaisePropertyChanged(() => Password);
                }
            }
        }

        private string _confrimPassword;
        public string ConfirmPassword
        {
            get
            {
                return _confrimPassword;
            }
            set
            {
                if(_confrimPassword!=value)
                {
                    _confrimPassword = value;
                    RaisePropertyChanged(() => ConfirmPassword);
                }
            }
        }

        private BitmapImage _tempAvatar;
        public BitmapImage TempAvatar
        {
            get
            {
                return _tempAvatar;
            }
            set
            {
                if(_tempAvatar!=value)
                {
                    _tempAvatar = value;
                    RaisePropertyChanged(() => TempAvatar);
                }
            }
        }

        private RegionList _regionsList;
        public RegionList RegionsList
        {
            get
            {
                return _regionsList;
            }
            set
            {
                if(_regionsList!=value)
                {
                    _regionsList = value;
                    RaisePropertyChanged(() => RegionsList);
                }
            }
        }

        private int _selectedGender;
        public int SelectedGender
        {
            get
            {
                return _selectedGender;
            }
            set
            {
                if (_selectedGender != value)
                {
                    _selectedGender = value;
                    RaisePropertyChanged(() => SelectedGender);
                }
            }
        }

        private int _selectedProvinceID;
        public int SelectedProvinceID
        {
            get
            {
                return _selectedProvinceID;
            }
            set
            {
                if (_selectedProvinceID != value)
                {
                    _selectedProvinceID = value;
                    LoadCity(value+1);
                    RaisePropertyChanged(() => SelectedProvinceID);
                }
            }
        }

        private int _selectedCity;
        public int SelectedCityID
        {
            get
            {
                return _selectedCity;
            }
            set
            {
                if(_selectedCity != value)
                {
                    _selectedCity = value;
                    RaisePropertyChanged(() => SelectedCityID);
                } 
            }
        }

        private RelayCommand _nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                if(_nextCommand!=null)
                {
                    return _nextCommand;
                }
                return _nextCommand=new RelayCommand(async()=>
                {
                    if(isToLogin)
                    {
                        await Login();

                    }
                    else
                    {
                        await Register();
                    }
                });
            }
        }

        private RelayCommand _updateCommand;
        public RelayCommand UpdateCommand
        {
           get
            {
                if (_updateCommand != null) return _updateCommand;
                return _updateCommand = new RelayCommand(async() =>
                {
                   await UpdateContent();
                });
            }
        }


        private RelayCommand _modifyLaterCommand;
        public RelayCommand ModifyLaterCommand
        {
            get
            {
                if (_modifyLaterCommand != null) return _modifyLaterCommand;
                return _modifyLaterCommand = new RelayCommand(() =>
                  {
                      var rootFrame = Window.Current.Content as Frame;
                      rootFrame.Navigate(typeof(MainPage));
                  });
            }
        }

        private RelayCommand _selectAvatarCommand;
        public RelayCommand SelectAvatarCommand
        {
            get
            {
                if (_selectAvatarCommand != null) return _selectAvatarCommand;
                return _selectAvatarCommand = new RelayCommand(async() =>
                  {
                      var picker = new FileOpenPicker();
                      picker.FileTypeFilter.Add(".jpg");
                      picker.FileTypeFilter.Add(".png");
                      picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                      var file = await picker.PickSingleFileAsync();
                      var stream = await file.OpenStreamForReadAsync();
                      this.tempFile = file;
                      this.TempAvatar = new BitmapImage();
                      await this.TempAvatar.SetSourceAsync(stream.AsRandomAccessStream());
                  });
            }
        }

        public UserInfoViewModel()
        {
            UserName = "";
            Password = "";
            ConfirmPassword = "";
            ShowLoadingVisibility = Visibility.Collapsed;
            RegionsList = new RegionList();
            SelectedGender = 0;
        }

        private async Task Login()
        {
            if (!ValidInput())
            {
                return;
            }
            ShowLoadingVisibility = Visibility.Visible;
            var result = await RequestHelper.Login(UserName, Password);
            if(result.IsSuccess)
            {
                ShowLoadingVisibility = Visibility.Collapsed;
                var rootFrame = Window.Current.Content as Frame;
                if (rootFrame != null)
                {
                    rootFrame.Navigate(typeof(MainPage));
                }
            }
            else
            {
                ShowLoadingVisibility = Visibility.Collapsed;
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(result.ErrorMessage), "toast");
                return;
            }
        }

        private async Task Register()
        {
            if(!ValidInput())
            {
                return;
            }
            ShowLoadingVisibility = Visibility.Visible;
            var result = await RequestHelper.RegisterUser(UserName, Password);
            if (result.IsSuccess)
            {
                ShowLoadingVisibility = Visibility.Collapsed;
                var rootFrame = Window.Current.Content as Frame;
                if (rootFrame != null)
                {
                    await RegionsList.LoadProvinceList();
                    await RegionsList.LoadCityList(1);
                    SelectedProvinceID = 0;
                    SelectedCityID = 0;
                    TempAvatar = new BitmapImage(new Uri("ms-appx://Assets/Icon/Account.png"));
                    rootFrame.Navigate(typeof(UserInfoPage));
                }
            }
            else
            {
                ShowLoadingVisibility = Visibility.Collapsed;
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(result.ErrorMessage), "toast");
                return;
            }
        }

        private async Task UpdateContent()
        {
            ShowLoadingVisibility = Visibility.Visible;

            string resultUrl = null;

            if(tempFile!=null)
            {
                var fileStream = await tempFile.OpenStreamForReadAsync();
                resultUrl = await RequestHelper.UploadAvatar(LocalSettingHelper.GetValue("userid"), fileStream);
            }

            var param= new Dictionary<string, string>();
            param.Add("gender", SelectedGender.ToString());
            param.Add("city_id", SelectedCityID.ToString());
            param.Add("province_id", SelectedProvinceID.ToString());
            if(resultUrl!=null)
            {
                param.Add("avatar_url", resultUrl);
            }

            var result_update = await RequestHelper.UpdateUserInfo(LocalSettingHelper.GetValue("userid"), param);
            ShowLoadingVisibility = Visibility.Collapsed;
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));

        }

        private async void LoadCity(int id)
        {
            await RegionsList.LoadCityList(id);
            SelectedCityID = 0;
        }

        private bool ValidInput()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("请输入用户名"), "toast");
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("请输入密码"), "toast");
                return false;
            }
            if(!isToLogin)
            {
                if (Password != ConfirmPassword)
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("两次密码不一致"), "toast");
                    return false;
                }
            }

            return true;
        }

        public void Activate(object param)
        {
            var data = param as NavigationData;
            if (data == null) return;
            string isLogin = (string)data.paramsData["ToLogin"];
            if(isLogin=="true")
            {
                isToLogin = true;
                Title = "登录";
                ShowConfirmVisibility = Visibility.Collapsed;
                LoginBtnContent = "登录";
            }
            else
            {
                isToLogin = false;
                Title = "注册";
                ShowConfirmVisibility = Visibility.Visible;
                LoginBtnContent = "注册";
            }
        }

        public void Deactivate(object param)
        {
            UserName = "";
            Password = "";
            ConfirmPassword = "";
            
        }
    }
}
