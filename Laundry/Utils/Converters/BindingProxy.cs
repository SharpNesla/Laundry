using System.Windows;

namespace Laundry.Utils.Converters
{
  /// <summary>
  /// Специальный proxy-объект, похволяющий пробросить DataContext
  /// в элементы интерфейса, не имеющие оного (ContextMenu, DataGrid и.т.п.)
  /// </summary>
  public class BindingProxy : Freezable
  {
    protected override Freezable CreateInstanceCore()
    {
      return new BindingProxy();
    }
    
    public object Data
    {
      get { return (object)GetValue(DataProperty); }
      set { SetValue(DataProperty, value); }
    }

    public static readonly DependencyProperty DataProperty =
      DependencyProperty.Register("Data", typeof(object),
        typeof(BindingProxy));
  }
}