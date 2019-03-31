﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laundry.Model.DatabaseClients
{
  public class EmployeeCollectionActions : CollectionActions<Employee>
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
      return this.GetById(employee.Id).PasswordHash;
    }

    public Employee GetLoginEmployee(string username, string password)
    {
      var user = this.Collection.Find(Builders<Employee>.Filter.Eq("Username", username)).First();
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

    public EmployeeCollectionActions(IModel model, IMongoCollection<Employee> collection) : base(model, collection)
    {
    }
  }
}