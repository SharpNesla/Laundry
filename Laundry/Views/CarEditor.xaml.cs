using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for CarEditor.xaml
  /// </summary>
  public class CarEditorViewModel : ActivityScreen
  {

    public Car Car{ get; set; }

    [AlsoNotifyFor(nameof(IsOrdersEnabled), nameof(EditorTitle))]
    public bool IsNew { get; set; }
    public bool IsOrdersEnabled
    {
      get { return !IsNew; }
    }

    public string EditorTitle
    {
      get { return !IsNew ? $"Редактирование автомобиля №{Car.Id}" : "Редактирование нового автомобиля"; }
    }

    public EmployeeDataGridViewModel Couriers { get; set; }
    public EmployeeDataGridViewModel Drivers { get; set; }

    public CarEditorViewModel(IEventAggregator aggregator, IModel model, EmployeeDataGridViewModel courierGrid, EmployeeDataGridViewModel driverGrid) : base(aggregator, model)
    {
      this.Couriers = courierGrid;
      this.Drivers = driverGrid;

      this.EventAggregator.Subscribe(this);

      this.IsNew = true;
      this.Car = new Car();
    }

    public void Cancel()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public void ApplyChanges()
    {
      if (IsNew)
      {
        Model.Cars.Add(this.Car);
      }
      else
      {
        Model.Cars.Update(this.Car);
      }

      ChangeApplicationScreen(Screens.Context);
    }
  }
}