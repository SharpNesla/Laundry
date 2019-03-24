using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laundry.Model
{
  class DataBaseModel : IModel
  {
    public Employee CurrentUser { get; }
    public ReadOnlyObservableCollection<Employee> Employees { get; }
    public ReadOnlyObservableCollection<Client> Clients { get; }
    public ReadOnlyObservableCollection<Order> Orders { get; }
    public ReadOnlyObservableCollection<ClothKind> ClothKinds { get; }
    public ReadOnlyObservableCollection<Car> Cars { get; }
    public ReadOnlyObservableCollection<Subsidiary> Subsidiaries { get; }
    
    private readonly IMongoDatabase _dataBase;
    private IMongoCollection<Client> _clients;
    private IMongoCollection<Order> _orders;

    public DataBaseModel()
    {
      string connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
      
      MongoClient client = new MongoClient(connectionString);
      this._dataBase = client.GetDatabase("laundry");
      this._clients =  _dataBase.GetCollection<Client>("clients");
      this._orders = _dataBase.GetCollection<Order>("Orders");
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

    public Client GetClientById(int id)
    {
      return _clients.Find(x => x.Id == id).First();
    }

    public void UpdateClient(Client client)
    {
      _clients.ReplaceOne(x => x.Id == client.Id, client);
    }

    public void RemoveClient(Client client)
    {
      _clients.UpdateOne(Builders<Client>.Filter.Where(x=>x.Id == client.Id), Builders<Client>.Update.Set(nameof(client.DeletionDate), DateTime.Now));
    }

    public IList<Client> GetClients(int offset, int limit)
    {
      return _clients.Find(Builders<Client>.Filter.Exists(nameof(Client.DeletionDate), false)).ToList();
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


    public Order GetOrderById(int id)
    {
      throw new NotImplementedException();
    }

    public void UpdateOrder(Order order)
    {
      throw new NotImplementedException();
    }

    public void RemoveOrder(Order order)
    {
      throw new NotImplementedException();
    }

    public IList<Order> GetOrders(int offset, int limit)
    {
      return _orders.Find(Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false)).ToList();
    }

    public Employee AddEmployee()
    {
      throw new NotImplementedException();
    }

    public void RemoveEmployee(Employee employee)
    {
      throw new NotImplementedException();
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
  }
}
