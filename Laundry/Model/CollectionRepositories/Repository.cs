﻿using System;
using System.Collections.Generic;
using Model.DatabaseClients;
using MongoDB.Driver;

namespace Model.CollectionRepositories
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

    protected virtual IAggregateFluent<T> GetAggregationFluent(bool includeDeleted = false,
      FilterDefinition<T> filter = null)
    {
      var filters = Builders<T>.Filter.And(
        includeDeleted ? Builders<T>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false) : Builders<T>.Filter.Empty,
        filter ?? Builders<T>.Filter.Empty);

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
        return GetAggregationFluent(filter).Skip(offset).Limit(limit).ToList();
      }
      catch (Exception e)
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

    public virtual long GetSearchStringCount(string searchString, FilterDefinition<T> filter = null)
    {
      return this.GetCount();
    }

    public virtual IReadOnlyList<T> GetBySearchString(string searchString, FilterDefinition<T> filter, int offset = 0,
      int capLimit = 10)
    {
      return this.Get(0, 10, filter);
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
    /// <param name="id">id</param>
    /// <returns></returns>
    public virtual T GetById(long id)
    {
      return Collection.Find(Builders<T>.Filter.Eq(nameof(IRepositoryElement.Id), id)).First();
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
    /// <param name="entity"></param>
    public virtual void Remove(T entity)
    {
      Collection.UpdateOne(Builders<T>.Filter.Where(x => x.Id == entity.Id),
        Builders<T>.Update.Set(nameof(entity.DeletionDate), DateTime.Now));
    }
  }
}