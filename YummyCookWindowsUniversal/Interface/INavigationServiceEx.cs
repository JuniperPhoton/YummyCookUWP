using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YummyCookWindowsUniversal.Util
{
    public interface INavigationServiceEx : INavigationService
    {
        bool CanGoBack { get; }

        bool RemoveBackEntry();
    }
}
