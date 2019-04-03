using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class EmployeeDictionaryViewModel : DrawerActivityScreen
  {
    public PaginatorViewModel Paginator { get; set; }
    public EmployeeDataGridViewModel EmployeeGrid { get; set; }
    public EmployeeDictionaryViewModel(IEventAggregator aggregator,PaginatorViewModel paginator, IModel model, EmployeeDataGridViewModel employeeGrid) : base(aggregator, model)
    {
      this.Paginator = paginator;
      this.Paginator.ElementsName = "Работников";

      this.EmployeeGrid = employeeGrid;

      this.EmployeeGrid.Employees = Model.Employees.Get(0, 20);
      this.EmployeeGrid.EditEmployeeClick += EditEmployee;
      this.EmployeeGrid.RemoveEmployeeClick += RemoveEmployee;
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      Paginator.Count = Model.Employees.GetCount();
      RefreshEmployees(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public void AddEmployee()
    {
      ChangeApplicationScreen(Screens.EmployeeEditor);
    }

    private void RefreshEmployees(int page, int elements)
    {
      this.EmployeeGrid.Employees = Model.Employees.Get(page * elements, elements);
    }

    public void RemoveEmployee(Employee selected)
    {
      Model.Employees.Remove(selected);
      this.Paginator.Count = Model.Employees.GetCount();
      RefreshEmployees(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public void EditEmployee(Employee selected)
    {
      ChangeApplicationScreen(Screens.EmployeeEditor);
      EventAggregator.PublishOnUIThread(selected);
    }
  }
}