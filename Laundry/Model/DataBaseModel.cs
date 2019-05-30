using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model.CollectionRepositories;
using Model.DatabaseClients;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Model
{
  class DataBaseModel : IModel
  {
    public Employee CurrentUser { get; set; }
    public ClientRepository Clients { get; set; }
    public EmployeeRepository Employees { get; set; }
    public OrderRepository Orders { get; set; }
    public SubsidiaryRepository Subsidiaries { get; set; }
    public CarRepository Cars { get; set; }
    public ClothKindRepository ClothKinds { get; set; }
    public DiscountSystemRepository DiscountEdges { get; set; }

    private IMongoDatabase _dataBase;
    private IMongoCollection<Client> _clients;
    private IMongoCollection<Order> _orders;
    private IMongoCollection<Employee> _employees;

    public void Connect(string username, string password)
    {
      string connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;


      var fromConnectionString = MongoClientSettings.FromConnectionString(connectionString);
      fromConnectionString.ConnectTimeout = TimeSpan.FromSeconds(1);

      MongoClient client = new MongoClient(
        fromConnectionString
      );

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
      this.ClothKinds = new ClothKindRepository(this, _dataBase.GetCollection<ClothKind>("clothkinds"));
      this.DiscountEdges = new DiscountSystemRepository(this, _dataBase.GetCollection<DiscountEdge>("discountsystem"));

      this.CurrentUser = this.Employees.GetCurrentEmployee(username, password);

      Connected?.Invoke(CurrentUser);
    }

    public event Action ConnectionLost;
    public event Action<Employee> Connected;
  }
}