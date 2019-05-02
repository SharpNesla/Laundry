using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;
using Laundry.Views;
using LiveCharts;
using MaterialDesignThemes.Wpf;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  public class EmployeeDataGridViewModel : EntityGrid<Employee, EmployeeRepository, EmployeeCardViewModel>, IChartable<Employee>
  {
    public EmployeeDataGridViewModel(IEventAggregator eventAggregator, EmployeeCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model, Visibilities visibilities)
      : base(eventAggregator, card, model.Employees, deleteDialog, Screens.EmployeeEditor, visibilities)
    {
    }


    protected override XSSFWorkbook PrepareWorkBook(XSSFWorkbook workbook)
    {
      throw new NotImplementedException();
    }

    public SeriesCollection Values { get; }
    public string[] Labels { get; }
    public string LabelsTitle { get; }
    public string ValuesTitle { get; }
  }
}