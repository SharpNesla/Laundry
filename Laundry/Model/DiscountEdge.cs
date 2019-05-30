using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
  public class DiscountEdge : RepositoryElement
  {
    [BsonIgnoreIfDefault]
    public double Edge { get; set; }
    [BsonIgnoreIfDefault]
    public double Discount { get; set; }
  }
}
