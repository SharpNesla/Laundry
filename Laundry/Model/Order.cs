using System;
using System.Collections.Generic;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  public enum OrderState
  {
    Taken,
    MoveFromSubs,
    Washing,
    MoveToSubs,
    Granted
  }

  public class Order : IRepositoryElement
  {
    private Client _client;
    public long Id { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime CreationDate { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime ExecutionDate { get; set; }

    [BsonIgnoreIfDefault]
    public OrderState Status { get; set; }

    [BsonIgnoreIfDefault]
    public float Price { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    public string Signature { get; }

    [BsonIgnore]
    public bool Selected { get; set; }

    [BsonIgnoreIfDefault]
    public bool IsCorporative { get; set; }

    [BsonIgnoreIfDefault]
    public bool IsDiscount { get; set; }

    public IList<ClothInstance> Instances { get; set; }

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