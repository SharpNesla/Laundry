using System;
using System.Collections.Generic;
using System.ComponentModel;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace Laundry.Model
{
  public enum OrderStatus
  {
    [Description("Принят")] Taken,

    [Description("Перевозка на прачечную")]
    MoveFromSubs,
    [Description("Готов к стирке")] ReadyToWash,
    [Description("В стирке")] Washing,
    [Description("Постиран")] Washed,

    [Description("Перевозка из прачечной")]
    MoveToSubs,
    [Description("Выдан")] Granted
  }
  [BsonIgnoreExtraElements]
  public class Order : IRepositoryElement
  {
    [BsonIgnoreIfDefault]
    public double CustomPrice { get; set; }

    public long Id { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime CreationDate { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime ExecutionDate { get; set; }

    public OrderStatus Status { get; set; }
    
    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    [BsonIgnore]
    public string Signature { get; }

    [BsonIgnore]
    public DiscountEdge DiscountEdge { get; set; }

    [BsonIgnore]
    public bool IsSelected { get; set; }

    [BsonIgnoreIfDefault]
    public bool IsCorporative { get; set; }

    [BsonIgnoreIfDefault]
    public bool IsDiscount { get; set; }

    [BsonIgnoreIfDefault]
    public bool IsCustomPrice { get; set; }

    public string InstancesCount
    {
      get
      {
        return
          $"{this.Instances.Where(x => x.ClothKindObj.MeasureKind == MeasureKind.Thing || x.ClothKindObj.MeasureKind == MeasureKind.Pair).Sum(x => x.Amount)}шт, " +
          $"{this.Instances.Where(x => x.ClothKindObj.MeasureKind == MeasureKind.Kg).Sum(x => x.Amount)}кг";
      }
    }


    [BsonIgnore]
    public double CalculatedPrice => Instances.Sum(x => x.Price);


    public double Price
    {
      get
      {
        var price = IsCustomPrice ? CustomPrice : CalculatedPrice;
        return price;
      }
      set { CustomPrice = value; }
    }

    [BsonIgnore]
    public double DiscountPrice
    {
      get { return Price * (1 - this.DiscountEdge.Discount / 100); }
    }

    public List<ClothInstance> Instances { get; set; }

    public Order()
    {
      this.Instances = new List<ClothInstance>();
      this.CreationDate = DateTime.Now;
      this.ExecutionDate = DateTime.Now;
    }

    public long InSubsidiary { get; internal set; }
    public long OutSubsidiary { get; internal set; }
    #region Ids

    [BsonElement("Client")]
    public long ClientId { get; internal set; }

    [BsonElement("InCourier")]
    public long InCourierId { get; set; }

    [BsonElement("Obtainer")]
    public long ObtainerId { get; set; }

    [BsonElement("CorpObtainer")]
    public long CorpObtainerId { get; set; }

    [BsonElement("WasherCourier")]
    public long WasherCourierId { get; set; }

    [BsonElement("OutCourier")]
    public long OutCourierId { get; set; }

    [BsonElement("CorpDistributer")]
    public long CorpDistributerId { get; set; }

    [BsonElement("Distributer")]
    public long DistributerId { get; set; }

    #endregion
  }
}