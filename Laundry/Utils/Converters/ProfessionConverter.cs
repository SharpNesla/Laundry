using System;
using System.Globalization;
using System.Windows.Data;
using Model;

namespace Laundry.Utils.Converters
{
  /// <summary>
  /// Конвертер перечисления-профессии в строку
  /// </summary>
  class ProfessionConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var val = (EmployeeProfession)value;

      return (new EnumerationExtension(typeof(EmployeeProfession))).GetDescription(val);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}