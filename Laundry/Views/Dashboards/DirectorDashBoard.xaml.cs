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
  public class DirectorDashBoardViewModel : DashBoardBase
  {
    private readonly IEventAggregator _aggregator;
    private readonly IModel _mockModel;
    private readonly OrderDataGridViewModel _orderGrid;
    private readonly TakeOrdersViewModel _takeOrders;
    private readonly WashOrdersViewModel _wash;

    public DirectorDashBoardViewModel(IEventAggregator aggregator, IModel model,
      OrderDataGridViewModel orderGrid, OrderDataGridViewModel actionsOrderGrid) : base(
      aggregator, model, orderGrid, actionsOrderGrid)
    {
      _aggregator = aggregator;
      _mockModel = model;
      _orderGrid = orderGrid;
    }

    public void OpenOrderEditor()
    {
      this.ChangeApplicationScreen(Utils.Screens.OrderEditor);
    }

    public void MoveFromSubs()
    {
      var takeOrders = new TakeOrdersViewModel(_mockModel, _orderGrid);
      DialogHostExtensions.ShowCaliburnVM(takeOrders);
    }

    public void MoveToSubs()
    {
      var deliverOrders = new DeliverOrdersViewModel(_mockModel, _orderGrid);
      DialogHostExtensions.ShowCaliburnVM(deliverOrders);
    }

    

    

    public void Wash()
    {
      var wash = new WashOrdersViewModel(_mockModel, _orderGrid);
      DialogHostExtensions.ShowCaliburnVM(wash);
    }
  }
}