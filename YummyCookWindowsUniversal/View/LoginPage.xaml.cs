﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YummyCookWindowsUniversal.Helper;
using YummyCookWindowsUniversal.ViewModel;


namespace YummyCookWindowsUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : BindablePage
    {
        private UserInfoViewModel UserInfoVM
        {
            get
            {
                return (DataContext as UserInfoViewModel);
            }
        }
        public LoginPage()
        {
            this.InitializeComponent();
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ToastToken, act =>
             {
                 var msg = act.Content;
                 ToastControl.ShowMessage(msg);
             });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}
