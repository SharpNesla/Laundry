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
using Laundry.Views;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  
  public partial class OrderDataGrid : UserControl
  {
    private readonly OrderCard _orderCard;


    public IObservableCollection<Order> Orders
    {
      get { return (IObservableCollection<Order>)GetValue(OrdersProperty); }
      set
      {

        this.DataContext = this;
        SetValue(OrdersProperty, value);
      }
    }

    // Using a DependencyProperty as the backing store for Orders.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrdersProperty =
        DependencyProperty.Register("Orders", typeof(IObservableCollection<Order>), typeof(OrderDataGrid));

    public void ShowOrderInfo(Order context)
    {
      //_orderCard.Order = context;
      //await DialogHost.Show(_orderCard);
    }

    public OrderDataGrid()
    {
      InitializeComponent();
    }
  }
}
