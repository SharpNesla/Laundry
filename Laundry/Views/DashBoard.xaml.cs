using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class DashBoardViewModel : DrawerActivityScreen
  {

    public DashBoardViewModel(IEventAggregator aggregator, IModel mockModel) : base(aggregator, mockModel)
    {
      
    }

    public void OpenOrderEditor()
    {
      this.ChangeApplicationScreen(Utils.Screens.OrderEditor);
    }

  }
}