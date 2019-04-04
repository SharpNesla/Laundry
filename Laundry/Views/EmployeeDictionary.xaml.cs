using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.DatabaseClients;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public class EmployeeDictionaryViewModel : DictionaryScreen<Employee, EmployeeDataGridViewModel>
  {
    public EmployeeDictionaryViewModel(IEventAggregator aggregator, IModel model, PaginatorViewModel paginator, EmployeeDataGridViewModel entityGrid)
      : base(aggregator, model, paginator, entityGrid)
    {
      paginator.ElementsName = "Работников";
    }
  }
}