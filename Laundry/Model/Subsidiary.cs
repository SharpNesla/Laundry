using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  [BsonIgnoreExtraElements]
  public class Subsidiary : IRepositoryElement
  {
    public long Id { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    public string Signature
    {
      get { return $"{Id} {Name} {Street} {House}"; }
    }

    public long? MainAdvisor { get; internal set; }

    [BsonIgnore]
    public bool IsSelected { get; set; }

    [BsonIgnoreIfNull]
    public string PhoneNumber { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }

    [BsonIgnoreIfNull]
    public string Name { get; set; }

    [BsonIgnoreIfNull]
    public string House { get; set; }

    [BsonIgnoreIfNull]
    public int? Flat { get; set; }

    [BsonIgnoreIfNull]
    public string Street { get; set; }

    [BsonIgnoreIfNull]
    public string City { get; set; }

    [BsonIgnoreIfNull]
    public int? ZipCode { get; set; }
  }
}