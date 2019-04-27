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
using Laundry.Views.Dashboards;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class DashBoardViewModel : ActivityScreen
  {
    private readonly WasherDashBoardViewModel _washerDashBoard;
    public DashBoardBase EmployeeDashBoard { get; set; }
    public DashBoardViewModel(IEventAggregator aggregator, IModel model, WasherDashBoardViewModel washerDashBoard) : base(aggregator, model)
    {
      _washerDashBoard = washerDashBoard;
    }
    protected override void OnActivate()
    {
      base.OnActivate();
      switch (Model.CurrentUser.Profession)
      {
        case EmployeeProfession.Courier:
          this.EmployeeDashBoard = _washerDashBoard;
          break;
        case EmployeeProfession.Director:
          this.EmployeeDashBoard = _washerDashBoard;
          break;
        case EmployeeProfession.Washer:
          this.EmployeeDashBoard = _washerDashBoard;
          break;
        case EmployeeProfession.Advisor:
          this.EmployeeDashBoard = _washerDashBoard;
          break;
        case EmployeeProfession.Driver:
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      this.EmployeeDashBoard.RaiseActivated();
    }
  }
}