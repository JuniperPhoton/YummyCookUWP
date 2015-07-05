using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace YummyCookWindowsUniversal.Converter
{
    public class IsMainConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool ismain = (bool)value;
            if (ismain)
            {
                return 0;
            }
            else return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int selected = (int)value;
            if (selected == 0)
            {
                return true;
            }
            else return false;
        }
    }
}
