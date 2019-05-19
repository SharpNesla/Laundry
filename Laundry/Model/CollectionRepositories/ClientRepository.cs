using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Model.CollectionRepositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Linq;
using NPOI.SS.Formula.Functions;

namespace Model.DatabaseClients
{
  public class ClientRepository : Repository<Client>
  {
    #region Client ProjectDef string

    private const string ClientProjectDefinition = @"
{
  Name:'$Name',
  Surname:'$Surname',
  Patronymic:'$Patronymic',
  PhoneNumber:'$PhoneNumber',
  DateBirth:'$DateBirth',
  Gender:'$Gender',
  House:'$House',
  City:'$City',
  Flat:'$Flat',
  ZipCode:'$ZipCode',
  Comment:'$Comment',
  IsCorporative:'$IsCorporative',
  OrdersCount : {$size:'$Orders'},
  OrdersPrice : {$sum:'$Orders.Price'}
}";

    #endregion

    protected override IAggregateFluent<Client> GetAggregationFluent(bool includeDeleted = false,
      FilterDefinition<Client> filter = null)
    {
      return base.GetAggregationFluent(includeDeleted, filter)
        .Lookup("orders", "_id", "Client", "Orders")
        .Project<Client>(ClientProjectDefinition);
    }

    public ClientRepository(IModel model, IMongoCollection<Client> collection) : base(
      model, collection, new[] {nameof(Client.Name), nameof(Client.Surname), nameof(Client.Patronymic)})
    {
    }

    public override void Update(Client entity)
    {
      entity.OrdersCountImpl = null;
      entity.OrdersPriceImpl = null;

      base.Update(entity);
    }
  }
}