using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Laundry.Model
{
  class DataBaseModel : IModel
  {

    public Employee CurrentUser { get; }

    private readonly IMongoDatabase _dataBase;
    private IMongoCollection<Client> _clients;
    private IMongoCollection<Order> _orders;
    private IMongoCollection<Employee> _employees;

    public DataBaseModel()
    {
      string connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;

      MongoClient client = new MongoClient(connectionString);

      this._dataBase = client.GetDatabase("laundry");
      this._clients = _dataBase.GetCollection<Client>("clients");
      this._orders = _dataBase.GetCollection<Order>("orders");
      this._employees = _dataBase.GetCollection<Employee>("employees");
    }

    public Client AddClient()
    {
      return null;
    }


    public void AddClient(Client client)
    {
      try
      {
        client.Id = _clients.Aggregate().SortByDescending(x => x.Id).First().Id + 1;
      }
      catch
      {
        client.Id = 0;
      }

      _clients.InsertOne(client);
    }

    public Client GetClientById(long id)
    {
      return _clients.Find(Builders<Client>.Filter.Eq(nameof(Client.Id), id)).First();
    }

    public void UpdateClient(Client client)
    {
      _clients.ReplaceOne(x => x.Id == client.Id, client);
    }

    public void RemoveClient(Client client)
    {
      _clients.UpdateOne(Builders<Client>.Filter.Where(x => x.Id == client.Id),
        Builders<Client>.Update.Set(nameof(client.DeletionDate), DateTime.Now));
    }

    public IList<Client> GetClients(int offset, int limit)
    {
      return _clients.Find(Builders<Client>.Filter.Exists(nameof(Client.DeletionDate), false)).Skip(offset).Limit(limit).ToList();
    }

    public long GetClientsCount()
    {
      return _clients.CountDocuments(Builders<Client>.Filter.Exists(nameof(Client.DeletionDate), false));
    }

    public void AddOrder(Order order)
    {
      try
      {
        order.Id = _orders.Aggregate().SortByDescending(x => x.Id).First().Id + 1;
      }
      catch
      {
        order.Id = 0;
      }

      _orders.InsertOne(order);
    }


    public Order GetOrderById(long id)
    {
      var orderById = _orders.Find(x => x.Id == id).First();
      orderById.Client = GetClientById(orderById.ClientId);
      return orderById;
    }

    public IList<Order> GetOrdersForClient(Client client, int offset, int limit)
    {
      var filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.ClientId), client.Id)
      );
      var orders = _orders.Find(filter).Skip(offset).Limit(limit).ToList();
      return orders;
    }

    public long GetOrdersCount()
    {
      return _orders.CountDocuments(Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false));
    }

    public long GetOrdersForClientCount(Client client)
    {
      var filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.ClientId), client.Id)
      );
      return _orders.CountDocuments(filter);
    }


    public void UpdateOrder(Order order)
    {
      _orders.ReplaceOne(x => x.Id == order.Id, order);
    }

    public void RemoveOrder(Order order)
    {
      throw new NotImplementedException();
    }

    public IList<Order> GetOrders(int offset, int limit)
    {
      var orders = _orders.Find(Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false)).ToList();
      foreach (var x in orders) x.Client = GetClientById(x.ClientId);
      return orders;
    }

    public void AddEmployee(Employee employee)
    {
      try
      {
        employee.Id = _employees.Aggregate().SortByDescending(x => x.Id).First().Id + 1;
      }
      catch
      {
        employee.Id = 0;
      }

      _employees.InsertOne(employee);
    }

    public void RemoveEmployee(Employee employee)
    {
      _clients.UpdateOne(Builders<Client>.Filter.Where(x => x.Id == employee.Id),
        Builders<Client>.Update.Set(nameof(employee.DeletionDate), DateTime.Now));
    }

    public ClothKind AddClothKind()
    {
      throw new NotImplementedException();
    }

    public void RemoveClothKind(ClothKind clothKind)
    {
      throw new NotImplementedException();
    }

    public ClothInstance AddClothInstance()
    {
      throw new NotImplementedException();
    }

    public void RemoveClothInstance(ClothInstance clothInstance)
    {
      throw new NotImplementedException();
    }

    public Subsidiary AddSubsidiary()
    {
      throw new NotImplementedException();
    }

    public void RemoveSubsidiary(Subsidiary subsidiary)
    {
      throw new NotImplementedException();
    }

    public Car AddCar()
    {
      throw new NotImplementedException();
    }

    public void RemoveCar(Car car)
    {
      throw new NotImplementedException();
    }

    public IList<Employee> GetEmployees(int offset, int limit)
    {
      return _employees.Find(Builders<Employee>.Filter.Exists(nameof(Employee.DeletionDate), false)).ToList();
    }
  }
}