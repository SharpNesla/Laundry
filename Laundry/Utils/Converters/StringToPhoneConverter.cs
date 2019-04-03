using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Laundry.Utils.Converters
{
  public class StringToPhoneConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return string.Empty;

      //retrieve only numbers in case we are dealing with already formatted phone no
      string phoneNo = value.ToString().Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);

      switch (phoneNo.Length)
      {
        case 7:
          return Regex.Replace(phoneNo, @"(\d{3})(\d{4})", "$1-$2");
        case 10:
          return Regex.Replace(phoneNo, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
        case 11:
          return Regex.Replace(phoneNo, @"(\d{1})(\d{3})(\d{3})(\d{4})", "$1-$2-$3-$4");
        default:
          return phoneNo;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value;
    }
  }
}
