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
  public class CarEditorViewModel : ActivityScreen, IHandle<Car>
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

    public void Handle(Car message)
    {
      this.Car = this.Model.Cars.GetById(message.Id);
      this.IsNew = false;

      this.EventAggregator.Unsubscribe(this);
    }
  }
}