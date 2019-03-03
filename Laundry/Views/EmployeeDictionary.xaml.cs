using System;
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
  public partial class EmployeeDictionaryViewModel : DrawerActivityScreen
  {
    public ObservableCollection<Employee> Employees { get; set; }

    public EmployeeDictionaryViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
      Employees = new ObservableCollection<Employee>(
        new[]
        {
          new Employee("Антрипотийединиколей", "Карлов", "Иванович") {Profession = EmployeeProfession.Courier},
          new Employee("Андрей", "Rjrjh", "Иванович"){Profession = EmployeeProfession.Courier},
          new Employee("Андрей", "Карлов", "Иванович"){Profession = EmployeeProfession.Courier},
          new Employee("Андрей", "Карлов", "Иванович"){Profession = EmployeeProfession.Courier},
          new Employee("Андрей", "Карлов", "Иванович"){Profession = EmployeeProfession.Courier},
        }
      );
    }

    private void OnEmployeeAddButtonClick(object sender, RoutedEventArgs e)
    {
      //App.CurrentWindow.ChangeView(new EmployeeEditor(this));
    }

    
  }
}