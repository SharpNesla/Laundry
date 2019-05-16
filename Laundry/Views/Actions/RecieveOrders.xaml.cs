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

namespace Laundry.Views.Actions
{
  public class RecieveOrdersViewModel : OrderActionsBase
  {
    private readonly IModel _model;

    public RecieveOrdersViewModel(IModel model, OrderDataGridViewModel orderGrid)
      : base(model.Orders, model.CurrentUser, nameof(Order.WasherCourierId), orderGrid, OrderStatus.MoveFromSubs,
        OrderStatus.ReadyToWash, "Bill.docx")
    {
      _model = model;
    }

    protected override IEnumerable<Tuple<string, string>> PrepareReplaceText(Order order)
    {
      return base.PrepareReplaceText(order)
        .Concat(new[]
        {
          new Tuple<string, string>("#ФИО_Отпускающего", _model.Employees.GetById(order.InCourierId).ToString()),
          new Tuple<string, string>("#ФИО_Принимающего", _model.CurrentUser.ToString())
        });
    }
  }
}