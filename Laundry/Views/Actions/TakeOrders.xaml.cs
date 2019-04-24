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
  public class TakeOrdersViewModel : OrderActionsBase
  {
    public TakeOrdersViewModel(IModel model, OrderDataGridViewModel orderGrid) 
      : base(model.Orders, model.CurrentUser, orderGrid, OrderStatus.Taken, OrderStatus.MoveFromSubs)
    {

    }
  }
}