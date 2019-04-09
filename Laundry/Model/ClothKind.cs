using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  public enum MeasureKind
  {
    Kg,
    Thing,
    Pair
  }

  public class ClothKind : IRepositoryElement
  {
    [BsonIgnoreIfNull]
    public string Name { get; set; }

    [BsonIgnoreIfDefault]
    public MeasureKind MeasureKind { get; set; }

    [BsonIgnoreIfDefault]
    public float Price { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }
    [BsonId]
    public long Id { get; set; }
    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }
    public string Signature { get; }
  }
}