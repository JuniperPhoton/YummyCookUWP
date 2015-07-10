using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.Model;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class CookingModeViewModel : ViewModelBase, INavigable
    {
        private bool IS_LIGHT_MODE = true;

        private SolidColorBrush _backgrdColor;
        public SolidColorBrush BackgrdColor
        {
            get
            {
                return _backgrdColor;
            }
            set
            {
                if(_backgrdColor!=value)
                {
                    _backgrdColor = value;
                    RaisePropertyChanged(() => BackgrdColor);
                }
            }
        }

        private SolidColorBrush _foregrdColor;
        public SolidColorBrush ForegrdColor
        {
            get
            {
                return _foregrdColor;
            }
            set
            {
                if (_foregrdColor != value)
                {
                    _foregrdColor = value;
                    RaisePropertyChanged(() => ForegrdColor);
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

        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                if (_exitCommand != null) return _exitCommand;
                return _exitCommand = new RelayCommand(() =>
                  {
                      ApplicationView.GetForCurrentView().ExitFullScreenMode();
                      var rootFrame = Window.Current.Content as Frame;
                      if(rootFrame.CanGoBack)
                      {
                          rootFrame.GoBack();
                      }
                      
                  });
            }
        }

        private RelayCommand _toggleLightCommand;
        public RelayCommand ToggleLightCommand
        {
            get
            {
                if (_toggleLightCommand != null) return _toggleLightCommand;
                return _toggleLightCommand = new RelayCommand(() =>
                  {
                      if(IS_LIGHT_MODE)
                      {
                          BackgrdColor = new SolidColorBrush(Colors.Black);
                          ForegrdColor = new SolidColorBrush(Colors.White);
                          IS_LIGHT_MODE = false;
                      }
                      else
                      {
                          BackgrdColor = new SolidColorBrush(Colors.White);
                          ForegrdColor = new SolidColorBrush(Colors.Black);
                          IS_LIGHT_MODE = true;
                      }
                  });
            }
        }

        private RelayCommand<Ingredient> _checkItemCommand;
        public RelayCommand<Ingredient> CheckItemCommand
        {
            get
            {
                if (_checkItemCommand != null)
                {
                    return _checkItemCommand;
                }
                return _checkItemCommand = new RelayCommand<Ingredient>((ingredient) =>
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
                if (_checkListGrouped != value)
                {
                    _checkListGrouped = value;
                    RaisePropertyChanged(() => CheckListGrouped);
                }
            }
        }


        public CookingModeViewModel()
        {
            BackgrdColor = new SolidColorBrush(Colors.White);
            ForegrdColor = new SolidColorBrush(Colors.Black);
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
                            GroupName = g.Key ? "主料" : "辅料"
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
            if(param is Recipe)
            {
                this.CurrentRecipe = param as Recipe;
                this.CheckListGrouped = GetCheckListsGrouped(this.CurrentRecipe.IngredientList);
            }
        }

        public void Deactivate(object param)
        {
           
        }

        public override void Cleanup()
        {
            base.Cleanup();

            this.CurrentRecipe = null;
            this.CheckListGrouped = null;
        }
    }
}
