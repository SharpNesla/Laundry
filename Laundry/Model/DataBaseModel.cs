using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Laundry.Model
{
  class DataBaseModel : IModel
  {
    public Employee CurrentUser { get; set; }
    public ClientRepository Clients { get; set; }
    public EmployeeRepository Employees { get; set; }
    public OrderRepository Orders { get; set; }
    public ClothInstancesRepository ClothInstances { get; set; }
    public SubsidiaryRepository Subsidiaries { get; set; }
    public CarRepository Cars { get; set; }
    public ClothKindRepository ClothKinds { get; set; }

    private IMongoDatabase _dataBase;
    private IMongoCollection<Client> _clients;
    private IMongoCollection<Order> _orders;
    private IMongoCollection<Employee> _employees;

    public void Connect(string username, string password)
    {
      string connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;

      MongoClient client = new MongoClient(connectionString);

      this._dataBase = client.GetDatabase("laundry");
      this._clients = _dataBase.GetCollection<Client>("clients");
      this._orders = _dataBase.GetCollection<Order>("orders");
      this._employees = _dataBase.GetCollection<Employee>("employees");

      this.Clients = new ClientRepository(this, _clients);
      this.Clients.ConnectionLost += ConnectionLost;

      this.Employees = new EmployeeRepository(this, _employees);
      this.Orders = new OrderRepository(this, _orders);
      this.Subsidiaries = new SubsidiaryRepository(this, _dataBase.GetCollection<Subsidiary>("subsidiaries"));
      this.Cars = new CarRepository(this, _dataBase.GetCollection<Car>("cars"));
      this.ClothInstances = new ClothInstancesRepository(this, _dataBase.GetCollection<ClothInstance>("clothinstances"));
      this.ClothKinds = new ClothKindRepository(this, _dataBase.GetCollection<ClothKind>("clothkinds"));
      this.CurrentUser = this.Employees.GetCurrentEmployee(username, password);

      Connected?.Invoke(CurrentUser);
    }

    public event Action ConnectionLost;
    public event Action<Employee> Connected;
  }
}