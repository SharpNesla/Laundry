using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.DatabaseClients;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class EmployeeEditorViewModel : EditorScreen<EmployeeRepository, Employee>
  {
    public bool IsOrdersEnabled
    {
      get { return !IsNew; }
    }

    public bool  ChangePassword { get; set; }

    public OrderDataGridViewModel OrderDataGrid { get; set; }
    public PaginatorViewModel Paginator { get; set; }

    public string Password { get; set; }
    public string AdditionalPassword { get; set; }
    #region TabBindings

    [AlsoNotifyFor(nameof(InfoVisibility))]
    public bool InfoChecked { get; set; }

    [AlsoNotifyFor(nameof(OrderGridVisibility))]
    public bool OrderChecked { get; set; }


    public Visibility InfoVisibility
    {
      get { return InfoChecked ? Visibility.Visible : Visibility.Collapsed; }
    }

    public Visibility OrderGridVisibility
    {
      get { return OrderChecked ? Visibility.Visible : Visibility.Collapsed; }
    }

    #endregion

    public EmployeeEditorViewModel(IEventAggregator aggregator, IModel model, OrderDataGridViewModel grid,
      PaginatorViewModel paginator)
      : base(aggregator, model, model.Employees, "работника")
    {
      this.InfoChecked = true;
      this.OrderDataGrid = grid;

      this.Paginator = paginator;
      paginator.ElementsName = "Заказов";
      paginator.RegisterPaginable(OrderDataGrid, false);
    }

    public override void ApplyChanges()
    {
      base.ApplyChanges();
      if (ChangePassword && (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(AdditionalPassword)) && AdditionalPassword == Password)
      {
        this.EntityRepository.SetPassword(this.Entity, Password);
      }
    }

    public void AdditionalPasswordChanged(PasswordBox box)
    {
      Password = box.Password;
    }

    public void PasswordChanged(PasswordBox box)
    {
      Password = box.Password;
    }
  }
}