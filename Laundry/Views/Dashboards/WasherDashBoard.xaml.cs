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
using MongoDB.Driver;

namespace Laundry.Views.Dashboards
{
  public class WasherDashBoardViewModel : DashBoardBase
  {
    private readonly OrderDataGridViewModel _actionsOrderGrid;
    private bool _isWashOrders;
    private bool _isApplyOrdersForDelivery;
    private bool _isRecieveOrders;
    public OrderDataGridViewModel OrderGrid { get; internal set; }
    

    public bool IsWashOrders
    {
      get { return _isWashOrders; }
      set
      {
        _isWashOrders = value;
        if (value)
        {
          OrderGrid.Filter =
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.ReadyToWash);
          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }

    public bool IsApplyOrdersForDelivery
    {
      get { return _isApplyOrdersForDelivery; }
      set
      {
        _isApplyOrdersForDelivery = value;
        if (value)
        {
          OrderGrid.Filter =
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.Washing);
          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }

    public bool IsRecieveOrders
    {
      get { return _isRecieveOrders; }
      set
      {
        _isRecieveOrders = value;
        if (value)
        {
          OrderGrid.Filter =
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.MoveToSubs);
          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }

    public WasherDashBoardViewModel(IEventAggregator aggregator, IModel model,
      OrderDataGridViewModel orderGrid, OrderDataGridViewModel actionsOrderGrid) : base(
      aggregator, model)
    {
      _actionsOrderGrid = actionsOrderGrid;

      this.OrderGrid = orderGrid;
      orderGrid.DisplaySelectionColumn = false;
      IsWashOrders = true;
    }

    public async void WashOrders()
    {
      var wash = new WashOrdersViewModel(Model, _actionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(wash);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public void ApplyOrdersForDelivery()
    {
      var applyorders = new ApplyOrdersForDeliveryViewModel(Model, _actionsOrderGrid);
      DialogHostExtensions.ShowCaliburnVM(applyorders);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public void RecieveOrders()
    {
      var takeOrders = new TakeOrdersViewModel(Model, _actionsOrderGrid);
      DialogHostExtensions.ShowCaliburnVM(takeOrders);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    
  }
}