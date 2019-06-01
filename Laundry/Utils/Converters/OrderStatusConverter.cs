using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Model;

namespace Laundry.Utils.Converters
{
  /// <summary>
  /// Конвертер статуса заказа в строку
  /// </summary>
  class OrderStatusConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var val = (OrderStatus) value;

      return (new EnumerationExtension(typeof(OrderStatus))).GetDescription(val);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}