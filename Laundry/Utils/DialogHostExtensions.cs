using System;
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
    public static DialogHost Current { get;set; }

    /// <summary>
    /// Метод для показа калибюрновских ViewModel MaterialInXaml'овским DialogHost'ом
    /// </summary>
    /// <typeparam name="T">Тип VM</typeparam>
    /// <param name="viewModel">VM</param>
    /// 

    public static void CloseCurrent()
    {
      Current.CurrentSession.Close();
    }

    public static async Task ShowCaliburnVM<T>(T viewModel)
    {
      var view = ViewLocator.LocateForModel(viewModel, null, null);
      ViewModelBinder.Bind(viewModel, view, null);

      var current = Current;

      current.CurrentSession?.Close();

      await Task.Delay(400);

      await current.ShowDialog(view);
    }
  }
}