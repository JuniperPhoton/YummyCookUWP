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

namespace YummyCookWindowsUniversal.ViewModel
{

    public class MainViewModel : ViewModelBase, INavigable
    {
        private int start = 0;
        private int number = 20;

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    if (value == "美食广场")
                    {
                        MainRadioChecked = true;
                        FavorRadioChecked = false;
                    }
                    else
                    {
                        MainRadioChecked = false;
                        FavorRadioChecked = true;
                    }
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        private System.Nullable<bool> _mainRadioChecked;
        public System.Nullable<bool> MainRadioChecked
        {
            get
            {
                return _mainRadioChecked;
            }
            set
            {
                if(_mainRadioChecked!=value)
                {
                    _mainRadioChecked = value;
                    RaisePropertyChanged(() => MainRadioChecked);
                }
            }
        }

        private System.Nullable<bool> _favorRadioChecked;
        public System.Nullable<bool> FavorRadioChecked
        {
            get
            {
                return _favorRadioChecked;
            }
            set
            {
                if(_favorRadioChecked != value)
                {
                    _favorRadioChecked = value;
                    RaisePropertyChanged(() => FavorRadioChecked);
                }
            }
        }

        private Visibility _showLoadingVisibility;
        public Visibility ShowLoadingVisibility
        {
            get
            {
                return _showLoadingVisibility;
            }
            set
            {
                if (_showLoadingVisibility != value)
                {
                    _showLoadingVisibility = value;
                    RaisePropertyChanged(() => ShowLoadingVisibility);
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

        private User _currentUser;
        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    RaisePropertyChanged(() => CurrentUser);
                }

            }
        }

        #region SplitView Pane involved

        private RelayCommand _modifyInfoCommand;
        public RelayCommand ModifyInfoCommand
        {
            get
            {
                if (_modifyInfoCommand != null) return _modifyInfoCommand;
                return _modifyInfoCommand = new RelayCommand(() =>
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(ModifyUserInfoPage), CurrentUser);
                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.HidePaneToken);
                });
            }
        }

        private RelayCommand _gotoMainCommand;
        public RelayCommand GotoMainCommand
        {
            get
            {
                if (_gotoMainCommand != null) return _gotoMainCommand;
                return _gotoMainCommand = new RelayCommand(() =>
                {
                    App.MainFrame.Navigate(typeof(HomePage));
                    Title = "美食广场";
                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.HidePaneToken);
                });
            }
        }

        private RelayCommand _gotoFavorCommand;
        public RelayCommand GotoFavorCommand
        {
            get
            {
                if (_gotoFavorCommand != null) return _gotoFavorCommand;
                return _gotoFavorCommand = new RelayCommand(() =>
                {
                    App.MainFrame.Navigate(typeof(FavorPage),CurrentUser);
                    Title = "我的收藏";
                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.HidePaneToken);
                });
            }
        }

        private RelayCommand _gotoAboutCommand;
        public RelayCommand GoToAboutCommand
        {
            get
            {
                if (_gotoAboutCommand != null) return _gotoAboutCommand;
                return _gotoAboutCommand = new RelayCommand(() =>
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(AboutPage));
                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.HidePaneToken);
                });
            }
        }

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand != null) return _logoutCommand;
                return _logoutCommand = new RelayCommand(async () =>
                {
                    MessageDialog md = new MessageDialog("注销后可以登录其他账户.", "注意");
                    md.Commands.Add(new UICommand("确认", act =>
                    {
                        LocalSettingHelper.CleanUpAll();
                        var rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(StartPage));
                    }));
                    md.Commands.Add(new UICommand("取消", act =>
                    {

                    }));
                    md.DefaultCommandIndex = 1;

                    await md.ShowAsync();
                });
            }
        }
        #endregion

        private RelayCommand<Recipe> _gotoDetailCommand;
        public RelayCommand<Recipe> GotoDetailCommand
        {
            get
            {
                if (_gotoDetailCommand != null) return _gotoDetailCommand;
                return _gotoDetailCommand = new RelayCommand<Recipe>((recipe) =>
                {
                    if (App.ContentFrame.Visibility == Visibility.Visible)
                    {
                        App.ContentFrame.Navigate(typeof(RecipeDetailPage));
                    }
                    else
                    {
                        var rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(RecipeDetailPage));
                    }
                    Messenger.Default.Send(new GenericMessage<Recipe>(recipe), MessengerToken.RecipeToken);
                });
            }
        }

        #region Commandbar involved


        private RelayCommand _addNewCommand;
        public RelayCommand AddNewCommand
        {
            get
            {
                if (_addNewCommand != null) return _addNewCommand;
                return _addNewCommand=new RelayCommand(()=>
                {
                    var rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(NewRecipePage));
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
                      start = 0;
                      number = 20;
                      //RecipeList = new ObservableCollection<Recipe>();
                      await GetRecipes();
                  });
            }
        }

        #endregion

        public MainViewModel()
        {
            RecipeList = new ObservableCollection<Recipe>();
            ShowLoadingVisibility = Visibility.Collapsed;

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.UpdateUserToken, async act =>
              {
                  await GetCurrentUser();
              });

            MainRadioChecked = true;
            FavorRadioChecked = false;
        }

        private async void InitialSampleData()
        {
            RecipeList = new ObservableCollection<Recipe>();
            for (int i = 0; i < 10; i++)
            {
                Recipe recipeToAdd = new Recipe();
                recipeToAdd.Title = "农家小炒肉";
                recipeToAdd.TitleImage = new BitmapImage(new System.Uri("ms-appx:///Assets/Image/Food_Sample (" + i % 6 + ").jpg"));
                recipeToAdd.CookUser = new User();
                recipeToAdd.CookUser.Avatar = new BitmapImage();
                var folder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                var folder2 = await folder.GetFolderAsync("Image");
                var file = await folder2.GetFileAsync("1.jpg");
                using (var stream = await file.OpenReadAsync())
                {
                    await recipeToAdd.CookUser.Avatar.SetSourceAsync(stream);
                }
                recipeToAdd.CookUser.UserName = "JuniperPhoton";
                RecipeList.Add(recipeToAdd);
            }
        }

        public async Task GetRecipes()
        {
            try
            {
                ShowLoadingVisibility = Visibility.Visible;
                var list = await RequestHelper.GetAllRecipesAsync(start.ToString(), number.ToString());
                start += number;
                this.RecipeList = list;
                ShowLoadingVisibility = Visibility.Collapsed;
                await LoadAllImge(this.RecipeList);
            }
            catch(Exception e)
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(e.Message), MessengerToken.ToastToken);
            }
        }

        private async Task LoadAllImge(ObservableCollection<Recipe> recipeList)
        {
            foreach(var item in recipeList)
            {
                if(!string.IsNullOrEmpty(item.TitleImageUrl))
                {
                    var stream = await RequestHelper.GetImageFromUrl(item.TitleImageUrl);
                    if(stream!=null)
                    {
                        await item.TitleImage.SetSourceAsync(stream);
                    }
                }
            }
        }

        public async Task GetCurrentUser()
        {
            var isLogin = await RequestHelper.Login(LocalSettingHelper.GetValue("username"), LocalSettingHelper.GetValue("password"));
            if(isLogin.IsSuccess)
            {
                
                var user = await RequestHelper.GetUserInfoAsync(LocalSettingHelper.GetValue("username"));
                if (user != null)
                {
                    await user.DownloadAvatar();
                    this.CurrentUser = user;
                    await this.CurrentUser.LoadRegionInfo();

                    Messenger.Default.Send(new GenericMessage<string>(user.UserName+",欢迎回来 :-)"), MessengerToken.ToastToken);
                }
            }
           else
            {
                Messenger.Default.Send(new GenericMessage<string>("登录失败"), MessengerToken.ToastToken);
            }
        }

        public void Activate(object param)
        {
            Title = "美食广场";
            if (RecipeList == null) RecipeList = new ObservableCollection<Recipe>();
            
            if (RecipeList.Count == 0)
            {
                var getRecipeTask=GetRecipes();
                var getUserTask=GetCurrentUser();
            }
        }

        public void Deactivate(object param)
        {

        }
    }
}