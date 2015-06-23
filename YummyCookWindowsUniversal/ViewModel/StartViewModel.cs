using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YummyCookWindowsUniversal.Interface;
using YummyCookWindowsUniversal.Model;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class StartViewModel:ViewModelBase,INavigable
    {
        private RelayCommand _navigateToLoginCommand;
        public RelayCommand NavigateToLoginCommand
        {
            get
            {
                if (_navigateToLoginCommand != null) return _navigateToLoginCommand;
                return _navigateToLoginCommand = new RelayCommand(() =>
                  {
                      Frame rootFrame = Window.Current.Content as Frame;
                      if(rootFrame!=null)
                      {
                          NavigationData data = new NavigationData();
                          data.paramsData.Add("ToLogin", "true");
                          rootFrame.Navigate(typeof(LoginPage),data);
                      }
                  });
            }
        }

        private RelayCommand _navigateToRegisterCommand;
        public RelayCommand NavigateToRegisterCommand
        {
            get
            {
                if (_navigateToRegisterCommand != null) return _navigateToRegisterCommand;
                return _navigateToRegisterCommand = new RelayCommand(() =>
                  {
                      Frame rootFrame = Window.Current.Content as Frame;
                      if (rootFrame != null)
                      {
                          NavigationData data = new NavigationData();
                          data.paramsData.Add("ToLogin", "false");
                          rootFrame.Navigate(typeof(LoginPage), data);
                      }
                  });
            }
        }

        public void Activate(object param)
        {
            
        }

        public void Deactivate(object param)
        {
            
        }
    }
}
