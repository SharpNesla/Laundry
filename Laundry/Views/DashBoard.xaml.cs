using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using Caliburn.Micro;
using Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using Laundry.Views.Actions;
using Laundry.Views.Dashboards;

namespace Laundry.Views
{
  /// <summary>
  /// Мультидашборд - контейнер для дашбордов сотрудников
  /// </summary>
  public class DashBoardViewModel : ActivityScreen
  {
    #region View-model'и дашбордов всех сотрудник

    private readonly WasherDashBoardViewModel _washerDashBoard;
    private readonly AdvisorDashBoardViewModel _advisorDashBoard;
    private readonly DirectorDashBoardViewModel _directorDashBoard;
    private readonly CourierDashBoardViewModel _courierDashBoard;

    #endregion

    /// <summary>
    /// Текущий дашборд, который отображается на view
    /// </summary>
    public DashBoardBase EmployeeDashBoard { get; set; }

    public DashBoardViewModel(IEventAggregator aggregator, IModel model,
      WasherDashBoardViewModel washerDashBoard, AdvisorDashBoardViewModel advisorDashBoard,
      DirectorDashBoardViewModel directorDashBoard, CourierDashBoardViewModel courierDashBoard)
      : base(aggregator, model)
    {
      _washerDashBoard = washerDashBoard;
      _advisorDashBoard = advisorDashBoard;
      _directorDashBoard = directorDashBoard;
      _courierDashBoard = courierDashBoard;
    }

    /// <summary>
    /// Отображение дашборда для текущего сотрудника при активации мультидашборда
    /// </summary>
    protected override void OnActivate()
    {
      base.OnActivate();
      switch (Model.CurrentUser.Profession)
      {
        case EmployeeProfession.Courier:
          this.EmployeeDashBoard = _courierDashBoard;
          break;
        case EmployeeProfession.Director:
          this.EmployeeDashBoard = _directorDashBoard;
          break;
        case EmployeeProfession.Washer:
          this.EmployeeDashBoard = _washerDashBoard;
          break;
        case EmployeeProfession.Advisor:
          this.EmployeeDashBoard = _advisorDashBoard;
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