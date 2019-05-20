﻿using System;
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
    /// <summary>
    /// Set Hash of employee password with salt
    /// </summary>
    /// <param name="employee">Employee</param>
    /// <param name="password">Password string</param>
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

    private string GetPasswordHash(Employee employee)
    {
      return this.GetById(employee.Id).Password;
    }

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

    public EmployeeRepository(IModel model, IMongoCollection<Employee> collection) : base(model, collection)
    {
    }

    public void SetSubsidiary(Employee employee, Subsidiary subsidiary)
    {
      if (subsidiary != null) employee.Subsidiary = subsidiary.Id;
    }

    public override Employee GetById(long id)
    {
      var byId = base.GetById(id);
      byId.OrdersCount = Model.Orders.GetForEmployeeCount(byId);
      return byId;
    }

    public override IReadOnlyList<Employee> Get(int offset, int limit, FilterDefinition<Employee> filter = null)
    {
      var basee = base.Get(offset, limit, filter);

      foreach (var employee in basee)
      {
        employee.OrdersCount = Model.Orders.GetForEmployeeCount(employee);
      }

      return basee;
    }

    public override IReadOnlyList<Employee> GetBySearchString(string searchString, FilterDefinition<Employee> filter,
      int offset = 0, int capLimit = 10)
    {
      var searchChunks = searchString.Split(' ');

      var regex = @"^";

      foreach (var searchChunk in searchChunks)
      {
        regex += $"(?=.*{searchChunk})";
      }

      regex += @".*$";

      //Бсондокументы, описывающие стадии агрегации (экспортированы из mongo compass)
      //(добавление поля Signature и его match по сооветствующему составляемому регулярному выражению)
      var match = new BsonDocument("$match",
        new BsonDocument("Signature",
          new BsonDocument("$regex", regex)));
      var addfields = new BsonDocument("$addFields",
        new BsonDocument("Signature",
          new BsonDocument("$concat",
            new BsonArray
            {
              new BsonDocument("$toString", "$_id"),
              " ",
              "$Name",
              " ",
              "$Surname"
            })));
      var filterdef = filter ?? Builders<Employee>.Filter.Empty;
      return Collection.Aggregate()
        .Match(filterdef)
        .AppendStage<BsonDocument>(addfields)
        .AppendStage<BsonDocument>(match)
        .Skip(offset)
        .Limit(capLimit)
        .ToList().Select(x => BsonSerializer.Deserialize<Employee>(x)).ToList();
    }

    public void SetCar(Employee entity, Car car)
    {
      if (car != null) entity.Car = car.Id;
    }

    public void UpdateTheme(Employee currentUser, bool IsDark)
    {
      this.Collection.UpdateOne(x=>x.Id == currentUser.Id, Builders<Employee>.Update.Set(x=>x.IsDarkTheme, IsDark));
    }
  }
}