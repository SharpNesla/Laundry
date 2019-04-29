using System;
using System.Collections.Generic;
using Laundry.Model.DatabaseClients;
using MongoDB.Driver;

namespace Laundry.Model.CollectionRepositories
{
  /// <summary>
  /// Базовый класс для работы с коллекциями элементов типа T
  /// </summary>
  /// <typeparam name="T">Тип объекта находящегося в коллекции</typeparam>
  public class Repository<T> where T : IRepositoryElement
  {
    protected IMongoCollection<T> Collection { get; set; }
    internal event Action ConnectionLost;
    protected IModel Model { get; set; }

    public Repository(IModel model, IMongoCollection<T> collection)
    {
      this.Collection = collection;
      this.Model = model;
    }

    /// <summary>
    /// Добавить элемент в текущую коллекцию
    /// </summary>
    /// <param name="entity"></param>
    public virtual void Add(T entity)
    {
      try
      {
        entity.Id = GetNextId();
      }
      catch
      {
        entity.Id = 0;
      }

      Collection.InsertOne(entity);
    }

    public long GetNextId()
    {
      return Collection.Aggregate().SortByDescending(x => x.Id).First().Id + 1;
    }

    /// <summary>
    /// Получить элемент из коллекции по id
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    public virtual T GetById(long id)
    {
      return Collection.Find(Builders<T>.Filter.Eq(nameof(IRepositoryElement.Id), id)).First();
    }

    public virtual void Update(T entity)
    {
      Collection.ReplaceOne(x => x.Id == entity.Id, entity);
    }

    public void Remove(T entity)
    {
      Collection.UpdateOne(Builders<T>.Filter.Where(x => x.Id == entity.Id),
        Builders<T>.Update.Set(nameof(entity.DeletionDate), DateTime.Now));
    }

    public virtual IReadOnlyList<T> Get(int offset, int limit, FilterDefinition<T> filter = null)
    {
      try
      {
        var filters = Builders<T>.Filter.And(
          Builders<T>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false),
          filter ?? Builders<T>.Filter.Empty);
        return Collection.Find(filters).Skip(offset).Limit(limit).ToList();
      }
      catch (Exception e)
      {
        throw;
        ConnectionLost?.Invoke();
        return null;
      }
    }

    public virtual long GetCount(FilterDefinition<T> filter = null)
    {
      try
      {
        var filters = Builders<T>.Filter.And(
          Builders<T>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false),
          filter ?? Builders<T>.Filter.Empty);
        return Collection.CountDocuments(filters);
      }
      catch (Exception e)
      {
        ConnectionLost?.Invoke();
        return 0;
      }
    }

    public virtual long GetSearchStringCount(string searchString ,FilterDefinition<T> filter = null)
    {
      return this.GetCount();
    }

    public virtual IReadOnlyList<T> GetBySearchString(string searchString, FilterDefinition<T> filter, int offset = 0,
      int capLimit = 10)
    {
      return this.Get(0, 10, filter);
    }
  }
}