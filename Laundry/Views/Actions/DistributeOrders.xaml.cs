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
using Laundry.Utils.Controls;
using Model;
using MongoDB.Driver;

namespace Laundry.Views.Actions
{
  /// <summary>
  /// Interaction logic for DistributeOrders.xaml
  /// </summary>
  public class DistributeOrdersViewModel : OrderActionsBase
  {
    public DistributeOrdersViewModel(IModel model, OrderDataGridViewModel orderGrid)
      : base(model.Orders, model, nameof(Order.DistributerId), orderGrid, OrderStatus.ReadyToDistribute,
        OrderStatus.Distributed, "Check.docx", Builders<Order>.Filter.Eq(nameof(Order.IsCorporative), false))
    {
    }
  }
}