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
  
    public static DialogHost Current { get; set; }
    
    /// <summary>
    /// Метод, закрывающий активный DialogHost
    /// </summary>
    public static void CloseCurrent()
    {
      Current?.CurrentSession.Close();
    }

    /// <summary>
    /// Метод для показа калибюрновских ViewModel MaterialInXaml'овским DialogHost'ом
    /// </summary>
    /// <typeparam name="T">Тип VM</typeparam>
    /// <param name="viewModel">VM</param>
    public static async Task ShowCaliburnVM<T>(T viewModel)
    {
      var view = ViewLocator.LocateForModel(viewModel, null, null);
      ViewModelBinder.Bind(viewModel, view, null);

      Current?.CurrentSession?.Close();

      await Task.Delay(400);
      if (Current != null)
      {
        await Current.ShowDialog(view);
      }
    }
  }
}