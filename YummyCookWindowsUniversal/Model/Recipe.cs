using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Interface;

namespace YummyCookWindowsUniversal.Model
{
    public class Recipe:ViewModelBase, ICanMakeJson
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

        public string TitleImageUrl;

        private ObservableCollection<Ingredient> _ingredientList;
        public ObservableCollection<Ingredient> IngredientList
        {
            get
            {
                return _ingredientList;
            }
            set
            {
                if(_ingredientList!=value)
                {
                    _ingredientList = value;
                    RaisePropertyChanged(() => IngredientList);
                }
            }
        }

        private ObservableCollection<Step> _stepsList;
        public ObservableCollection<Step> StepsList
        {
            get
            {
                return _stepsList;
            }
            set
            {
                if (_stepsList != value)
                {
                    _stepsList = value;
                    RaisePropertyChanged(() => StepsList);
                }
            }
        }

        public Recipe()
        {
            TitleImage = new BitmapImage();
            IngredientList = new ObservableCollection<Ingredient>();
            StepsList = new ObservableCollection<Step>();
            Title = "";
            CookUser = new User();
        }

        public string MakeJson()
        {
            var ingredientArray= new StringBuilder();
            ingredientArray.Append("[");
            for(int i=0;i<this.IngredientList.Count;i++)
            {
                var thisItem = this.IngredientList.ElementAt(i);
                var objJson = thisItem.MakeJson();
                ingredientArray.Append(objJson);
                if (i != IngredientList.Count - 1) ingredientArray.Append(",");
            }
            ingredientArray.Append("]");

            var stepArray = new StringBuilder();
            stepArray.Append("[");
            for (int i = 0; i < this.StepsList.Count; i++)
            {
                var thisItem = this.StepsList.ElementAt(i);
                var objJson = thisItem.MakeJson();
                stepArray.Append(objJson);
                if (i != StepsList.Count - 1) stepArray.Append(",");
            }
            stepArray.Append("]");

            var title = JsonMaker.MakeJsonObj("title", this.Title);
            var titleImage = JsonMaker.MakeJsonObj("title_img", this.TitleImageUrl);
            var checklist = JsonMaker.MakeJsonObj("checklist", ingredientArray.ToString());
            var steplist = JsonMaker.MakeJsonObj("content", stepArray.ToString());
            var name = JsonMaker.MakeJsonObj("username", CookUser.UserName);

            var allJson = JsonMaker.MakeJsonString(new List<string> { title, titleImage, checklist, steplist,name });
            return allJson;
        }
    }
}
