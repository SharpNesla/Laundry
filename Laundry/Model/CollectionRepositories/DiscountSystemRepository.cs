using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DatabaseClients;
using MongoDB.Driver;

namespace Model.CollectionRepositories
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

    public DiscountEdge GetForOrder(Order order)
    {
      var filters = Builders<DiscountEdge>.Filter.Lte(nameof(DiscountEdge.Edge), order.Price);
      return Collection.Find(filters).SortByDescending(x => x.Edge).First();
    }

    public DiscountSystemRepository(IModel model, IMongoCollection<DiscountEdge> collection) : base(model, collection)
    {
    }

    public DiscountEdge GetForClient(Client client)
    {
      var filters = Builders<DiscountEdge>.Filter.And(
        Builders<DiscountEdge>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false),
        Builders<DiscountEdge>.Filter.Gte(nameof(DiscountEdge.Edge), client.OrdersPrice));
      return Collection.Find(filters).First();
    }
  }
}