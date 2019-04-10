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
    
    #region Ids

    [BsonElement("Client")]
    public long ClientId { get; set; }

    [BsonElement("Obtainer")]
    public long ObtainerId { get; set; }

    [BsonElement("WasherCourier")]
    public long WasherCourierId { get; set; }

    [BsonElement("OutWasherCourier")]
    public long OutWasherCourierId { get; set; }

    [BsonElement("Distributer")]
    public long DistributerId { get; set; }

    #endregion
    }
}