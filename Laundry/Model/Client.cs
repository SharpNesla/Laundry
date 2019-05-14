using System;
using System.Collections.ObjectModel;
using System.Windows;
using Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;
using PropertyChanged;

namespace Model
{
  [BsonIgnoreExtraElements]
  public class Client : Person
  {
    [BsonIgnoreIfDefault]
    public bool IsPremiumClient { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }
    
    public bool IsCorporative { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("OrdersCount")]
    internal long? OrdersCountImpl;

    [BsonIgnoreIfNull]
    [BsonElement("OrdersPrice")]
    internal long? OrdersPriceImpl;
    
    [BsonIgnore]
    public long OrdersCount => OrdersCountImpl ?? 0;

    [BsonIgnore]
    public long OrdersPrice => OrdersPriceImpl ?? 0;
  }
}