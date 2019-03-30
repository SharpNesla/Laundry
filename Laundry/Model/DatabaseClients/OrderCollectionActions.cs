using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Laundry.Model.DatabaseClients
{
  public class OrderCollectionActions : CollectionActions<Order>
  {
    private CollectionActions<Client> _clientCollectionActions;


    public OrderCollectionActions(IMongoCollection<Order> collection, CollectionActions<Client> clientCollectionActions) : base(collection)
    {
      this._clientCollectionActions = clientCollectionActions;
    }

    public IList<Order> GetForClient(Client client, int offset, int limit)
    {
      var filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.ClientId), client.Id)
      );
      var orders = Collection.Find(filter).Skip(offset).Limit(limit).ToList();
      return orders;
    }

    public override Order GetById(long id)
    {
        var orderById = Collection.Find(x => x.Id == id).First();
        orderById.Client = _clientCollectionActions.GetById(orderById.ClientId);
        return orderById;
    }

    public override IList<Order> Get(int offset, int limit)
    {
      var orders = Collection.Find(Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false))
        .Skip(offset).Limit(limit).ToList();
      foreach (var x in orders) x.Client = _clientCollectionActions.GetById(x.ClientId);
      return orders;
    }

    public long GetForClientCount(Client client)
    {
      var filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.ClientId), client.Id)
      );
      return Collection.CountDocuments(filter);
    }
  }
}
