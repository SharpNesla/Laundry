using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class EmployeeDictionaryViewModel : DrawerActivityScreen
  {
    public IList<Employee> Employees { get; set; }

    public EmployeeDictionaryViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
      this.Employees = Model.GetEmployees(0, 0);
    }

    public void AddEmployee()
    {
      ChangeApplicationScreen(Screens.EmployeeEditor);
    }

    
  }
}