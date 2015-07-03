using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace YummyCookWindowsUniversal.Converter
{
    public class UnitConverter : IValueConverter
    {
        public static List<string> UnitList = new List<string> { "g", "kg", "勺", "个" };
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int index = UnitList.FindIndex(s =>
              {
                  if (s == (string)value) return true;
                  else return false;
              });
            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return UnitList.ElementAt((int)value);
        }
    }
}
