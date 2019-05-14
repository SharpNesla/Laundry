using System;
using System.Collections.Generic;
using System.ComponentModel;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;


namespace Model
{
  public enum CarCategory
  {
    [Description("A")]
    A,
    [Description("B")]
    B,
    [Description("C")]
    C,
    [Description("D")]
    D,
    [Description("E")]
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

    public CarCategory Catergory { get; set; }
    [BsonIgnoreIfDefault]
    public CarCategory Category { get; set; }
    
    [BsonIgnoreIfNull]
    public string BodyID { get; set; }

    [BsonIgnoreIfNull]
    public string Color { get; set; }

    [BsonId]
    public long Id { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }
    [BsonIgnore]
    public string Signature
    {
      get { return $"{Id} {BrandAndModel}"; }
    }
    [BsonIgnore]
    public bool IsSelected { get; set; }
    [BsonIgnoreIfNull]
    public short? CreationYear{ get; set; }

    public string Comment { get; set; }
  }
}