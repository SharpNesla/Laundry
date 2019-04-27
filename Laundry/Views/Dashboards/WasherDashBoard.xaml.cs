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
using Laundry.Utils;
using Laundry.Utils.Controls;
using Laundry.Views.Actions;

namespace Laundry.Views.Dashboards
{
  public class WasherDashBoardViewModel : DashBoardBase
  {
    private readonly OrderDataGridViewModel _actionsOrderGrid;
    public OrderDataGridViewModel OrderGrid { get; internal set; }

    public PaginatorViewModel Paginator { get; internal set; }

    public bool IsWashOrders { get; set; }
    public bool IsApplyOrdersForDelivery { get; set; }
    public bool IsAcceptDelivery { get; set; }

    public WasherDashBoardViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator,
      OrderDataGridViewModel orderGrid, OrderDataGridViewModel actionsOrderGrid) : base(
      aggregator, model)
    {
      _actionsOrderGrid = actionsOrderGrid;
      IsWashOrders = true;

      this.Paginator = paginator;
      this.Paginator.RegisterPaginable(orderGrid);
      this.Paginator.ElementsName = "Заказов";

      this.OrderGrid = orderGrid;
    }

    public async void WashOrders()
    {
      var wash = new WashOrdersViewModel(Model, _actionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(wash);
    }
  }
}