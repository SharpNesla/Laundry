﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Laundry.Utils.Converters
{
  
  public class BoolRowDetailsVizConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var val = (bool)value;

      return val ? DataGridRowDetailsVisibilityMode.VisibleWhenSelected: DataGridRowDetailsVisibilityMode.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  /// <summary>
  /// Конвертер для преобразование булевых значений в перечисление Visibility,
  /// Часто используется для отсекания областей видимости пользователей на формах
  /// Соответствие true - Visible, false - Collapsed
  /// </summary>
  class BoolVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var val = (bool)value;

      return val ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }

  /// <summary>
  /// Конвертер, аналогичный предыдущему, но инвертирующий булево значение
  /// </summary>
  class BoolNotVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var val = (bool)value;

      return !val ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
