using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Interface;

namespace YummyCookWindowsUniversal
{
    /// <summary>
    /// 提供一个 Page 的基类，继承这个类的 Page，可以实现：
    /// 在 ViewModel 里获得 OnNavigatedTo 的参数；
    /// 处理返回键的导航。
    /// 如果要自定义返回键的行为，可以覆写 RegisterHandleBackLogic() 和 UnRegisterHandleBackLogic() 方法
    /// NOTE: += 了返回键的事件，必须要在离开页面的时候 -= 
    /// </summary>
    public partial class BindablePage : Page
    {
        public BindablePage()
        {
            TransitionCollection collection = new TransitionCollection();
            NavigationThemeTransition theme = new NavigationThemeTransition();
            
            var info = new ContinuumNavigationTransitionInfo();
            theme.DefaultNavigationTransitionInfo = info;
            collection.Add(theme);
            this.Transitions = collection;

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var task=StatusBar.GetForCurrentView().HideAsync();
                //StatusBar.GetForCurrentView().BackgroundOpacity = 0.001;
                //StatusBar.GetForCurrentView().BackgroundColor = ((SolidColorBrush)App.Current.Resources["CookThemeDark"]).Color;
            }
            this.IsTextScaleFactorEnabled = false;
        }

        protected virtual void RegisterHandleBackLogic()
        {
            try
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += BindablePage_BackRequested;
                if (ApiInformationHelper.CheckHardwareButton())
                {
                    Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        protected virtual void UnRegisterHandleBackLogic()
        {
            try
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= BindablePage_BackRequested;
                if (ApiInformationHelper.CheckHardwareButton())
                {
                    Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (Frame != null)
            {
                if (Frame.CanGoBack)
                {
                    e.Handled = true;
                    Frame.GoBack();
                }
            }
        }

        private void BindablePage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(Frame!=null)
            {
                if (Frame.CanGoBack)
                    Frame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var NavigationViewModel = (INavigable)this.DataContext;
            if (NavigationViewModel != null)
            {
                NavigationViewModel.Activate(e.Parameter);
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            RegisterHandleBackLogic();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var NavigationViewModel = (INavigable)this.DataContext;
            if (NavigationViewModel != null)
            {
                NavigationViewModel.Deactivate(null);
            }
            UnRegisterHandleBackLogic();
        }

    }
}
