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



namespace YummyCookWindowsUniversal.ViewModel
{
    public class UserInfoViewModel:ViewModelBase, INavigable
    {
        private bool isToLogin = false;

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

        private RelayCommand _nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                if(_nextCommand!=null)
                {
                    return _nextCommand;
                }
                return _nextCommand=new RelayCommand(()=>
                {
                    if(isToLogin)
                    {
                        Login();

                    }
                    else
                    {
                        Register();
                    }
                });
            }
        }

        public UserInfoViewModel()
        {
            UserName = "";
            Password = "";
            ConfirmPassword = "";
            
        }

        private async void Login()
        {
            
        }

        private void Register()
        {
            var rootFrame = Window.Current.Content as Frame;
            if(rootFrame!=null)
            {
                rootFrame.Navigate(typeof(UserInfoPage));
            }
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
            
        }
    }
}
