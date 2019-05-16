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
  public class CourierDashBoardViewModel : DashBoardBase
  {
    private bool _takeOrders;
    private bool _isDelivery;
    private bool _isCorp;

    public bool IsTakeOrders
    {
      get { return _takeOrders; }
      set
      {
        _takeOrders = value;
        if (value)
        {
          OrderGrid.Filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.Taken),
            Builders<Order>.Filter.Eq(nameof(Order.InCourierId), this.Model.CurrentUser.Id)
          );

          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }

    public bool IsDelivery
    {
      get { return _isDelivery; }
      set
      {
        _isDelivery = value;
        if (value)
        {
          OrderGrid.Filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.Washed),
            Builders<Order>.Filter.Eq(nameof(Order.OutCourierId), this.Model.CurrentUser.Id)
          );
          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }

    public bool IsCorp
    {
      get { return _isCorp; }
      set
      {
        _isCorp = value;
        if (value)
        {
          OrderGrid.Filter =
            Builders<Order>.Filter.And(
              Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.MoveToSubs),
              Builders<Order>.Filter.Eq(nameof(Order.OutCourierId), this.Model.CurrentUser.Id),
              Builders<Order>.Filter.Eq(nameof(Order.IsCorporative), true)
            );
          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }

    public CourierDashBoardViewModel(IEventAggregator aggregator, IModel model,
      OrderDataGridViewModel orderGrid, OrderDataGridViewModel actionsOrderGrid) : base(
      aggregator, model, orderGrid, actionsOrderGrid)
    {
    }

    protected override void OnActivate()
    {
      base.OnActivate();

      this.IsTakeOrders = true;
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public async void Delivery()
    {
      var wash = new DeliverOrdersViewModel(Model, ActionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(wash);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public async void TakeOrders()
    {
      var applyorders = new TakeOrdersViewModel(Model, ActionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(applyorders);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public async void DistributeCorp()
    {
      var applyorders = new DistributeCorpOrdersViewModel(Model, ActionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(applyorders);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }
  }
}