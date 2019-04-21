using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  public class ClothInstance : IRepositoryElement
  {
    public long ClothKind { get; set; }

    [BsonIgnoreIfDefault]
    public int WearPercentage { get; set; }

    [BsonIgnoreIfDefault]
    public int Amount { get; set; }

    public long TagNumber { get; set; }

    [BsonId]
    public long Id { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    [BsonIgnore]
    public string Signature { get; }

    public ClothKind ClothKindObj { get; set; }

    [BsonIgnore]
    public bool Selected { get; set; }
    
    public long Order { get; set; }
    [BsonIgnore]
    public float Price
    {
      get { return this.ClothKindObj.Price * Amount; }
    }
  }
}