using GalaSoft.MvvmLight.Messaging;
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
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.ViewModel;

namespace YummyCookWindowsUniversal.View
{

    public sealed partial class FavorPage : BindablePage
    {
        public FavorViewModel FavorVM
        {
            get
            {
                return DataContext as FavorViewModel;
            }
        }
        public FavorPage()
        {
            this.InitializeComponent();
            this.DataContext = new FavorViewModel();

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ToastToken, act =>
            {
                var msg = act.Content;
                ToastControl.ShowMessage(msg);
            });
        }

    }
}
