using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laundry.Model
{
  public class MockModel : IModel
  {
    
    public Employee CurrentUser { get; private set; }
    public ReadOnlyObservableCollection<Employee> Employees { get; private set; }

    public ReadOnlyObservableCollection<Client> Clients => new ReadOnlyObservableCollection<Client>(_clients);
    public ReadOnlyObservableCollection<Order> Orders => new ReadOnlyObservableCollection<Order>(_orders);

    public ReadOnlyObservableCollection<ClothKind> ClothKinds { get; private set; }
    public ReadOnlyObservableCollection<Car> Cars { get; private set; }
    public ReadOnlyObservableCollection<Subsidiary> Subsidiaries { get; private set; }

    private readonly ObservableCollection<Client> _clients;
    private readonly ObservableCollection<Order> _orders;

    public MockModel()
    {
      this.CurrentUser = new Employee("F", "f", "D"){Profession = EmployeeProfession.Courier};

      
      var kind = new ClothKind { MeasureKind = MeasureKind.Kg, Name = "Носки" };

      this._orders = new ObservableCollection<Order>(
        new[]
        {
          new Order{ClothInstances = new ObservableCollection<ClothInstance>(
              new[]
              {
                new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
                new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
                new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
                new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
                new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
              }
            )
          }
        });
      this._clients = new ObservableCollection<Client>(
        new[]
        {
          new Client("Антрипотийединиколей", "Карлов", "Иванович"),
          new Client("Андрей", "Rjrjh", "Иванович") {Orders = _orders, PhoneNumber="+79052848432",
            Comment ="gjiwhgiwohgigwgirworhgwgoiwrhgwgowig\nrwjhighiwgiwr\nwjighwigio"},
          new Client("Андрей", "Карлов", "Иванович"),
          new Client("Андрей", "Карлов", "Иванович"),
          new Client("Андрей", "Карлов", "Иванович"),
        }
      );
    }
  }
}