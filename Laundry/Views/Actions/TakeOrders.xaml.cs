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
using Model;
using Model.CollectionRepositories;
using Laundry.Utils.Controls;
using Laundry.Utils.Controls.EntitySearchControls;
using MongoDB.Driver;
using NPOI.XWPF.UserModel;

namespace Laundry.Views.Actions
{
  public class TakeOrdersViewModel : OrderActionsBase
  {
    private readonly IModel _model;

    public TakeOrdersViewModel(IModel model, OrderDataGridViewModel orderGrid)
      : base(model.Orders, model, nameof(Order.InCourierId), orderGrid, OrderStatus.Taken,
        OrderStatus.MoveFromSubs, "Bill.docx")
    {
      _model = model;
    }

    protected override IEnumerable<Tuple<string, string>> PrepareReplaceText(Order order)
    {
      return base.PrepareReplaceText(order)
        .Concat(new []
        {
          !order.IsCorporative ? new Tuple<string,string>("#ФИО_Отпускающего", _model.Employees.GetById(order.ObtainerId).ToString())
          : new Tuple<string,string>("#ФИО_Отпускающего", _model.Clients.GetById(order.CorpObtainerId).ToString()),
          new Tuple<string,string>("#ФИО_Принимающего", _model.CurrentUser.ToString())
        });
    }
  }
}