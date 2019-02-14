using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Laundry.Model;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public partial class EmployeeDictionary : UserControl
  {
    public ObservableCollection<Employee> Employees { get; set; }

    public EmployeeDictionary()
    {
      InitializeComponent();

      this.DataContext = this;

      MenuToggleButton.Checked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = true;

      MenuToggleButton.Unchecked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = false;


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
      App.CurrentWindow.ChangeView(new EmployeeEditor(this));
    }
  }
}