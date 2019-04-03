using System.Collections.Generic;
using MongoDB.Driver;

namespace Laundry.Model.DatabaseClients
{
  public class ClientRepository : Repository<Client>
  {
    public override IList<Client> Get(int offset, int limit)
    {
      var clients = base.Get(offset, limit);
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