using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;
using PropertyChanged;
namespace Laundry.Model
{
  public enum MeasureKind
  {
    Kg,
    Thing,
    Pair
  }

  [AddINotifyPropertyChangedInterface]
  public class ClothKind : IRepositoryElement
  {
    public long Parent { get; set; }
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

    public long ChildrenCount { get; internal set; }

    public bool HasChildren
    {
      get { return ChildrenCount != 0; }
    }
    public IReadOnlyList<ClothKind> MyCollection { get; internal set; }
    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }
    public string Signature { get; }
  }
}