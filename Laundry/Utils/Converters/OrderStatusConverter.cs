using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Laundry.Model;
namespace Laundry.Utils.Converters
{
    class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
          var val = (OrderState) value;

            switch (val)
            {
                case OrderState.Taken:
                    return "Принят";
                case OrderState.MoveFromSubs:
                    return "Перевозится в прачечную";
                case OrderState.Washing:
                    return "В стирке";
                case OrderState.MoveToSubs:
                    return "Перевозится в филиал";
                case OrderState.Granted:
                    return "Выдан";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
