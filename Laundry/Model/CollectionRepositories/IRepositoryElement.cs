using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.DatabaseClients
{
  public class RepositoryElement
  {
    [BsonId]
    public long Id { get; set; }

    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    [BsonIgnore]
    public virtual string Signature => Id.ToString();

    public bool IsSelected { get; set; }
  }
}