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
    #region Определение проекции для клиента

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

    /// <inheritdoc />
    /// <summary>
    /// Переопределение цепочки для подсчёта стоимости всех заказов и их количества для клиента
    /// </summary>
    /// <param name="includeDeleted"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    protected override IAggregateFluent<Client> GetAggregationFluent(bool includeDeleted = false,
      FilterDefinition<Client> filter = null)
    {
      return base.GetAggregationFluent(includeDeleted, filter)
        .Lookup("orders", "_id", "Client", "Orders")
        .Project<Client>(ClientProjectDefinition);
    }

    //В базовый конструктор передаются критерии поиска клиента
    public ClientRepository(IModel model, IMongoCollection<Client> collection) : base(
      model, collection, new[] {nameof(Client.Name), nameof(Client.Surname), nameof(Client.Patronymic)})
    {
    }

    /// <summary>
    /// Очистка вычисляемых в запросах полей
    /// </summary>
    /// <param name="entity"></param>
    public override void Update(Client entity)
    {
      entity.OrdersCountImpl = null;
      entity.OrdersPriceImpl = null;

      base.Update(entity);
    }
  }
}