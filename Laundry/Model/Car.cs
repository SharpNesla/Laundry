using System;
using System.Collections.Generic;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;


namespace Laundry.Model
{
  public enum CarCategory
  {
    A,
    B,
    C,
    D,
    E
  }

  public class Car : IRepositoryElement
  {
    [BsonIgnoreIfNull]
    public string Sign { get; set; }

    [BsonIgnoreIfNull]
    public string BrandAndModel { get; set; }

    [BsonIgnoreIfNull]
    public string VIN { get; set; }

    //public CarType Type { get; set; }
    [BsonIgnoreIfDefault]
    public CarCategory Category { get; set; }

    [BsonIgnoreIfDefault] public DateTime CreationYear;

    [BsonIgnoreIfNull]
    public string BodyID { get; set; }

    [BsonIgnoreIfNull]
    public string Color { get; set; }

    [BsonId]
    public long Id { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }
    [BsonIgnore]
    public string Signature { get; }

    [BsonIgnoreIfNull]
    [BsonElement(nameof(Couriers))]
    public List<long> CourierIds { get; set; }

    [BsonElement(nameof(Drivers))]
    [BsonIgnoreIfNull]
    public List<long> DriverIds { get; set; }

    [BsonIgnore]
    public List<Employee> Couriers { get; set; }

    [BsonIgnore]
    public List<Employee> Drivers { get; set; }
  }
}