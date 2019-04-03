using System;
using System.Collections.ObjectModel;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  public enum EmployeeProfession
  {
    Courier,
    Director,
    Washer,
    Advisor,
    Driver
  }

  public class Employee : IMongoCollectionElement
  {
    public long Id { get; set; }

    [BsonIgnoreIfNull]
    public string Name { get; set; }

    [BsonIgnoreIfNull]
    public string Surname { get; set; }

    [BsonIgnoreIfNull]
    public string Patronymic { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DateBirth { get; set; }

    [BsonIgnoreIfDefault]
    public EmployeeProfession Profession { get; set; }

    public int House { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }

    public string Password { get; set; }

    public string Username { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }
  }
}