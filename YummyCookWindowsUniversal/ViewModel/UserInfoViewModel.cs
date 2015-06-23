using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.Model;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class UserInfoViewModel:ViewModelBase, INavigable
    {
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

        public string CurrentPageKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            throw new NotImplementedException();
        }

        public void Activate(object param)
        {
            var data = param as NavigationData;
            string isLogin = (string)data.paramsData["ToLogin"];
            if(isLogin=="true")
            {
                Title = "LOGIN";
                ShowConfirmVisibility = Visibility.Collapsed;
                LoginBtnContent = "LOGIN";
            }
            else
            {
                Title = "REGISTER";
                ShowConfirmVisibility = Visibility.Visible;
                LoginBtnContent = "REGISTER";
            }
        }

        public void Deactivate(object param)
        {
            throw new NotImplementedException();
        }
    }
}
