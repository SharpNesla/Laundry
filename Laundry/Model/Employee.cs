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
  [BsonIgnoreExtraElements]
  public class Employee : IMongoCollectionElement
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateBirth { get; set; }
    public EmployeeProfession Profession { get; set; }

    public int House { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }

    public string PasswordHash { get; set; }

    public DateTime DeletionDate { get; set; }
  }
}