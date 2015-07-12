using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
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
        private bool IS_TIMER_START = false;

        private bool IS_LIGHT_MODE = true;

        private DispatcherTimer _timer;
        public DispatcherTimer Timer
        {
            get
            {
                return _timer;
            }
            set
            {
                if(_timer!=value)
                {
                    _timer = value;
                    RaisePropertyChanged(() => Timer);
                }
            }
        }

        private string _time;
        public string Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        private string _timerContent;
        public string TimerContent
        {
            get
            {
                return _timerContent;
            }
            set
            {
                if(_timerContent!=value)
                {
                    _timerContent = value;
                    RaisePropertyChanged(() => TimerContent);
                }
            }
        }

        private bool _enablePicker;
        public bool EnablePicker
        {
            get
            {
                return _enablePicker;
            }
            set
            {
                if(_enablePicker!=value)
                {
                    _enablePicker = value;
                    RaisePropertyChanged(() => EnablePicker);
                }
            }
        }

        private Visibility _showTimeVisibililty;
        public Visibility ShowTimeVisibility
        {
            get
            {
                return _showTimeVisibililty;
            }
            set
            {
                if (_showTimeVisibililty != value)
                {
                    _showTimeVisibililty = value;
                    RaisePropertyChanged(() => ShowTimeVisibility);
                }
            }
        }

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

        private ObservableCollection<int> _minutesSource;
        public ObservableCollection<int> MinutesSource
        {
            get
            {
                return _minutesSource;
            }
            set
            {
                if(_minutesSource!=value)
                {
                    _minutesSource = value;
                    RaisePropertyChanged(() => MinutesSource);
                }
            }
        }

        private ObservableCollection<int> _secondsSource;
        public ObservableCollection<int> SecondsSource
        {
            get
            {
                return _secondsSource;
            }
            set
            {
                if (_secondsSource != value)
                {
                    _secondsSource = value;
                    RaisePropertyChanged(() => SecondsSource);
                }
            }
        }

        private int _selectedMinute;
        public int SelectedMinute
        {
            get
            {
                return _selectedMinute;
            }
            set
            {
                if (_selectedMinute != value)
                {
                    _selectedMinute = value;
                    RaisePropertyChanged(() => SelectedMinute);
                }
            }
        }

        private int _selectedSecond;
        public int SelectedSecond
        {
            get
            {
                return _selectedSecond;
            }
            set
            {
                if(_selectedSecond!=value)
                {
                    _selectedSecond = value;
                    RaisePropertyChanged(() => SelectedSecond);
                }
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

        private RelayCommand _startTimerCommand;
        public RelayCommand StartTimerCommand
        {
            get
            {
                if (_startTimerCommand != null) return _startTimerCommand;
                return _startTimerCommand = new RelayCommand(() =>
                  {
                      if(!IS_TIMER_START)
                      {
                          Time = MinutesSource.ElementAtOrDefault(SelectedMinute) + ":" + SecondsSource.ElementAtOrDefault(SelectedSecond);
                          IS_TIMER_START = true;
                          Timer.Start();
                          EnablePicker = false;
                          ShowTimeVisibility = Visibility.Visible;
                          TimerContent = "停止计时器";
                      }
                      else
                      {
                          Time = "00:00";
                          IS_TIMER_START = false;
                          Timer.Stop();
                          EnablePicker = true;
                          ShowTimeVisibility = Visibility.Collapsed;
                          TimerContent = "启动计时器";
                      }
                  });
            }
        }

        public CookingModeViewModel()
        {
            BackgrdColor = new SolidColorBrush(Colors.White);
            ForegrdColor = new SolidColorBrush(Colors.Black);
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += (timert, et) =>
              {
                  var times = Time.Split(':');
                  var min = int.Parse(times[0]);
                  var sec = int.Parse(times[1]);
                  if(sec!=0)
                  {
                      sec--;
                      Time = min + ":" + sec;
                      return;
                  }
                  else
                  {
                      if(min==0)
                      {
                          Time = "00:00";
                          ShowTimeVisibility = Visibility.Collapsed;
                          EnablePicker = true;
                          TimerContent = "启动计时器";
                          return;
                      }
                      sec = 59;
                      if (min != 0)
                      {
                          min--;
                      }
                      //var newmin = min.ToString().Length != 1 ? min.ToString() : "0" + min;
                      //var newsec = sec.ToString().Length != 1 ? sec.ToString() : "0" + sec;
                      Time = min + ":" + sec;
                      return;
                  }
              };

            MinutesSource = new ObservableCollection<int>();
            SecondsSource = new ObservableCollection<int>();
            for(int i= 0;i <= 60;i++)
            {
                MinutesSource.Add(i);
                SecondsSource.Add(i);
            }
            SelectedSecond = 0;
            SelectedMinute = 0;

            Time = "00:00";
            TimerContent = "启动计时器";
            EnablePicker = true;
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
