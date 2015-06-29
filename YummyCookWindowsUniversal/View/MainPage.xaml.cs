using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using JP.Utils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.Model;
using YummyCookWindowsUniversal.ViewModel;

namespace YummyCookWindowsUniversal
{

    public sealed partial class MainPage : BindablePage,IFrame
    {
        private MainViewModel MainVM
        {
            get
            {
                return (DataContext as MainViewModel);
            }
        }
        public MainPage()
        {
            this.InitializeComponent();

            Messenger.Default.Register<GenericMessage<string>>(this, "toast", act =>
            {
                var msg = act.Content;
                ToastControl.ShowMessage(msg);
            });
        }


        private void HamburgerClick(object sender,RoutedEventArgs e)
        {
            root_sv.IsPaneOpen = !root_sv.IsPaneOpen;
            MaskInStory.Begin();
            HamInStory.Begin();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            base.OnNavigatedTo(e);
            Frame.BackStack.Clear();

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = (App.Current.Resources["CookThemeDark"] as SolidColorBrush).Color;
            titleBar.ForegroundColor = Colors.White;
            titleBar.InactiveBackgroundColor = titleBar.BackgroundColor;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = (App.Current.Resources["CookThemeDark"] as SolidColorBrush).Color;
            titleBar.ButtonForegroundColor = Colors.White;
            titleBar.ButtonInactiveBackgroundColor = titleBar.BackgroundColor;
            titleBar.ButtonInactiveForegroundColor = Colors.White;
            titleBar.ButtonHoverBackgroundColor = (App.Current.Resources["CookTheme"] as SolidColorBrush).Color;
            if (ApiInformationHelper.CheckStatusBar())
            {
                //StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                //StatusBar.GetForCurrentView().BackgroundColor = ((SolidColorBrush)App.Current.Resources["CookThemeDark"]).Color;
                //StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
            }

            App.ContentFrame = this.ContentFrame;
        }

        private void root_sv_PaneClosed(SplitView sender, object args)
        {
            MaskOutStory.Begin();
            HamOutStory.Begin();
        }

        public Frame GetFrame()
        {
            if (ContentFrame.Visibility == Visibility.Visible) return ContentFrame;
            else return Frame;
        }
    }
}
