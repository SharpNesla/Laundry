using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Laundry.Model
{
  public interface IModel
  {
    Employee CurrentUser { get;}

    void AddClient(Client client);
    Client GetClientById(long id);
    void UpdateClient(Client client);
    void RemoveClient(Client client);
    IList<Client> GetClients(int offset, int limit);

    void AddOrder(Order order);
    Order GetOrderById(long id);
    IList<Order> GetOrdersForClient(Client client, int offset, int limit);
    void UpdateOrder(Order order);
    void RemoveOrder(Order order);
    IList<Order> GetOrders(int offset, int limit);
    
    IList<Employee> GetEmployees(int offset, int limit);
    void AddEmployee(Employee employee);
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