using System;
using System.Collections.Generic;
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
using YummyCookWindowsUniversal.Interface;

namespace YummyCookWindowsUniversal
{
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
                StatusBar.GetForCurrentView().BackgroundOpacity = 0.001;
                StatusBar.GetForCurrentView().BackgroundColor = ((SolidColorBrush)App.Current.Resources["CookThemeDark"]).Color;
            }
            this.IsTextScaleFactorEnabled = false;
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var NavigationViewModel = (INavigable)this.DataContext;
            if (NavigationViewModel != null)
            {
                NavigationViewModel.Deactivate(null);
            }
        }

    }
}
