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
    public IList<Employee> Employees { get; set; }

    public PaginatorViewModel Paginator { get; set; }

    public EmployeeDictionaryViewModel(IEventAggregator aggregator,PaginatorViewModel paginator, IModel model) : base(aggregator, model)
    {
      this.Paginator = paginator;
      this.Paginator.ElementsName = "Работников";

      this.Employees = Model.Employees.Get(0, 20);
    }

    public void AddEmployee()
    {
      ChangeApplicationScreen(Screens.EmployeeEditor);
    }
  }
}