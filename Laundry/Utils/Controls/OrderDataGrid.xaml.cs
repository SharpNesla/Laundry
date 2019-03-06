using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Laundry.Model;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  public partial class OrderDataGrid : UserControl
  {


    public IObservableCollection<Order> Orders
    {
      get { return (IObservableCollection<Order>)GetValue(OrdersProperty); }
      set { SetValue(OrdersProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Orders.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrdersProperty =
        DependencyProperty.Register("Orders", typeof(IObservableCollection<Order>), typeof(OrderDataGrid));



    public OrderDataGrid()
    {
      InitializeComponent();
      this.DataContext = this;
    }
  }
}
