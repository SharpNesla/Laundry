using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Model
{
  public class ClothInstance : IRepositoryElement
  {
    public long ClothKind { get; set; }

    [BsonIgnoreIfDefault]
    public int WearPercentage { get; set; }

    [BsonIgnoreIfDefault]
    public int Amount { get; set; }
    [BsonIgnoreIfDefault]
    public long TagNumber { get; set; }
    [BsonIgnoreIfNull]
    public string Comment { get; set; }
    [BsonId]
    public long Id { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    [BsonIgnore]
    public string Signature { get; }
    [BsonIgnore]
    public ClothKind ClothKindObj { get; set; }

    [BsonIgnore]
    public bool IsSelected { get; set; }
    
    [BsonIgnore]
    public float Price
    {
      get { return this.ClothKindObj.Price * Amount; }
    }
    public ClothInstance Clone()
    {
      return new ClothInstance
      {
        ClothKind = this.ClothKind,
        ClothKindObj = this.ClothKindObj,
        TagNumber = this.TagNumber,
        Id = this.Id,
        Amount = this.Amount,
        WearPercentage = this.WearPercentage,
      };
    }
  }
}