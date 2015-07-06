using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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

    public sealed partial class RecipeDetailPage : BindablePage
    {
        public DetailedRecipeViewModel DetailVM;
        public RecipeDetailPage()
        {
            this.InitializeComponent();
            DetailVM = new DetailedRecipeViewModel();
            this.DataContext = DetailVM;

            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.ShowPictureToken, act =>
              {
                  ShowPictureStory.Begin();
              });
            Messenger.Default.Register<GenericMessage<string>>(this, MessengerToken.HidePictureToken, act =>
            {
                HidePictureStory.Begin();
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}
