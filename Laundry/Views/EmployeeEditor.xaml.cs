using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class EmployeeEditorViewModel : DrawerActivityScreen, IHandle<Employee>
  {

    public Employee Employee{ get; set; }

    [AlsoNotifyFor(nameof(IsOrdersEnabled), nameof(EditorTitle))]
    public bool IsNew { get; set; }
    public bool IsOrdersEnabled
    {
      get { return !IsNew; }
    }

    public string EditorTitle
    {
      get { return !IsNew ? $"Редактирование работника №{Employee.Id}" : "Редактирование нового работника"; }
    }


    public OrderDataGridViewModel OrderDataGrid { get; set; }
    public PaginatorViewModel Paginator { get; set; }
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

    public EmployeeEditorViewModel(IEventAggregator aggregator, IModel model, OrderDataGridViewModel grid, PaginatorViewModel paginator) : base(aggregator, model)
    {
      this.EventAggregator.Subscribe(this);
      this.InfoChecked = true;
      this.OrderDataGrid = grid;

      this.IsNew = true;
      this.Employee = new Employee();

      this.Paginator = paginator;
      paginator.ElementsName = "Заказов";
      this.Paginator.Changed += RefreshOrders;
    }


    private void RefreshOrders(int page, int elements)
    {
        this.OrderDataGrid.Entities = Model.Orders.GetForEmployee(this.Employee, page * elements, elements);
    }

    public void ApplyChanges()
    {
      if (IsNew)
      {
        Model.Employees.Add(this.Employee);
      }
      else
      {
        Model.Employees.Update(this.Employee);
      }

      ChangeApplicationScreen(Screens.Context);
    }

    public void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public void Handle(Employee message)
    {
      this.Employee = this.Model.Employees.GetById(message.Id);
      this.IsNew = false;

      Paginator.Count = Model.Orders.GetForEmployeeCount(this.Employee);

      RefreshOrders(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
      this.EventAggregator.Unsubscribe(this);
    }
  }
}