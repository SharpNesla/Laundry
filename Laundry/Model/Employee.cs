using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
  public enum EmployeeProfession
  {
    [Description("Курьер")] Courier = 0,
    [Description("Директор")] Director,
    [Description("Прачечник")] Washer,
    [Description("Приёмщик")] Advisor,
    [Description("Водитель")] Driver
  }
  
  public class Employee : Person
  {
    public EmployeeProfession Profession { get; set; }

    public bool IsCourierCarDriver { get; set; }

    public long Subsidiary { get; internal set; }

    public long Car { get; internal set; }

    public string Username { get; set; }

    public string PassportSerial { get; set; }

    public string PassportDistributor { get; set; }

    public string PassportDistributorCode { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }
    
    public override string Signature
    {
      get { return $"{this.Id} {this.Name} {this.Surname}"; }
    }
    
    [BsonElement("OrdersCount")]
    internal long? OrdersCountImpl;

    [BsonIgnoreIfNull]
    [BsonElement("OrdersPrice")]
    internal long? OrdersPriceImpl;

    [BsonIgnore]
    public long OrdersCount => OrdersCountImpl ?? 0;

    [BsonIgnore]
    public long OrdersPrice => OrdersPriceImpl ?? 0;

    public bool IsDarkTheme { get; set; }

    public string Password { get; set; }
  }
}