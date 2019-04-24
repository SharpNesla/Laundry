using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
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

    public DashBoardViewModel(IEventAggregator aggregator, IModel mockModel) : base(aggregator, mockModel)
    {
      _aggregator = aggregator;
      _mockModel = mockModel;
    }
    
    public void OpenOrderEditor()
    {
      this.ChangeApplicationScreen(Utils.Screens.OrderEditor);
    }

    public async void Wash()
    {
      var washOrdersDialog = new WashOrdersViewModel(_aggregator,_mockModel);
      await DialogHostExtensions.ShowCaliburnVM(washOrdersDialog);
    }
  }
}