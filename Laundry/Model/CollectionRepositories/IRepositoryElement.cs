using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.DatabaseClients
{

  public interface IRepositoryElement
  {
    [BsonId]
    long Id { get; set; }

    [BsonIgnoreIfDefault]
    DateTime DeletionDate { get; set; }
    [BsonIgnore]
    string Signature { get; }

    bool IsSelected { get; set; }
  }
}