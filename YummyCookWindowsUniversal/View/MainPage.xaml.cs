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

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ToastToken, act =>
                {
                    var msg = act.Content;
                    ToastControl.ShowMessage(msg);
                });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.HidePaneToken, act =>
                  {
                      root_sv_PaneClosed(null, null);
                  });
            NavigationCacheMode = NavigationCacheMode.Required;
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

            App.SetUpTitleBar();
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

        protected override void RegisterHandleBackLogic()
        {
            base.RegisterHandleBackLogic();
        }

        protected override void UnRegisterHandleBackLogic()
        {
            base.UnRegisterHandleBackLogic();
        }

        public Frame GetFrame()
        {
            if (ContentFrame.Visibility == Visibility.Visible)
            {
                App.SetUpTitleBar();
                return ContentFrame;
                
            }
            else
            {
                App.SetUpTitleBar(true);
                return Frame;
                
            }
        }
    }
}
