using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
  public class DiscountEdge : IRepositoryElement
  {
    [BsonId]
    public long Id { get; set; }
    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }
    [BsonIgnoreIfDefault]
    public double Edge { get; set; }
    [BsonIgnoreIfDefault]
    public double Discount { get; set; }

    [BsonIgnore]
    public string Signature { get; }
    [BsonIgnoreIfDefault]
    public bool IsSelected { get; set; }
  }
}
