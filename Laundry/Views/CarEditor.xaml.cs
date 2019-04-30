using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MongoDB.Driver;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for CarEditor.xaml
  /// </summary>
  public class CarEditorViewModel : EditorScreen<Repository<Car>, Car>
  {
    public EmployeeDataGridViewModel Couriers { get; set; }
    public EmployeeDataGridViewModel Drivers { get; set; }

    public CarEditorViewModel(IEventAggregator aggregator, IModel model,
      EmployeeDataGridViewModel couriersGrid, EmployeeDataGridViewModel driversGrid) : base(aggregator, model,
      model.Cars, "автомобиля")
    {
      this.Couriers = couriersGrid;

      Couriers.Filter = Builders<Employee>.Filter.And(
        Builders<Employee>.Filter.Eq(nameof(Employee.Car), this.Entity.Id),
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Courier)
      );
      this.Couriers.DisplaySelectionColumn = false;
      this.Couriers.IsCompact = true;
      this.Couriers.Refresh(0, int.MaxValue);

      this.Drivers = driversGrid;

      Drivers.Filter = Builders<Employee>.Filter.And(
        Builders<Employee>.Filter.Eq(nameof(Employee.Car), this.Entity.Id),
        Builders<Employee>.Filter.Or(
          Builders<Employee>.Filter.And(
            Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Courier),
            Builders<Employee>.Filter.Eq(nameof(Employee.IsCourierCarDriver), true)
          ),
          Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Driver)
        )
      );
      this.Drivers.DisplaySelectionColumn = false;
      this.Drivers.IsCompact = true;

      this.Drivers.Refresh(0, int.MaxValue);
    }
  }
}