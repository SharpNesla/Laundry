using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laundry.Model.CollectionRepositories
{
  public class ClothInstancesRepository : Repository<ClothInstance>
  {
    private List<ClothInstance> _unregistredPool; 

    public ClothInstancesRepository(IModel model, IMongoCollection<ClothInstance> collection) : base(model, collection)
    {
      _unregistredPool = new List<ClothInstance>();
    }

    public override IReadOnlyList<ClothInstance> Get(int offset, int limit, FilterDefinition<ClothInstance> filter = null)
    {
        var filters = Builders<ClothInstance>.Filter.And(
          Builders<ClothInstance>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false),
          filter ?? Builders<ClothInstance>.Filter.Empty);
        

        return Collection
          .Aggregate()
          .Match(filters)
          .Lookup("clothkinds","ClothKind", "_id", "ClothKindObj")
          .Unwind<ClothInstance>("ClothKindObj")
          
          .Skip(offset).Limit(limit).ToList().AsReadOnly();
      }

    public void AddUnRegistred(ClothInstance instance)
    {
      _unregistredPool.Add(instance);
    }

    public void RegisterUnregistred()
    {
      foreach (var clothInstance in _unregistredPool)
      {
        this.Add(clothInstance);
      }

      this._unregistredPool.Clear();
    }

    public void SetOrder(ClothInstance instance, Order order)
    {
      instance.Order = order.Id;
    }

    public long GetForOrderCount(Order order)
    {
      var filter = Builders<ClothInstance>.Filter.And(
        Builders<ClothInstance>.Filter.Exists(nameof(ClothInstance.DeletionDate), false),
        Builders<ClothInstance>.Filter.Eq(nameof(ClothInstance.Order), order.Id)
      );
      return Collection.CountDocuments(filter);
    }

    public long GetUnRegistredCount()
    {
      return _unregistredPool.Count;
    }

    public void ClearUnRegistred()
    {
      this._unregistredPool.Clear();
    }

    public IReadOnlyList<ClothInstance> GetForOrder(int offset, int limit, Order order)
    {
      return this.Get(offset, limit, Builders<ClothInstance>.Filter.Eq(nameof(ClothInstance.Order), order.Id));
    }

    public IReadOnlyList<ClothInstance> GetUnRegistred(int offset, int limit)
    {
      return _unregistredPool.Skip(offset).Take(limit).ToList();
    }
  }
}
