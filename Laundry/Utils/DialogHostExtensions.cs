﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;

namespace Laundry.Utils
{
  /// <summary>
  /// Класс с методом для показа калибюрновских ViewModel MaterialInXaml'овским DialogHost'ом
  /// </summary>
  static class DialogHostExtensions
  {
    /// <summary>
    /// Метод для показа калибюрновских ViewModel MaterialInXaml'овским DialogHost'ом
    /// </summary>
    /// <typeparam name="T">Тип VM</typeparam>
    /// <param name="viewModel">VM</param>
    public static void ShowCaliburnVM<T>(T viewModel)
    {
      //Ищем View для ViewModel карточки клиента (Caliburn)
      var view = ViewLocator.LocateForModel(viewModel, null, null);
      ViewModelBinder.Bind(viewModel, view, null);

      DialogHost.Show(view);
    }
  }
}