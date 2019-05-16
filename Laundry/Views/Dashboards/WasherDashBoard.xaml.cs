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
using Laundry.Utils;
using Laundry.Utils.Controls;
using Laundry.Views.Actions;
using MongoDB.Driver;

namespace Laundry.Views.Dashboards
{
  public class WasherDashBoardViewModel : DashBoardBase
  {
    private bool _isWashOrders;
    private bool _isApplyOrdersForDelivery;
    private bool _isRecieveOrders;


    public bool IsWashOrders
    {
      get { return _isWashOrders; }
      set
      {
        _isWashOrders = value;
        if (value)
        {
          OrderGrid.Filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.ReadyToWash),
            Builders<Order>.Filter.Eq(nameof(Order.WasherCourierId), this.Model.CurrentUser.Id)
          );
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
          OrderGrid.Filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.Washing),
            Builders<Order>.Filter.Eq(nameof(Order.WasherCourierId), this.Model.CurrentUser.Id)
          );
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
          OrderGrid.Filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.MoveFromSubs),
            Builders<Order>.Filter.Eq(nameof(Order.WasherCourierId), this.Model.CurrentUser.Id)
          );
          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }

    public WasherDashBoardViewModel(IEventAggregator aggregator, IModel model,
      OrderDataGridViewModel orderGrid, OrderDataGridViewModel actionsOrderGrid) : base(
      aggregator, model, orderGrid, actionsOrderGrid)
    {
      IsWashOrders = true;
    }

    public async void WashOrders()
    {
      var wash = new WashOrdersViewModel(Model, ActionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(wash);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public async void ApplyOrdersForDelivery()
    {
      var applyorders = new ApplyOrdersForDeliveryViewModel(Model, ActionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(applyorders);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public async void RecieveOrders()
    {
      var recieveOrders = new RecieveOrdersViewModel(Model, ActionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(recieveOrders);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      this.OrderGrid.Refresh(0, int.MaxValue);
    }
  }
}