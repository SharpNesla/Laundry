using System;
using System.Collections.ObjectModel;
using System.Windows;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;
using PropertyChanged;

namespace Laundry.Model
{
  [BsonIgnoreExtraElements]
  public class Client : Person
  {

    [BsonIgnoreIfNull]
    public ObservableCollection<Order> Orders { get; set; }

    [BsonIgnoreIfDefault]
    public bool IsPremiumClient { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }

    [BsonIgnoreIfDefault]
    public bool IsCorporative { get; set; }

    public long OrdersCount { get; internal set; }
  }
}