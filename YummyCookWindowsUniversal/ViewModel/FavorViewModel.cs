using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Media.Imaging;
using YummyCookWindowsUniversal.Model;
using System;
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.Helper;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using JP.Utils.Data;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using YummyCookWindowsUniversal.View;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System.Threading.Tasks;
using JP.Utils.Network;
using System.Collections.Generic;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class FavorViewModel: ViewModelBase, INavigable
    {
        private bool isLoadFriends = false;
        private bool isLoadRecipe = false;

        private List<string> friendIDList;
        public List<string> recipeIDList;

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if(_selectedIndex!=value)
                {
                    _selectedIndex = value;
                    switch(value)
                    {
                        case 0:
                            {
                                if (!isLoadFriends)
                                {
                                    var task1=LoadUserList();
                                }
                            };break;
                        case 1:
                            {
                                if(!isLoadRecipe)
                                {
                                   var task2= LoadRecipeList();
                                }
                            }; break;
                    }
                    RaisePropertyChanged(() => SelectedIndex);
                }
            }
        }

        private ObservableCollection<Recipe> _recipeList;
        public ObservableCollection<Recipe> RecipeList
        {
            get
            {
                return _recipeList;
            }
            set
            {
                if (_recipeList != value)
                {
                    _recipeList = value;
                    RaisePropertyChanged(() => RecipeList);
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

        private RelayCommand<Recipe> _gotoDetailCommand;
        public RelayCommand<Recipe> GotoDetailCommand
        {
            get
            {
                if (_gotoDetailCommand != null) return _gotoDetailCommand;
                return _gotoDetailCommand = new RelayCommand<Recipe>((recipe) =>
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(RecipeDetailPage));

                    Messenger.Default.Send(new GenericMessage<Recipe>(recipe), MessengerToken.RecipeToken);
                });
            }
        }

        private RelayCommand<User> _gotoUserDetailCommand;
        public RelayCommand<User> GotoUserDetailCommand
        {
            get
            {
                if (_gotoUserDetailCommand != null) return _gotoUserDetailCommand;
                return _gotoUserDetailCommand = new RelayCommand<User>((user) =>
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(UserInfoPage),user);
                });
            }
        }

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand != null) return _refreshCommand;
                return _refreshCommand = new RelayCommand(async() =>
                  {
                      var user = await RequestHelper.GetUserInfoAsync(LocalSettingHelper.GetValue("username"));
                      if(user!=null)
                      {
                          var locator = App.Current.Resources["Locator"] as ViewModelLocator;
                          locator.MainVM.CurrentUser = user;
                          this.friendIDList = user.FriendsIDList;
                          this.recipeIDList = user.FavorsIDList;

                          if (SelectedIndex == 0)
                          {
                              FriendsList.Clear();
                              await LoadUserList();
                          }
                          else
                          {
                              RecipeList.Clear();
                              await LoadRecipeList();
                          }
                      }
                  });
            }
        }

        public FavorViewModel()
        {
            FriendsList = new ObservableCollection<User>();
            RecipeList = new ObservableCollection<Recipe>();
        }

        private async Task LoadUserList()
        {
            try
            {
                foreach(var friendID in friendIDList)
                {
                    var newUser = await RequestHelper.GetUserInfoAsync(friendID,false);
                    if(newUser!=null)
                    {
                        await newUser.DownloadAvatar();
                        FriendsList.Add(newUser);
                        isLoadFriends = true;
                    }
                }
            }
            catch(Exception)
            {

            }
        }

        private async Task LoadRecipeList()
        {
            try
            {
                foreach (var recipeID in recipeIDList)
                {
                    var newRecipe = await RequestHelper.GetRecipeAsync(recipeID);
                    if (newRecipe != null)
                    {
                        RecipeList.Add(newRecipe);
                        isLoadRecipe = true;
                    }
                }
                await LoadAllImge(this.RecipeList);
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
                if(!isLoadFriends)
                {
                    var user = param as User;
                    
                    if (user != null)
                    {
                        this.friendIDList = user.FriendsIDList;
                        this.recipeIDList = user.FavorsIDList;
                        var loadTask=LoadUserList();
                    }
                }
            }
        }

        public void Deactivate(object param)
        {
            
        }
    }
}
