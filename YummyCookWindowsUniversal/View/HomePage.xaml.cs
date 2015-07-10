using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.ViewModel;


namespace YummyCookWindowsUniversal.View
{

    public sealed partial class HomePage : BindablePage, IFrame
    {
        private MainViewModel MainVM
        {
            get
            {
                return (DataContext as MainViewModel);
            }
        }
        public HomePage()
        {
            this.InitializeComponent();

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ToastToken, act =>
            {
                var msg = act.Content;
                ToastControl.ShowMessage(msg);
            });

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.ContentFrame = this.ContentFrame;
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
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
