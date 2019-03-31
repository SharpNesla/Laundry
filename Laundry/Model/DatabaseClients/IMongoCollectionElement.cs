using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model.DatabaseClients
{
  public interface IMongoCollectionElement
  {
    long Id { get; set; }

    [BsonIgnoreIfDefault]
    DateTime DeletionDate { get; set; }
  }
}