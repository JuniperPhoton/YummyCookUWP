using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using JP.Utils.Data;
using JP.Utils.Framework;
using JP.Utils.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.Model;
using YummyCookWindowsUniversal.View;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class DetailedRecipeViewModel:ViewModelBase,INavigable
    {
        CancellationTokenSource _cts;

        private Visibility _showLargePictureVisibility;
        public Visibility ShowLargePictureVisibility
        {
            get
            {
                return _showLargePictureVisibility;
            }
            set
            {
                if (_showLargePictureVisibility != value)
                {
                    _showLargePictureVisibility = value;
                    RaisePropertyChanged(() => ShowLargePictureVisibility);
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

        private RelayCommand _goToUserInfoCommand;
        public RelayCommand GoToUserInfoCommand
        {
            get
            {
                if (_goToUserInfoCommand != null) return _goToUserInfoCommand;
                return _goToUserInfoCommand = new RelayCommand(() =>
                  {
                      var rootFrame = Window.Current.Content as Frame;
                      rootFrame.Navigate(typeof(UserInfoPage), CurrentRecipe.CookUser);
                  });
            }
        }

        private RelayCommand _launchMapCommand;
        public RelayCommand LaunchMapCommand
        {
            get
            {
                if (_launchMapCommand != null) return _launchMapCommand;
                return _launchMapCommand = new RelayCommand(async() =>
                  {
                      await GetLoacationAndShowAsync();
                  });
            }
        }

        private RelayCommand _cancelLaunchMapCommand;
        public RelayCommand CancelLaunchMapCommand
        {
            get
            {
                if (_cancelLaunchMapCommand != null) return _cancelLaunchMapCommand;
                return _cancelLaunchMapCommand = new RelayCommand(() =>
                {
                    if(_cts!=null)
                    {
                        _cts.Cancel();
                        _cts = null;
                        ShowLoadingVisibility = Visibility.Collapsed;
                    }
                });
            }
        }

        private RelayCommand<BitmapImage> _showLargePictureCommand;
        public RelayCommand<BitmapImage> ShowLargePictureCommand
        {
            get
            {
                if (_showLargePictureCommand != null) return _showLargePictureCommand;
                return _showLargePictureCommand = new RelayCommand<BitmapImage>((img) =>
                {
                    
                    this.CurrentImage = img;
                    
                    ShowLargePictureVisibility = Visibility.Visible;

                    Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.ShowPictureToken);
                });
            }
        }

        private RelayCommand _hideLargePictureCommand;
        public RelayCommand HideLargePictureCommand
        {
            get
            {
                if (_hideLargePictureCommand != null) return _hideLargePictureCommand;
                return _hideLargePictureCommand = new RelayCommand(() =>
                  {
                      Messenger.Default.Send(new GenericMessage<string>(""), MessengerToken.HidePictureToken);

                      ShowLargePictureVisibility = Visibility.Collapsed;
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

        #region Command bar involved

        private RelayCommand _toggleAddUserCommand;
        public RelayCommand ToggleAddUserCommand
        {
            get
            {
                if (_toggleAddUserCommand != null) return _toggleAddUserCommand;
                return _toggleAddUserCommand = new RelayCommand(async() =>
                  {
                      var id = CurrentRecipe.CookUser.ID;
                      var mainVM = (App.Current.Resources["Locator"] as ViewModelLocator).MainVM;

                      if(id==mainVM.CurrentUser.ID)
                      {
                          Messenger.Default.Send(new GenericMessage<string>("无法关注你自己 ;)"), MessengerToken.ToastTokenFollow);
                          return;
                      }
                      //Check if it's already followed.
                      var findID = mainVM.CurrentUser.FriendsIDList.Find(s =>
                        {
                            if (s == id) return true;
                            else return false;
                        });
                      if(findID!=null)
                      {
                          MessageDialog md = new MessageDialog("关注该用户，可以方便你以后查找他（她）的亨饪攻略。", "取消关注");
                          md.Commands.Add(new UICommand("取消关注",async act =>
                           {
                               mainVM.CurrentUser.FriendsIDList.Remove(findID);
                               await UpdateFriendList();
                           }));
                          md.Commands.Add(new UICommand("按错了", act =>
                          {
                              return;
                          }));
                          md.DefaultCommandIndex = 1;
                          await md.ShowAsync();
                      }
                      else
                      {
                          mainVM.CurrentUser.FriendsIDList.Add(id);
                          await UpdateFriendList();
                      }
                      
                  });
            }
        }

        private RelayCommand _toggleAddFavorCommand;
        public RelayCommand ToggleAddFavorCommand
        {
            get
            {
                if (_toggleAddFavorCommand != null) return _toggleAddFavorCommand;
                return _toggleAddFavorCommand = new RelayCommand(async() =>
                {
                    var id = CurrentRecipe.RecipeID;
                    var mainVM = (App.Current.Resources["Locator"] as ViewModelLocator).MainVM;

                    //Check if it's already favored.
                    var findID = mainVM.CurrentUser.FavorsIDList.Find(s =>
                    {
                        if (s == id) return true;
                        else return false;
                    });
                    if (findID != null)
                    {
                        MessageDialog md = new MessageDialog("", "取消收藏");
                        md.Commands.Add(new UICommand("取消收藏", async act =>
                        {
                            mainVM.CurrentUser.FavorsIDList.Remove(findID);
                            await UpdateFavorList();
                        }));
                        md.Commands.Add(new UICommand("按错了", act =>
                        {
                            return;
                        }));
                        md.DefaultCommandIndex = 1;
                        await md.ShowAsync();
                    }
                    else
                    {
                        mainVM.CurrentUser.FavorsIDList.Add(id);
                        await UpdateFavorList();
                    }
                });
            }
        }

        private RelayCommand _gotoCookingModeCommand;
        public RelayCommand GotoCookingModeCommand
        {
            get
            {
                if (_gotoCookingModeCommand != null) return _gotoCookingModeCommand;
                return _gotoCookingModeCommand = new RelayCommand(() =>
                  {
                      var rootFrame = Window.Current.Content as Frame;
                      rootFrame.Navigate(typeof(CookingPage),CurrentRecipe);
                      ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                  });
            }
        }
        #endregion

        public DetailedRecipeViewModel()
        {
            ShowLargePictureVisibility = Visibility.Collapsed;
            ShowLoadingVisibility = Visibility.Collapsed;

            Messenger.Default.Register<GenericMessage<BitmapImage>>(this, MessengerToken.ShowPictureToken, act =>
            {
                var bitmap = act.Content;
                if (bitmap != null)
                {
                    ShowLargePictureCommand.Execute(bitmap);
                }
            });
            Messenger.Default.Register<GenericMessage<Recipe>>(this, MessengerToken.RecipeToken, async act =>
            {
                var recipe = act.Content;
                if (recipe != null)
                {
                    this.CurrentRecipe = recipe;

                    this.CheckListGrouped = GetCheckListsGrouped(this.CurrentRecipe.IngredientList);
                }
                await LoadDetailPage();
            });
        }

        public async Task LoadDetailPage()
        {
            try
            {
                var userName = this.CurrentRecipe.CookUser.UserName;
                var user = await RequestHelper.GetUserInfoAsync(userName);
                if (user != null)
                {
                    await user.DownloadAvatar();
                    this.CurrentRecipe.CookUser = user;
                    await this.CurrentRecipe.CookUser.LoadRegionInfo();
                }

                foreach(var step in this.CurrentRecipe.StepsList)
                {
                    step.ShowImageVisibility = Visibility.Collapsed;
                    if(!string.IsNullOrEmpty(step.ImageUrl))
                    {
                        step.ShowImageVisibility = Visibility.Visible;
                        var stream = await RequestHelper.GetImageFromUrl(step.ImageUrl);
                        if(stream!=null)
                        {
                            step.InsertedImage = new BitmapImage();
                            var task=step.InsertedImage.SetSourceAsync(stream);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(e.Message), MessengerToken.ToastToken);
            }
            

        }

        private async Task UpdateFriendList()
        {
            var mainVM = (App.Current.Resources["Locator"] as ViewModelLocator).MainVM;
            var dic = new Dictionary<string, string>();
            var friendlist = Functions.MakeStringFromList(mainVM.CurrentUser.FriendsIDList);
            dic.Add("friends_list", friendlist);
            var isSuccess = await RequestHelper.UpdateUserInfo(LocalSettingHelper.GetValue("userid"), dic);
            if (isSuccess.IsSuccess)
            {
                Messenger.Default.Send(new GenericMessage<string>("操作成功"), MessengerToken.ToastTokenFollow);
            }
            else
            {
                Messenger.Default.Send(new GenericMessage<string>("操作失败"), MessengerToken.ToastTokenFollow);
            }
        }

        private async Task UpdateFavorList()
        {
            var mainVM = (App.Current.Resources["Locator"] as ViewModelLocator).MainVM;
            var dic = new Dictionary<string, string>();
            var favorList = Functions.MakeStringFromList(mainVM.CurrentUser.FavorsIDList);
            dic.Add("favors_list", favorList);
            var isSuccess = await RequestHelper.UpdateUserInfo(LocalSettingHelper.GetValue("userid"), dic);
            if (isSuccess.IsSuccess)
            {
                Messenger.Default.Send(new GenericMessage<string>("操作成功"), MessengerToken.ToastTokenFollow);
            }
            else
            {
                Messenger.Default.Send(new GenericMessage<string>("操作失败"), MessengerToken.ToastTokenFollow);
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

        private async Task GetLoacationAndShowAsync()
        {
            try
            {
                // Request permission to access location
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        ShowLoadingVisibility = Visibility.Visible;

                        // Get cancellation token
                        _cts = new CancellationTokenSource();
                        CancellationToken token = _cts.Token;

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 10 };

                        // Carry out the operation
                        Geoposition pos = await geolocator.GetGeopositionAsync().AsTask(token);

                        string uriToLaunch = @"bingmaps:?cp="+pos.Coordinate.Point.Position.Latitude+"~"+pos.Coordinate.Point.Position.Longitude+"&lvl=3&where=supermarket";

                        // Launch the URI
                        var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(uriToLaunch));

                        ShowLoadingVisibility = Visibility.Collapsed;

                        break;
                    default:
                        {
                            Messenger.Default.Send(new GenericMessage<string>("请开启地理位置权限"), MessengerToken.ToastToken);
                        };break;
                }
            }
            catch (TaskCanceledException ex)
            {
                Messenger.Default.Send(new GenericMessage<string>(ex.Message), MessengerToken.ToastTokenFollow);
            }
        }

        public void Activate(object param)
        {
            
        }

        public void Deactivate(object param)
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
                ShowLoadingVisibility = Visibility.Collapsed;
            }
        }
    }
}
