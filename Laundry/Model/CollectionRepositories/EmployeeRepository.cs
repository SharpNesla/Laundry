using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Model.CollectionRepositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Model.DatabaseClients
{
  public class EmployeeRepository : Repository<Employee>
  {
    private const string ProjectDefinition = @"{
  Name:""$Name"",
  Surname:""$Surname"",
  Patronymic:""$Patronymic"",
  PhoneNumber:""$PhoneNumber"",
  DateBirth:""$DateBirth"",
  Gender:""$Gender"",
  House:""$House"",
  City:""$City"",
  Flat: ""$Flat"",
  ZipCode:""$ZipCode"",
  Comment:""$Comment"",
  IsCorporative:""$IsCorporative"",
  Profession: ""$Profession"",
  IsCourierCarDriver:""$IsCourierCarDriver"",
  Subsidiary:""$Subsidiary"",
  Car:""$Car"",
  Username: ""$Username"",
  PassportSerial:""$PassportSerial"",
  PassportDistributor : ""$PassportDistributor"",
  OrdersCount :{
    $add:[
      {$size : ""$Orders""}
      ]
  },

  OrdersPrice : {$sum:'$Orders.Price'}
}";

    protected override IAggregateFluent<Employee> GetAggregationFluent(bool includeDeleted = false,
      FilterDefinition<Employee> filter = null)
    {
      //Вложенный pipeline для pipeline-let lookup-а заказов в аналитике филиалов
      var pipeline = PipelineDefinition<Order, Order>.Create(new[]
        {
          PipelineStageDefinitionBuilder.Match<Order>(new BsonDocument("$expr",
            new BsonDocument("$or",
              new BsonArray
              {
                new BsonDocument("$eq",
                  new BsonArray
                  {
                    "$InCourier",
                    "$$kindid"
                  }),
                new BsonDocument("$eq",
                  new BsonArray
                  {
                    "$Obtainer",
                    "$$kindid"
                  }),
                new BsonDocument("$eq",
                  new BsonArray
                  {
                    "$WasherCourier",
                    "$$kindid"
                  }),
                new BsonDocument("$eq",
                  new BsonArray
                  {
                    "$Distributor",
                    "$$kindid"
                  }),
                new BsonDocument("$eq",
                  new BsonArray
                  {
                    "$OutCourier",
                    "$$kindid"
                  })
              })))
        }
      );

      return base.GetAggregationFluent(includeDeleted, filter)
        .Lookup<Order, Order, List<Order>, Employee>(
          this.Collection.Database.GetCollection<Order>("orders"), new BsonDocument("kindid", "$_id"), pipeline,
          "Orders")
        .Project<Employee>(ProjectDefinition);

    }

    public override void Update(Employee entity)
    {
      entity.OrdersCountImpl = null;
      entity.OrdersPriceImpl = null;

      base.Update(entity);
    }

    /// <summary>
    /// Задать хэш пароля работника с солью
    /// </summary>
    /// <param name="employee">Работник</param>
    /// <param name="password">Пароль</param>
    public void SetPassword(Employee employee, string password)
    {
      byte[] salt;
      new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

      var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
      byte[] hash = pbkdf2.GetBytes(20);

      byte[] hashBytes = new byte[36];
      Array.Copy(salt, 0, hashBytes, 0, 16);
      Array.Copy(hash, 0, hashBytes, 16, 20);

      string savedPasswordHash = Convert.ToBase64String(hashBytes);

      this.Collection.UpdateOne(Builders<Employee>.Filter.Eq(x => x.Id, employee.Id),
        Builders<Employee>.Update.Set("Password", savedPasswordHash));
    }
    /// <summary>
    /// Получить хэш работника из бд
    /// </summary>
    /// <param name="employee">Работник</param>
    /// <returns></returns>
    private string GetPasswordHash(Employee employee)
    {
      return this.Collection
        .Aggregate()
        .Match(x => x.Id == employee.Id)
        .Project<BsonDocument>("{ Password:'$Password'} ")
        .First()
        .ToBsonDocument()["Password"].AsString;
    }
    /// <summary>
    /// Авторизоваться в системе и получть
    /// </summary>
    /// <param name="username">Имя пользователя</param>
    /// <param name="password">Пароль</param>
    /// <returns>Работник по данной паре логин-пароль</returns>
    /// <exception cref="UnauthorizedAccessException">Исключение в случае
    /// несовпадения пары логин-пароль или отсутствия пользователя с такиим логичном</exception>
    public Employee GetCurrentEmployee(string username, string password)
    {
      Employee user = null;
      try
      {
        user = this.Collection.Find(Builders<Employee>.Filter.Eq("Username", username)).First();
      }
      catch (InvalidOperationException)
      {
        throw new UnauthorizedAccessException();
      }

      //Проверка хэша пароля на соответствие
      byte[] hashBytes = Convert.FromBase64String(GetPasswordHash(user));
      /* Get the salt */
      byte[] salt = new byte[16];
      Array.Copy(hashBytes, 0, salt, 0, 16);
      /* Compute the hash on the password the user entered */
      var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
      byte[] hash = pbkdf2.GetBytes(20);
      /* Compare the results */
      for (int i = 0; i < 20; i++)
        if (hashBytes[i + 16] != hash[i])
          throw new UnauthorizedAccessException();
      return user;
    }

    public EmployeeRepository(IModel model, IMongoCollection<Employee> collection)
      : base(model, collection, new[] {nameof(Client.Name), nameof(Client.Surname), nameof(Client.Patronymic)})
    {
    }

    public void SetSubsidiary(Employee employee, Subsidiary subsidiary)
    {
      if (subsidiary != null) employee.Subsidiary = subsidiary.Id;
    }
    
    public void SetCar(Employee entity, Car car)
    {
      if (car != null) entity.Car = car.Id;
    }

    /// <summary>
    /// Обновить тему для пользователя
    /// </summary>
    /// <param name="currentUser">Пользователь</param>
    /// <param name="IsDark">Темы (true - светлая, false - тёмная)</param>
    public void UpdateTheme(Employee currentUser, bool IsDark)
    {
      this.Collection.UpdateOne(x => x.Id == currentUser.Id, Builders<Employee>.Update.Set(x => x.IsDarkTheme, IsDark));
    }
  }
}