using System.Collections.Generic;
using Laundry.Model.CollectionRepositories;
using MongoDB.Driver;

namespace Laundry.Model.DatabaseClients
{
  public class ClientRepository : Repository<Client>
  {
    public override IReadOnlyList<Client> Get(int offset, int limit, FilterDefinition<Client> filter = null)
    {
      var clients = base.Get(offset, limit, filter);
      foreach (var client in clients)
      {
        client.OrdersCount = Model.Orders.GetForClientCount(client);
      }

      return clients;
    }

    public override Client GetById(long id)
    {
      var client = base.GetById(id);
      client.OrdersCount = Model.Orders.GetForClientCount(client);
      return client;
    }

    public ClientRepository(IModel model, IMongoCollection<Client> collection) : base(model, collection)
    {
    }
  }
}