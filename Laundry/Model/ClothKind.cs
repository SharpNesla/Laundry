using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;
using PropertyChanged;

namespace Model
{
  public enum MeasureKind
  {
    [Description("шт")] Thing,
    [Description("кг")] Kg,
    [Description("пар")] Pair
  }

  [AddINotifyPropertyChangedInterface]
  public class ClothKind : RepositoryElement
  {
    [BsonIgnoreIfDefault]
    public long? Parent { get; set; }

    [BsonIgnoreIfNull]
    public string Name { get; set; }

    public MeasureKind MeasureKind { get; set; }

    public float Price { get; set; }

    public float WashPrice { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }
    
    public long ChildrenCount { get; internal set; }

    [BsonIgnore]
    public bool HasChildren => ChildrenCount != 0;

    [BsonIgnore]
    public IReadOnlyList<ClothKind> Children { get; internal set; }

    [BsonIgnore]
    public int Level { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("SumPrice")]
    internal double? SumPriceImpl;

    [BsonIgnoreIfNull]
    [BsonElement("Count")]
    internal int? CountImpl;

    public double Count => CountImpl ?? 0;
    public double SumPrice => SumPriceImpl ?? 0;
  }
}