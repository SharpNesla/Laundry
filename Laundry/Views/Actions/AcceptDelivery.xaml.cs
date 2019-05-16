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
using Model.CollectionRepositories;

namespace Laundry.Views.Actions
{
  public class AcceptDeliveryViewModel : OrderActionsBase
  {
    public AcceptDeliveryViewModel(IModel model, OrderDataGridViewModel orderGrid)
      : base(model.Orders, model.CurrentUser, nameof(Order.InCourierId), orderGrid, OrderStatus.MoveFromSubs,
        OrderStatus.ReadyToWash)
    {
    }
  }
}