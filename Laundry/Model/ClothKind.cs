﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [Description("шт")]
    Thing,
    [Description("кг")]
    Kg,
    [Description("пар")]
    Pair
  }

  [AddINotifyPropertyChangedInterface]
  public class ClothKind : IRepositoryElement
  {
    [BsonIgnoreIfDefault]
    public long? Parent { get; set; }
    [BsonIgnoreIfNull]
    public string Name { get; set; }
    
    public MeasureKind MeasureKind { get; set; }

    [BsonIgnoreIfDefault]
    public float Price { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }
    [BsonId]
    public long Id { get; set; }
    [BsonIgnore]
    public long ChildrenCount { get; internal set; }

    [BsonIgnore]
    public bool HasChildren => ChildrenCount != 0;

    [BsonIgnore]
    public IReadOnlyList<ClothKind> Children { get; internal set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }
    public string Signature { get; }

    [BsonIgnore]
    public bool IsSelected { get; set; }
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