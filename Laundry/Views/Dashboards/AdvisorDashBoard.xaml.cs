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
using Laundry.Utils;
using Model;
using Laundry.Utils.Controls;
using Laundry.Views.Actions;
using MongoDB.Driver;

namespace Laundry.Views.Dashboards
{
  public class AdvisorDashBoardViewModel : DashBoardBase
  {
    private bool _isDistribute;
    private bool _isAcceptDelivery;


    public bool IsDistribute
    {
      get { return _isDistribute; }
      set
      {
        _isDistribute = value;
        if (value)
        {
          OrderGrid.Filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.ReadyToDistribute),
            Builders<Order>.Filter.Eq(nameof(Order.DistributerId), this.Model.CurrentUser.Id),
            Builders<Order>.Filter.Eq(nameof(Order.IsCorporative), false));
          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }

    public bool IsAcceptDelivery
    {
      get { return _isAcceptDelivery; }
      set
      {
        _isAcceptDelivery = value;
        if (value)
        {
          OrderGrid.Filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(nameof(Order.Status), OrderStatus.MoveToSubs),
            Builders<Order>.Filter.Eq(nameof(Order.DistributerId), this.Model.CurrentUser.Id),
            Builders<Order>.Filter.Eq(nameof(Order.IsCorporative), false)
          );
          this.OrderGrid.Refresh(0, int.MaxValue);
        }
      }
    }


    public AdvisorDashBoardViewModel(IEventAggregator aggregator, IModel model,
      OrderDataGridViewModel orderGrid, OrderDataGridViewModel actionsOrderGrid) : base(
      aggregator, model, orderGrid, actionsOrderGrid)
    {
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      IsDistribute = true;
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public async void AcceptDelivery()
    {
      var applyorders = new AcceptDeliveryViewModel(Model, ActionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(applyorders);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }

    public async void Distribute()
    {
      var applyorders = new DistributeOrdersViewModel(Model, ActionsOrderGrid);
      await DialogHostExtensions.ShowCaliburnVM(applyorders);
      this.OrderGrid.Refresh(0, int.MaxValue);
    }
  }
}