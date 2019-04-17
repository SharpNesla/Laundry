using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;
using Laundry.Views;
using MaterialDesignThemes.Wpf;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  public class EmployeeDataGridViewModel : EntityGrid<Employee, EmployeeRepository, EmployeeCardViewModel>
  {
    public EmployeeDataGridViewModel(IEventAggregator eventAggregator, EmployeeCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model, Visibilities visibilities)
      : base(eventAggregator, card, model.Employees, deleteDialog, Screens.EmployeeEditor, visibilities)
    {
    }

    public override void ExportToExcel()
    {
      throw new NotImplementedException();
    }
  }
}