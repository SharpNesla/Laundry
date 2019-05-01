using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  public enum EmployeeProfession
  {
    [Description("Курьер")]
    Courier = 0,
    [Description("Директор")]
    Director,
    [Description("Прачечник")]
    Washer,
    [Description("Приёмщик")]
    Advisor,
    [Description("Водитель")]
    Driver
  }

  public class Employee : Person
  {

    public EmployeeProfession Profession { get; set; }

    public bool IsCourierCarDriver { get; set; }

    public long Subsidiary { get; internal set; }

    public long Car { get; internal set; }

    public string Username { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }

    [BsonIgnore]
    public override string Signature
    {
      get { return $"{this.Id} {this.Name} {this.Surname}"; }
    }

    public string Password { get; set; }
    [BsonIgnore]
    public long OrdersCount { get; set; }
  }
}