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

namespace YummyCookWindowsUniversal.ViewModel
{

    public class MainViewModel : ViewModelBase, INavigable
    {
        private int start = 0;
        private int number = 20;

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

        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand != null) return _refreshCommand;
                return _refreshCommand = new RelayCommand(() =>
                  {
                      start = 0;
                      number = 20;
                      RecipeList.Clear();
                      GetRecipes();
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

        public MainViewModel()
        {
            RecipeList = new ObservableCollection<Recipe>();
            ShowLoadingVisibility = Visibility.Collapsed;
        }

        public async void InitialSampleData()
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

        public async void GetRecipes()
        {
            ShowLoadingVisibility = Visibility.Visible;
            var list = await RequestHelper.GetAllRecipes(start.ToString(), number.ToString());
            start += number;
            this.RecipeList = list;
            ShowLoadingVisibility = Visibility.Collapsed;

        }

        public void Activate(object param)
        {
            GetRecipes();
        }

        public void Deactivate(object param)
        {

        }
    }
}