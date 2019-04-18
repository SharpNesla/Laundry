using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Laundry.Model.DatabaseClients
{
  public interface IRepositoryElement
  {
    [BsonId]
    long Id { get; set; }

    [BsonIgnoreIfDefault]
    DateTime DeletionDate { get; set; }
    [BsonIgnore]
    string Signature { get; }

    bool Selected { get; set; }
  }
}