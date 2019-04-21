using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Driver;

namespace Laundry.Model.CollectionRepositories
{
  public class DiscountSystemRepository : Repository<DiscountEdge>
  {
    public override IReadOnlyList<DiscountEdge> Get(int offset, int limit, FilterDefinition<DiscountEdge> filter = null)
    {
      var filters = Builders<DiscountEdge>.Filter.And(
        Builders<DiscountEdge>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false),
        filter ?? Builders<DiscountEdge>.Filter.Empty);
      return Collection.Find(filters).Skip(offset).Limit(limit).SortBy(x => x.Edge).ToList();
    }

    public DiscountSystemRepository(IModel model, IMongoCollection<DiscountEdge> collection) : base(model, collection)
    {
    }
  }
}