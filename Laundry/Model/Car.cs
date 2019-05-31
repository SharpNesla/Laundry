using System;
using System.Collections.Generic;
using System.ComponentModel;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;


namespace Model
{
  public enum CarCategory
  {
    [Description("A")] A,
    [Description("B")] B,
    [Description("C")] C,
    [Description("D")] D,
    [Description("E")] E
  }
  [BsonIgnoreExtraElements]
  public class Car : RepositoryElement
  {
    [BsonIgnoreIfNull]
    public string Sign { get; set; }

    [BsonIgnoreIfNull]
    public string BrandAndModel { get; set; }

    [BsonIgnoreIfNull]
    public string VIN { get; set; }


    public CarCategory Category { get; set; }

    [BsonIgnoreIfNull]
    public string BodyID { get; set; }

    [BsonIgnoreIfNull]
    public string Color { get; set; }

    [BsonIgnore]
    public override string Signature
    {
      get { return $"{Id} {BrandAndModel}"; }
    }

    [BsonIgnoreIfNull]
    public short? CreationYear { get; set; }

    public string Comment { get; set; }
  }
}