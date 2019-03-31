using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Laundry.Model.DatabaseClients
{
  public class CollectionActions<T> where T : IMongoCollectionElement
  {
    protected IMongoCollection<T> Collection;

    protected IModel Model{ get; set; }

    public CollectionActions(IModel model, IMongoCollection<T> collection)
    {
      this.Collection = collection;
      this.Model = model;
    }

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
      return Collection.Find(Builders<T>.Filter.Eq(nameof(IMongoCollectionElement.Id), id)).First();
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
      return Collection.Find(Builders<T>.Filter.Exists(nameof(IMongoCollectionElement.DeletionDate), false))
        .Skip(offset).Limit(limit).ToList();
    }

    public virtual IList<T> GetFiltered(int offset, int limit, FilterDefinition<T> filter)
    { 
      return Collection.Find(Builders<T>.Filter.Exists(nameof(IMongoCollectionElement.DeletionDate), false))
        .Skip(offset).Limit(limit).ToList();
    }

    public long GetCount()
    {
      return Collection.CountDocuments(Builders<T>.Filter.Exists(nameof(IMongoCollectionElement.DeletionDate), false));
    }
  }
}