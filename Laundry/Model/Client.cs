﻿using System;
using System.Collections.ObjectModel;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model
{
  [BsonIgnoreExtraElements]
  public class Client : IRepositoryElement
  {
    public long Id { get; set; }

    [BsonIgnoreIfNull]
    public string Name { get; set; }

    [BsonIgnoreIfNull]
    public string Surname { get; set; }

    [BsonIgnoreIfNull]
    public string Patronymic { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DateBirth { get; set; }

    [BsonIgnoreIfNull]
    public ObservableCollection<Order> Orders { get; set; }

    [BsonIgnoreIfNull]
    public string PhoneNumber { get; set; }

    [BsonIgnoreIfDefault]
    public int House { get; set; }

    [BsonIgnoreIfNull]
    public string Street { get; set; }

    [BsonIgnoreIfNull]
    public string City { get; set; }

    [BsonIgnoreIfDefault]
    public int ZipCode { get; set; }

    [BsonIgnoreIfDefault]
    public bool IsPremiumClient { get; set; }

    [BsonIgnoreIfNull]
    public string Comment { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }
    
    [BsonIgnore]
    public string Signature
    {
      get { return $"{this.Id} {this.Name} {this.Surname}"; }
    }
    [BsonIgnoreIfDefault]
    public bool IsCorporative { get; set; }
    public long OrdersCount { get; internal set; }
  }
}