using System;
using System.Globalization;
using System.Windows.Data;
using Laundry.Model;

namespace Laundry.Utils.Converters
{
  class ProfessionConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var val = (EmployeeProfession)value;

      switch (val)
      {
        case EmployeeProfession.Courier:
          return "Курьер";
        case EmployeeProfession.Director:
          return "Директор";
        case EmployeeProfession.Washer:
          return "Прачечник";
        case EmployeeProfession.Advisor:
          return "Приёмщик";
        case EmployeeProfession.Driver:
          return "Водитель";
        default:
          return "Работник";
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}