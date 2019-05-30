using System;
using System.Collections.Generic;
using Model.DatabaseClients;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Model.CollectionRepositories
{
  /// <summary>
  /// Базовый класс для работы с коллекциями элементов типа T
  /// </summary>
  /// <typeparam name="T">Тип объекта находящегося в коллекции</typeparam>
  public class Repository<T> where T : RepositoryElement
  {
    private readonly string[] _searchStringCriterias;
    internal IMongoCollection<T> Collection { get; set; }
    public event Action ConnectionLost;
    protected IModel Model { get; set; }

    public Repository(IModel model, IMongoCollection<T> collection, string[] searchStringCriterias = null)
    {
      _searchStringCriterias = searchStringCriterias;
      this.Collection = collection;
      this.Model = model;
    }

    protected virtual IAggregateFluent<T> GetAggregationFluent(bool includeDeleted = false,
      FilterDefinition<T> filter = null)
    {
      var filters = includeDeleted
        ? Builders<T>.Filter.Empty
        : Builders<T>.Filter.Exists(nameof(RepositoryElement.DeletionDate), false);

      return this.Collection.Aggregate().Match(filters);
    }

    /// <summary>
    /// Получить некоторое количество элементов 
    /// коллекции с некоторым смещением с учётом фильтра
    /// </summary>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Количество</param>
    /// <param name="filter">Фильтр (по умолчанию null)</param>
    /// <returns>Readonly лист элементов</returns>
    public virtual IReadOnlyList<T> Get(int offset, int limit, FilterDefinition<T> filter = null)
    {
      try
      {
        return GetAggregationFluent().Match(filter ?? Builders<T>.Filter.Empty).Skip(offset).Limit(limit).ToList();
      }
      catch (Exception)
      {
        return null;
      }
    }

    /// <summary>
    /// Получить количество всех элементов в коллекции с учётом фильтра
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <returns></returns>
    public virtual long GetCount(FilterDefinition<T> filter = null)
    {
      try
      {
        var filters = Builders<T>.Filter.And(
          Builders<T>.Filter.Exists(nameof(RepositoryElement.DeletionDate), false),
          filter ?? Builders<T>.Filter.Empty);
        return Collection.CountDocuments(filters);
      }
      catch (Exception)
      {
        ConnectionLost?.Invoke();
        return 0;
      }
    }

    public virtual long GetSearchStringCount(string searchString, FilterDefinition<T> filter = null)
    {
      try
      {
        var result = GetBySearchStringFluent(searchString, filter).Count().First();
        return result.Count;
      }
      catch (InvalidOperationException)
      {
        return 0;
      }
    }
    
    protected IAggregateFluent<T> GetBySearchStringFluent(string searchString, FilterDefinition<T> filter)
    {
      var searchChunks = searchString.Split(' ');

      var regex = @"^";

      foreach (var searchChunk in searchChunks)
      {
        regex += $"(?=.*{searchChunk})";
      }

      regex += @".*$";

      var bsonArray = new BsonArray
      {
        new BsonDocument("$toString", "$_id"),
        " ",
      };

      if (_searchStringCriterias != null)
      {
        foreach (var fieldDefinition in _searchStringCriterias)
        {
          bsonArray.Add($"${fieldDefinition}");
          bsonArray.Add(" ");
        }
      }
      
      var addfields = new BsonDocument("$addFields",
        new BsonDocument("Signature",
          new BsonDocument("$concat",
            bsonArray)));

      var filterdef = filter ?? Builders<T>.Filter.Empty;
      return this.GetAggregationFluent()
        .Match(filterdef)
        .AppendStage<BsonDocument>(addfields)
        .Match(Builders<BsonDocument>.Filter.Regex(nameof(RepositoryElement.Signature), regex))
        .As<T>();
    }

    /// <summary>
    /// Получить элементы, по поисковой строке с учётом фильтров
    /// </summary>
    /// <param name="searchString">Строка поиска</param>
    /// <param name="filter">Фильтр</param>
    /// <param name="offset">Смещение</param>
    /// <param name="capLimit">Количество элементов</param>
    /// <returns></returns>
    public virtual IReadOnlyList<T> GetBySearchString(string searchString, FilterDefinition<T> filter, int offset = 0,
      int capLimit = 10)
    {
      return this.GetBySearchStringFluent(searchString, filter)
        .Skip(offset)
        .Limit(capLimit)
        .ToList();
    }

    /// <summary>
    /// Добавить элемент в текущую коллекцию
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Add(T entity)
    {
      try
      {
        entity.Id = Collection.Aggregate().SortByDescending(x => x.Id).First().Id + 1;
      }
      catch
      {
        entity.Id = 0;
      }

      Collection.InsertOne(entity);
    }

    /// <summary>
    /// Получить элемент из коллекции по id
    /// </summary>
    /// <param name="id">id элементы</param>
    /// <returns>Элемент коллекции</returns>
    public virtual T GetById(long id)
    {
      return GetAggregationFluent().Match(Builders<T>.Filter.Eq(nameof(RepositoryElement.Id), id)).First();
    }

    /// <summary>
    /// Обновление записи в коллекции путём замены
    /// </summary>
    /// <param name="entity">Entity на замену</param>
    public virtual void Update(T entity)
    {
      Collection.ReplaceOne(x => x.Id == entity.Id, entity);
    }

    /// <summary>
    /// Установление даты удаления для элемента коллекции
    /// </summary>
    /// <param name="entity">Удаляемый элемент</param>
    public virtual void Remove(T entity)
    {
      Collection.UpdateOne(Builders<T>.Filter.Where(x => x.Id == entity.Id),
        Builders<T>.Update.Set(nameof(entity.DeletionDate), DateTime.Now));
    }
  }
}