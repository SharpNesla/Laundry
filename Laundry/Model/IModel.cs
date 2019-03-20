using System.Collections.ObjectModel;

namespace Laundry.Model
{
  public interface IModel
  {
    Employee CurrentUser { get;}
    ReadOnlyObservableCollection<Employee> Employees { get; }
    ReadOnlyObservableCollection<Client> Clients { get; }
    ReadOnlyObservableCollection<Order> Orders { get; }
    ReadOnlyObservableCollection<ClothKind> ClothKinds { get; }
    ReadOnlyObservableCollection<Car> Cars { get; }
    ReadOnlyObservableCollection<Subsidiary> Subsidiaries { get; }

    Client AddClient();
    void RemoveClient(Client client);

    Order AddOrder();
    void RemoveOrder(Order order);

    Employee AddEmployee();
    void RemoveEmployee(Employee employee);

    ClothKind AddClothKind();
    void RemoveClothKind(ClothKind clothKind);

    ClothInstance AddClothInstance();
    void RemoveClothInstance(ClothInstance clothInstance);

    Subsidiary AddSubsidiary();
    void RemoveSubsidiary(Subsidiary subsidiary);

    Car AddCar();
    void RemoveCar(Car car);

  }
}