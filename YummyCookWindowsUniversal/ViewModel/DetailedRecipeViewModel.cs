using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.Model;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class DetailedRecipeViewModel:ViewModelBase,INavigable
    {

        private Visibility _showPictureVisibility;
        public Visibility ShowPictureVisibility
        {
            get
            {
                return _showPictureVisibility;
            }
            set
            {
                if (_showPictureVisibility != value)
                {
                    _showPictureVisibility = value;
                    RaisePropertyChanged(() => ShowPictureVisibility);
                }
            }
        }


        private Recipe _currentRecipe;
        public Recipe CurrentRecipe
        {
            get
            {
                return _currentRecipe;
            }
            set
            {
                if(_currentRecipe!=value)
                {
                    _currentRecipe = value;
                    RaisePropertyChanged(() => CurrentRecipe);
                }
            }
        }

        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get
            {
                return _currentImage;
            }
            set
            {
                if(_currentImage!=value)
                {
                    _currentImage = value;
                    RaisePropertyChanged(() => CurrentImage);
                }
            }
        }

        private RelayCommand _showPictureCommand;
        public RelayCommand ShowPictureCommand
        {
            get
            {
                if (_showPictureCommand != null) return _showPictureCommand;
                return _showPictureCommand = new RelayCommand(() =>
                {
                    this.CurrentImage = CurrentRecipe.TitleImage;
                    
                    ShowPictureVisibility = Visibility.Visible;

                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.ShowPictureToken);
                });
            }
        }

        private RelayCommand _hidePictureCommand;
        public RelayCommand HidePictureCommand
        {
            get
            {
                if (_hidePictureCommand != null) return _hidePictureCommand;
                return _hidePictureCommand = new RelayCommand(() =>
                  {
                      Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.HidePictureToken);

                      ShowPictureVisibility = Visibility.Collapsed;
                  });
            }
        }

        private RelayCommand _savePictureCommand;
        public RelayCommand SavePictureCommand
        {
            get
            {
                if (_savePictureCommand != null) return _savePictureCommand;
                return _savePictureCommand=new RelayCommand(()=>
                {
                    if(CurrentImage==null)
                    {
                        return;
                    }
                    return;
                });
            }
        }

        private RelayCommand<Ingredient> _checkItemCommand;
        public RelayCommand<Ingredient> CheckItemCommand
        {
            get
            {
                if(_checkItemCommand!=null)
                {
                    return _checkItemCommand;
                }
                return _checkItemCommand=new RelayCommand<Ingredient>((ingredient) =>
                {
                    ingredient.IsChecked = !ingredient.IsChecked;
                });
            }
        }

        private ObservableCollection<GroupInfoList> _checkListGrouped;
        public ObservableCollection<GroupInfoList> CheckListGrouped
        {
            get
            {
                return _checkListGrouped;
            }
            set
            {
                if(_checkListGrouped!=value)
                {
                    _checkListGrouped = value;
                    RaisePropertyChanged(() => CheckListGrouped);
                }
            }
        }

        public DetailedRecipeViewModel()
        {
            ShowPictureVisibility = Visibility.Collapsed;
        }

        public async Task LoadDetailPage()
        {
            try
            {
                var userName = this.CurrentRecipe.CookUser.UserName;
                var user = await RequestHelper.GetUserInfoAsync(userName);
                if (user != null)
                {
                    this.CurrentRecipe.CookUser = user;
                    await this.CurrentRecipe.CookUser.LoadRegionInfo();
                    
                }
            }
            catch(Exception e)
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(e.Message), MessengerToken.ToastToken);
            }
            

        }

        public static ObservableCollection<GroupInfoList> GetCheckListsGrouped(ObservableCollection<Ingredient> ingredients)
        {
            ObservableCollection<GroupInfoList> groups = new ObservableCollection<GroupInfoList>();
            
            var query = from item in ingredients
                        group item by item.IsMain into g
                        orderby g.Key
                        select new
                        {
                            Items = g,
                            GroupName = g.Key?"主料":"辅料"
                        };

            foreach (var g in query)
            {
                GroupInfoList info = new GroupInfoList();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
            }

            return groups;
        }

        public void Activate(object param)
        {
            Messenger.Default.Register<GenericMessage<Recipe>>(this, MessengerToken.RecipeToken, async act =>
             {
                 var recipe = act.Content;
                 if(recipe!=null)
                 {
                     this.CurrentRecipe = recipe;
                     this.CheckListGrouped=GetCheckListsGrouped(this.CurrentRecipe.IngredientList);
                 }
                 await LoadDetailPage();
             });
        }

        public void Deactivate(object param)
        {
           
        }
    }
}
