using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Views;
using MaterialDesignThemes.Wpf;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>

  public class EmployeeDataGridViewModel : PropertyChangedBase
  {
    private EmployeeCardViewModel _card;
    private IEventAggregator _eventAggregator;
    public IList<Employee> Employees{ get; set; }
    public Employee SelectedEmployee { get; set; }

    public event Action<Employee> RemoveEmployeeClick;
    public event Action<Employee> EditEmployeeClick;
    public EmployeeDataGridViewModel(EmployeeCardViewModel card, IEventAggregator eventAggregator, IModel model)
    {
      this._card = card;
      this._eventAggregator = eventAggregator;
    }

    public void ShowEmployeeInfo(Employee context)
    {
      
    }

    public void EditEmployee()
    {
      this.EditEmployeeClick?.Invoke(this.SelectedEmployee);
    }

    public void RemoveEmployee()
    {
      RemoveEmployeeClick?.Invoke(SelectedEmployee);
    }
  }
}
