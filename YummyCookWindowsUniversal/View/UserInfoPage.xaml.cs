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
using YummyCookWindowsUniversal.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace YummyCookWindowsUniversal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserInfoPage : BindablePage
    {
        public UserDetailViewModel UserVM
        {
            get
            {
                return DataContext as UserDetailViewModel;
            }
        }

        private List<Grid> itemGridList = new List<Grid>();

        public UserInfoPage()
        {
            this.InitializeComponent();
            UserDetailViewModel userVM = new UserDetailViewModel();
            this.DataContext = userVM;

            this.SizeChanged += UserInfoPage_SizeChanged;
        }

        private void UserInfoPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double newWindowWidth = e.NewSize.Width-50;
            
            foreach(var grid in itemGridList)
            {
                if (newWindowWidth >= 1000)
                {
                    grid.Width = newWindowWidth / 4;
                }
                else if (newWindowWidth >= 700 && newWindowWidth < 1000)
                {
                    grid.Width = newWindowWidth / 3;
                }
                else if(newWindowWidth>=600 && newWindowWidth<700)
                {
                    grid.Width = newWindowWidth / 2;
                }
                else
                {
                    grid.Width = newWindowWidth+30;
                }
            }
        }

        private void RecipeRootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            itemGridList.Add(sender as Grid);
        }
    }
}
