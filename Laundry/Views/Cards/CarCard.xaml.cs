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
using Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MongoDB.Driver;

namespace Laundry.Views.Cards
{
  /// <summary>
  /// Interaction logic for CarCard.xaml
  /// </summary>
  public class CarCardViewModel : Card<Car>
  {
    public override Car Entity
    {
      get { return base.Entity; }

      set
      {
        base.Entity = value;
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

        this.Drivers.Refresh(0, int.MaxValue);
      }
    }

    public CarCardViewModel(IEventAggregator eventAggregator, EmployeeDataGridViewModel driversGrid,
      Visibilities visibilities) : base(eventAggregator, Screens.CarEditor, visibilities)
    {
      this.Drivers = driversGrid;

      
      this.Drivers.DisplaySelectionColumn = false;
      this.Drivers.IsCompact = true;

    }

    public EmployeeDataGridViewModel Drivers { get; set; }
  }
}