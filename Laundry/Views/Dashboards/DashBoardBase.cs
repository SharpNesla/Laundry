using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;

namespace Laundry.Views.Dashboards
{
  public class DashBoardBase : DrawerActivityScreen
  {
    public OrderDataGridViewModel OrderGrid { get; internal set; }

    protected readonly OrderDataGridViewModel ActionsOrderGrid;

    public DashBoardBase(IEventAggregator aggregator, IModel model, OrderDataGridViewModel orderGrid,
      OrderDataGridViewModel actionsGrid)
      : base(aggregator, model)
    {
      this.OrderGrid = orderGrid;
      this.ActionsOrderGrid = actionsGrid;
      orderGrid.DisplaySelectionColumn = false;
    }
  }
}