using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Laundry.Model.DatabaseClients
{
  public class OrderRepository : Repository<Order>
  {
    public OrderRepository(IModel model, IMongoCollection<Order> collection) : base(model, collection)
    {
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
      var orderById = base.GetById(id);
      orderById.Client = Model.Clients.GetById(orderById.ClientId);
      return orderById;
    }

    public override IList<Order> Get(int offset, int limit)
    {
      var orders = base.Get(offset, limit);
      foreach (var x in orders) x.Client = Model.Clients.GetById(x.ClientId);
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

    public IList<Order> GetForEmployee(Employee employee, int offset, int limit)
    {
      var filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.Distributer), employee.Id)
      );
      var orders = Collection.Find(filter).Skip(offset).Limit(limit).ToList();
      return orders;
    }

    public long GetForEmployeeCount(Employee employee)
    {
      var filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.ClientId), employee.Id)
      );
      return Collection.CountDocuments(filter);
    }
  }
}