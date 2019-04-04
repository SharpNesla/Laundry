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
    protected IMongoCollection<T> Collection;
    internal event Action ConnectionLost;
    protected IModel Model{ get; set; }

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
        entity.Id = Collection.Aggregate().SortByDescending(x => x.Id).First().Id + 1;
      }
      catch
      {
        entity.Id = 0;
      }

      Collection.InsertOne(entity);
    }

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

    public virtual IList<T> Get(int offset, int limit)
    {
      try
      {
        return Collection.Find(Builders<T>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false))
          .Skip(offset).Limit(limit).ToList();
      }
      catch (Exception e)
      {
        ConnectionLost?.Invoke();
        return null;
      }
      
    }

    public virtual IList<T> GetFiltered(int offset, int limit, FilterDefinition<T> filter)
    { 
      return Collection.Find(Builders<T>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false))
        .Skip(offset).Limit(limit).ToList();
    }

    public long GetCount()
    {
      try
      {

        return Collection.CountDocuments(Builders<T>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false));
      }
      catch (Exception e)
      {
        ConnectionLost?.Invoke();
        return 0;
      }
    }
  }
}