using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace YummyCookWindowsUniversal.Model
{
    public class Recipe:ViewModelBase
    {
        private string _recipeID;
        public string RecipeID
        {
            get
            {
                return _recipeID;
            }
            set
            {
                if(_recipeID!=value)
                {
                    _recipeID = value;
                    RaisePropertyChanged(() => RecipeID);
                }
            }
        }

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

        private User _cookUser;
        public User CookUser
        {
            get
            {
                return _cookUser;
            }
            set
            {
                if (_cookUser != value)
                {
                    _cookUser = value;
                    RaisePropertyChanged(() => CookUser);
                }
                
            }
        }

        private BitmapImage _titleImage;
        public BitmapImage TitleImage
        {
            get
            {
                return _titleImage;
            }
            set
            {
                if (_titleImage != value)
                {
                    _titleImage = value;
                    RaisePropertyChanged(() => TitleImage);
                }

            }
        }

        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    RaisePropertyChanged(() => Content);
                }
            }
        }

        public Recipe()
        {
            TitleImage = new BitmapImage();
        }

    }
}
