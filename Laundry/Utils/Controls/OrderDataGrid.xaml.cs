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
