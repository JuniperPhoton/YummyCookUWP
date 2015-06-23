using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YummyCookWindowsUniversal.Interface;

namespace YummyCookWindowsUniversal
{
    public partial class BindablePage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var NavigationViewModel = (INavigable)this.DataContext;
            if (NavigationViewModel != null)
            {
                NavigationViewModel.Activate(e.Parameter);
            }
            // 在导航到每一页的时候，显示或隐藏后退按钮
            if (Frame.CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var NavigationViewModel = (INavigable)this.DataContext;
            if (NavigationViewModel != null)
            {
                //NavigationViewModel.Deactivate(e.Parameter);
            }

        }

    }
}
