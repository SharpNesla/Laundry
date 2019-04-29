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
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils.Controls;

namespace Laundry.Views.Actions
{
  public class ApplyOrdersForDeliveryViewModel : OrderActionsBase
  {
    public ApplyOrdersForDeliveryViewModel(IModel model, OrderDataGridViewModel orderGrid) 
      : base(model.Orders, model.CurrentUser, nameof(Order.WasherCourierId), orderGrid, OrderStatus.Washing , OrderStatus.Washed)
    {
    }

    public override void PrintReport()
    {
      
    }
  }
}
