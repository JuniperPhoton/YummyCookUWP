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

        private int _quality;
        public int Quality
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

        private bool _isChecked;
        public bool IsChecked
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

        public Ingredient(string name,int quality,bool ischeck=false)
        {
            this.IngredientName = name;
            this.Quality = quality;
            this.IsChecked = ischeck;
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
            var json = JsonMaker.MakeJsonString(new List<string> { name, quality });
            return json;
        }
    }
}
