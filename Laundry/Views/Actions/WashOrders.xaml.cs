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
using Model;
using Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MongoDB.Driver;
using NPOI.XWPF.UserModel;

namespace Laundry.Views.Actions
{
  public class WashOrdersViewModel : OrderActionsBase
  {
    public WashOrdersViewModel(IModel model, OrderDataGridViewModel orderGrid) 
      : base(model.Orders, model.CurrentUser, nameof(Order.WasherCourierId), orderGrid, OrderStatus.ReadyToWash, OrderStatus.Washing)
    {
    }


    public override Document PrepareDocument(Document document, Order order)
    {
      throw new NotImplementedException();
    }
  }

}
