using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using Laundry.Views.Actions;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class DashBoardViewModel : DrawerActivityScreen
  {
    private readonly IEventAggregator _aggregator;
    private readonly IModel _mockModel;
    private readonly OrderDataGridViewModel _orderGrid;
    private readonly TakeOrdersViewModel _takeOrders;
    private readonly WashOrdersViewModel _wash;

    public DashBoardViewModel(IEventAggregator aggregator, IModel mockModel, OrderDataGridViewModel orderGrid) : base(aggregator, mockModel)
    {
      _aggregator = aggregator;
      _mockModel = mockModel;
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

    public void ApplyOrdersForDelivery()
    {
      var applyorders = new ApplyOrdersForDeliveryViewModel(_mockModel, _orderGrid);
      DialogHostExtensions.ShowCaliburnVM(applyorders);
    }

    public void RecieveOrders()
    {
      var takeOrders = new TakeOrdersViewModel(_mockModel, _orderGrid);
      DialogHostExtensions.ShowCaliburnVM(takeOrders);
    }

    public void Wash()
    {
      var wash = new WashOrdersViewModel(_mockModel, _orderGrid);
      DialogHostExtensions.ShowCaliburnVM(wash);
    }
  }
}