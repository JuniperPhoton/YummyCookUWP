using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YummyCookWindowsUniversal.Interface;

namespace YummyCookWindowsUniversal.ViewModel
{
    public class CookingModeViewModel : ViewModelBase, INavigable
    {

        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                if (_exitCommand != null) return _exitCommand;
                return _exitCommand = new RelayCommand(() =>
                  {
                      ApplicationView.GetForCurrentView().ExitFullScreenMode();
                      var rootFrame = Window.Current.Content as Frame;
                      if(rootFrame.CanGoBack)
                      {
                          rootFrame.GoBack();
                      }
                      
                  });
            }
        }
        public CookingModeViewModel()
        {

        }

        public void Activate(object param)
        {
            
        }

        public void Deactivate(object param)
        {
           
        }
    }
}
