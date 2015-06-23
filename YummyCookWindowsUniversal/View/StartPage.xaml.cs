using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YummyCookWindowsUniversal.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace YummyCookWindowsUniversal
{
    
    public sealed partial class StartPage : BindablePage
    {
        public StartPage()
        {
            this.InitializeComponent();
            StartViewModel startVM = new StartViewModel();
            this.DataContext = startVM;

            if(ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar.GetForCurrentView().BackgroundOpacity = 100;
                //StatusBar.GetForCurrentView().BackgroundColor = ((SolidColorBrush)App.Current.Resources["CookThemeDark"]).Color;
            }
            
        }
    }
}
