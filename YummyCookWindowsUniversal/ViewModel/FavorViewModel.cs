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
    public class FavorViewModel: ViewModelBase, INavigable
    {
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
                    //var rootFrame = Window.Current.Content as Frame;
                    //rootFrame.Navigate(typeof(RecipeDetailPage));

                    //Messenger.Default.Send(new GenericMessage<Recipe>(recipe), MessengerToken.RecipeToken);
                });
            }
        }

        public FavorViewModel()
        {
            FriendsList = new ObservableCollection<User>();
            RecipeList = new ObservableCollection<Recipe>();

            for(int i=0;i<10;i++)
            {
                FriendsList.Add(new User());
                RecipeList.Add(new Recipe());
            }
        }

        private async Task LoadUserList()
        {

        }

        public void Activate(object param)
        {
            if(param is User)
            {
                if(FriendsList.Count==0)
                {
                    LoadUserList();
                }
            }
        }

        public void Deactivate(object param)
        {
            
        }
    }
}
