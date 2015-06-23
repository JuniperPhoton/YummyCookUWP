using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace YummyCookWindowsUniversal.Model
{
    public class User:ViewModelBase
    {
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

        private ImageSource _avatar;
        public ImageSource Avatar
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

        private List<string> _friendsList;
        public List<string> FriendsList
        {
            get
            {
                return _friendsList;
            }
            set
            {
                if(_friendsList!=value)
                {
                    _friendsList = value;
                    RaisePropertyChanged(() => FriendsList);
                }
            }
        }

        private List<string> _favorsList;
        public List<string> FavorsList
        {
            get
            {
                return _favorsList;
            }
            set
            {
                if (_favorsList != value)
                {
                    _favorsList = value;
                    RaisePropertyChanged(() => FavorsList);
                }
            }
        }

        public User()
        {
            FriendsList = new List<string>();
            FavorsList = new List<string>();
        }

    }
}
