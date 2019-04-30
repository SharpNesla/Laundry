using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Laundry.Model.CollectionRepositories
{
  public class OrderRepository : Repository<Order>
  {
    public OrderRepository(IModel model, IMongoCollection<Order> collection) : base(model, collection)
    {
    }

    public override IReadOnlyList<Order> Get(int offset, int limit, FilterDefinition<Order> filter = null)
    {
      var basee = base.Get(offset, limit, filter);
      foreach (var order in basee)
      {
        foreach (var instance in order.Instances)
        {
          instance.ClothKindObj = Model.ClothKinds.GetById(instance.ClothKind);
        }
      }

      return basee;
    }

    public override Order GetById(long id)
    {
      var order =base.GetById(id);

      foreach (var instance in order.Instances)
      {
        instance.ClothKindObj = Model.ClothKinds.GetById(instance.ClothKind);
      }

      return order;
    }

    public void SetClient(Order order, Client client)
    {
      if (client != null)
      {
        order.ClientId = client.Id;
      }
    }

    public Client GetClient(Order order)
    {
      return Model.Clients.GetById(order.ClientId);
    }


    public IReadOnlyList<Order> GetForClient(Client client, int offset, int limit)
    {
      var filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.ClientId), client.Id)
      );
      var orders = this.Get(offset, limit, filter);
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

    public IReadOnlyList<Order> GetForEmployee(Employee employee, int offset, int limit)
    {
      var filter = Builders<Order>.Filter.And(
        Builders<Order>.Filter.Exists(nameof(Order.DeletionDate), false),
        Builders<Order>.Filter.Eq(nameof(Order.DistributerId), employee.Id)
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


    internal void SetObtainer(Order entity, Employee obtainer)
    {
      if (obtainer != null)
      {
        entity.ObtainerId = obtainer.Id;
        entity.InSubsidiary = obtainer.Subsidiary;
      }
    }

    public void SetObtainer(Order entity, Client corpObtainer)
    {
      if (corpObtainer != null) entity.CorpObtainerId = corpObtainer.Id;
    }

    public void SetInCourier(Order entity, Employee inCourier)
    {
      if (inCourier != null) entity.InCourierId = inCourier.Id;
    }

    public void SetOutCourier(Order entity, Employee outCourier)
    {
      if (outCourier != null) entity.OutCourierId = outCourier.Id;
    }

    public void SetWasher(Order entity, Employee washer)
    {
      if (washer != null) entity.WasherCourierId = washer.Id;
    }

    public void SetDistributer(Order entity, Client corpDistributer)
    {
      if (corpDistributer != null) entity.CorpDistributerId = corpDistributer.Id;
    }

    public void SetDistributer(Order entity, Employee distributer)
    {
      if (distributer != null)
      {
        entity.DistributerId = distributer.Id;
        entity.OutSubsidiary = distributer.Subsidiary;
      }
    }

    public void SetOrdersStatus(IEnumerable<Order> orders, OrderStatus status)
    {
      foreach (var order in orders)
      {
        Collection.UpdateOne(x => x.Id == order.Id, Builders<Order>.Update.Set(x=>x.Status, status));
      }
    }
  }
}