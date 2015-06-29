using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YummyCookWindowsUniversal.Helper;

namespace YummyCookWindowsUniversal.Model
{
    public class Ingredient : ViewModelBase
    {
        private string _ingredientName;
        public string IngredientName
        {
            get
            {
                return _ingredientName;
            }
            set
            {
                if (_ingredientName != value)
                {
                    _ingredientName = value;
                    RaisePropertyChanged(() => IngredientName);
                }
            }
        }

        private string _quality;
        public string Quality
        {
            get
            {
                return _quality;
            }
            set
            {
                if (_quality != value)
                {
                    _quality = value;
                    RaisePropertyChanged(() => Quality);
                }
            }
        }

        private System.Nullable<bool> _isChecked;
        public System.Nullable<bool> IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    RaisePropertyChanged(() => IsChecked);
                }
            }
        }

        private bool _isMain;
        public bool IsMain
        {
            get
            {
                return _isMain;
            }
            set
            {
                if(_isMain!=value)
                {
                    _isMain = value;
                    RaisePropertyChanged(() => IsMain);
                }
            }
        }

        public Ingredient()
        {
            IsChecked = false;
            IsMain = true;
        }

        public Ingredient(string name,string quality,bool ischeck=false,bool ismain=true)
        {
            this.IngredientName = name;
            this.Quality = quality;
            this.IsChecked = ischeck;
            this.IsMain = ismain;
        }

        /// <summary>
        /// 获得用于上传服务器的JSON 内容
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string MakeJson(Ingredient item)
        {
            var name=JsonMaker.MakeJsonObj("name", item.IngredientName);
            var quality = JsonMaker.MakeJsonObj("quality", item.Quality);
            var main = JsonMaker.MakeJsonObj("is_main", item.IsMain);
            var json = JsonMaker.MakeJsonString(new List<string> { name, quality,main});
            return json;
        }
    }
}
