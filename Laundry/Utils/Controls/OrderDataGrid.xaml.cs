using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Laundry.Model;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  
  public class OrderDataGridViewModel : UserControl
  {
    public event Action<Order> OrderInfoClicked;
    public Client SpecifiedClient { get; set; }
    public IList<Order> Orders { get; set; }

    public OrderDataGridViewModel()
    {
      
    }

    public void ShowClientInfo(Order context)
    {
      OrderInfoClicked?.Invoke(context);
    }
  }
}
