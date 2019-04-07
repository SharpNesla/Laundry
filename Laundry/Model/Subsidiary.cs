using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  public class Subsidiary : IRepositoryElement
  {
    public long Id { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    public string Signature { get; }

    [BsonIgnoreIfDefault]
    public int House { get; set; }

    [BsonIgnoreIfNull]
    public string Street { get; set; }

    [BsonIgnoreIfNull]
    public string City { get; set; }

    [BsonIgnoreIfDefault]
    public int ZipCode { get; set; }
  }
}