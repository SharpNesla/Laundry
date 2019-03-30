using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Laundry.Model
{
  class DataBaseModel : IModel
  {

    public Employee CurrentUser { get; }
    public CollectionActions<Client> Clients { get; set; }
    public CollectionActions<Employee> Employees { get; set; }
    public OrderCollectionActions Orders { get; set; }

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

      this.Clients = new CollectionActions<Client>(_clients);
      this.Employees = new CollectionActions<Employee>(_employees);
      this.Orders = new OrderCollectionActions(_orders, Clients);
    }
  }
}