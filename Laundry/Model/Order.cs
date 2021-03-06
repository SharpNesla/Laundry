﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace Model
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
    [Description("Готов к выдаче")] ReadyToDistribute,
    [Description("Выполнен")] Distributed
  }

  [BsonIgnoreExtraElements]
  public class Order : RepositoryElement
  {
    [BsonIgnoreIfNull]
    public string Comment { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime CreationDate { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime ExecutionDate { get; set; }

    public OrderStatus Status { get; set; }

    [BsonIgnore]
    public DiscountEdge DiscountEdge { get; internal set; }
    
    public bool IsCorporative { get; set; }

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


    public double Price { get; set; }

    public List<ClothInstance> Instances { get; set; }

    public Order()
    {
      this.Instances = new List<ClothInstance>();
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