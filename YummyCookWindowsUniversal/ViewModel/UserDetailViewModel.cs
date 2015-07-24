using GalaSoft.MvvmLight;
using JP.Utils.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.Model;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class UserDetailViewModel : ViewModelBase, INavigable
    {
        private bool isLoadRecipes = false;
        private bool isLoadFriends = false;
        private bool isLoadFavors = false;

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    switch (value)
                    {
                        case 0:
                            {
                               if(!isLoadRecipes)
                                {
                                    var task0 = LoadRecipesList();
                                    isLoadRecipes = true;
                                }
                            }; break;
                        case 1:
                            {
                                if (!isLoadFriends)
                                {
                                    var task1 = LoadUserList();
                                    isLoadFriends = true;
                                }
                               
                            }; break;
                        case 2:
                            {
                                if (!isLoadFavors)
                                {
                                    var task2 = LoadFavorsList();
                                    isLoadFavors = true;
                                }
                            };break;
                    }
                    RaisePropertyChanged(() => SelectedIndex);
                }
            }
        }

        private User _currentUser;
        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                if(_currentUser!=value)
                {
                    _currentUser = value;
                    RaisePropertyChanged(() => CurrentUser);
                }
            }
        }

        private Visibility _showNoRecipesVisibility;
        public Visibility ShowNoRecipesVisibility
        {
            get
            {
                return _showNoRecipesVisibility;
            }
            set
            {
                if (_showNoRecipesVisibility != value)
                {
                    _showNoRecipesVisibility = value;
                    RaisePropertyChanged(() => ShowNoRecipesVisibility);
                }

            }
        }

        private Visibility _showNoFriendsVisibility;
        public Visibility ShowNoFriendsVisibility
        {
            get
            {
                return _showNoFriendsVisibility;
            }
            set
            {
                if (_showNoFriendsVisibility != value)
                {
                    _showNoFriendsVisibility = value;
                    RaisePropertyChanged(() => ShowNoFriendsVisibility);
                }

            }
        }

        private Visibility _showNoFavorsVisibility;
        public Visibility ShowNoFavorsVisibility
        {
            get
            {
                return _showNoFavorsVisibility;
            }
            set
            {
                if (_showNoFavorsVisibility != value)
                {
                    _showNoFavorsVisibility = value;
                    RaisePropertyChanged(() => ShowNoFavorsVisibility);
                }

            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                if(_isLoading!=value)
                {
                    _isLoading = value;
                    RaisePropertyChanged(() => IsLoading);
                }
            }
        }

        private ObservableCollection<Recipe> _recipesList;
        public ObservableCollection<Recipe> RecipesList
        {
            get
            {
                return _recipesList;
            }
            set
            {
                if (_recipesList != value)
                {
                    _recipesList = value;
                    RaisePropertyChanged(() => RecipesList);
                }
            }
        }

        private ObservableCollection<User> _friendsList;
        public ObservableCollection<User> FriendsList
        {
            get
            {
                return _friendsList;
            }
            set
            {
                if (_friendsList != value)
                {
                    _friendsList = value;
                    RaisePropertyChanged(() => FriendsList);
                }
            }
        }

        private ObservableCollection<Recipe> _favorsList;
        public ObservableCollection<Recipe> FavorsList
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


        public UserDetailViewModel()
        {
            CurrentUser = new User();
            RecipesList = new ObservableCollection<Recipe>();
            FavorsList = new ObservableCollection<Recipe>();
            FriendsList = new ObservableCollection<User>();

            ShowNoFavorsVisibility = Visibility.Collapsed;
            ShowNoFriendsVisibility = Visibility.Collapsed;
            ShowNoRecipesVisibility = Visibility.Collapsed;

            IsLoading = false;
        }

        private async Task LoadRecipesList()
        {
            try
            {
                IsLoading = true;

                var result = await RequestHelper.GetRecipesByUserAsync(CurrentUser.UserName);
                if(result!=null)
                {
                    RecipesList = result;
                }
                await LoadAllImge(RecipesList);

                if(RecipesList.Count==0)
                {
                    ShowNoRecipesVisibility = Visibility.Visible;
                }

                IsLoading = false;
            }
            catch(Exception)
            {

            }
        }

        private async Task LoadUserList()
        {
            try
            {
                IsLoading = true;

                foreach (var friendID in CurrentUser.FriendsIDList)
                {
                    var newUser = await RequestHelper.GetUserInfoAsync(friendID, false);
                    if (newUser != null)
                    {
                        await newUser.DownloadAvatar();
                        FriendsList.Add(newUser);
                    }
                }

                if(FriendsList.Count==0)
                {
                    ShowNoFriendsVisibility = Visibility.Visible;
                }

                IsLoading = false;
            }
            catch (Exception)
            {

            }
        }

        private async Task LoadFavorsList()
        {
            try
            {
                IsLoading = true;

                foreach (var recipeID in CurrentUser.FavorsIDList)
                {
                    var newRecipe = await RequestHelper.GetRecipeAsync(recipeID);
                    if (newRecipe != null)
                    {
                        RecipesList.Add(newRecipe);
                    }
                }
                await LoadAllImge(this.FavorsList);

                if(FavorsList.Count==0)
                {
                    ShowNoFavorsVisibility = Visibility.Visible;
                }

                IsLoading = false;
            }
            catch (Exception)
            {

            }
        }

        private async Task LoadAllImge(ObservableCollection<Recipe> recipeList)
        {
            foreach (var item in recipeList)
            {
                if (!string.IsNullOrEmpty(item.TitleImageUrl))
                {
                    var stream = await RequestHelper.GetImageFromUrl(item.TitleImageUrl);
                    if (stream != null)
                    {
                        await item.TitleImage.SetSourceAsync(stream);
                    }
                }
            }
        }

        public void Activate(object param)
        {
            if(param is User)
            {
                CurrentUser = param as User;
                if(!isLoadRecipes)
                {
                    var task=LoadRecipesList();
                    isLoadRecipes = true;
                }
            }
        }

        public void Deactivate(object param)
        {
            
        }
    }
}
