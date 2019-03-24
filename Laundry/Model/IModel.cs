using System.Collections.Generic;
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

    void AddClient(Client client);
    Client GetClientById(int id);
    void UpdateClient(Client client);
    void RemoveClient(Client client);
    IList<Client> GetClients(int offset, int limit);

    void AddOrder(Order order);
    Order GetOrderById(int id);
    void UpdateOrder(Order order);
    void RemoveOrder(Order order);
    IList<Order> GetOrders(int offset, int limit);

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