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
    [BsonIgnoreIfNull]
    public IList<ClothInstance> ClothInstances { get; set; }

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

    #region Reference Objects

    [BsonIgnore]
    public Client Client
    {
      get { return _client; }
      set
      {
        _client = value;
        this.ClientId = _client.Id;
      }
    }

    [BsonIgnore]
    public Employee Obtainer { get; set; }

    [BsonIgnore]
    public Employee WasherCourier { get; set; }

    [BsonIgnore]
    public Employee OutWasherCourier { get; set; }

    [BsonIgnore]
    public Employee Distributer { get; set; }

    #endregion

    public Order()
    {
      this.ClothInstances = new List<ClothInstance>();
    }
  }
}