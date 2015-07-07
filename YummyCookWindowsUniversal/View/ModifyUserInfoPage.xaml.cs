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
using YummyCookWindowsUniversal.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace YummyCookWindowsUniversal
{ 
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ModifyUserInfoPage : BindablePage
    {
        private UserInfoViewModel UserInfoVM
        {
            get
            {
                return (DataContext as UserInfoViewModel);
            }
        }
        public ModifyUserInfoPage()
        {
            this.InitializeComponent();
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ToastToken, act =>
              {
                  ToastControl.ShowMessage(act.Content);
              });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}
