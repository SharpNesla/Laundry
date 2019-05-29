using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.CollectionRepositories
{
  /// <summary>
  /// Результат агрегаций для заказов и видов одежды
  /// </summary>
  public class AggregationResult
  {
    /// <summary>
    /// Дата по которой ведётся отбор
    /// </summary>
    [BsonId]
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Суммарная цена
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// Суммарное количество штучных (парных) вещей
    /// </summary>
    public long Count { get; set; }
    /// <summary>
    /// Суммарное количество килограмм
    /// </summary>
    public double UnCountableCount { get; set; }
  }

  public class SubsidiaryAggregationResult
  {
    /// <summary>
    /// Id филиала (по которому ведётся группировка)
    /// </summary>
    [BsonId] public int SubsidiaryId;
    /// <summary>
    /// Id и адресс филиала
    /// </summary>
    public string Signature { get; set; }
    /// <summary>
    /// Суммарная цена
    /// </summary>
    public double Price { get; set; }
    /// <summary>
    /// Суммарное количество штучных (парных) вещей
    /// </summary>
    public long Count { get; set; }
    /// <summary>
    /// Суммарное количество килограмм
    /// </summary>
    public double UnCountableCount { get; set; }
  }
}
