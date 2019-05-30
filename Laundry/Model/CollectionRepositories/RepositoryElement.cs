using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.DatabaseClients
{
  /// <summary>
  /// Базовый абстрактный класс для работы с сущностями базы данных
  /// </summary>
  public abstract class RepositoryElement
  {
    /// <summary>
    /// Id элемента в БД
    /// </summary>
    [BsonId]
    public long Id { get; set; }


    /// <summary>
    /// Дата удаления при её наличии в бд
    /// (Игнорируется на добавление в бд при значении по умолчанию)
    /// </summary>
    [BsonIgnoreIfDefault]
    public DateTime DeletionDate { get; set; }

    [BsonIgnore]
    public virtual string Signature => Id.ToString();

    public bool IsSelected { get; set; }
  }
}