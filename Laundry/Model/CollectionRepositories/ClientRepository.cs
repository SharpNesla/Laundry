using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Laundry.Model.CollectionRepositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Linq;
using NPOI.SS.Formula.Functions;

namespace Laundry.Model.DatabaseClients
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

    public override IReadOnlyList<Client> Get(int offset, int limit, FilterDefinition<Client> filter = null)
    {
      var readOnlyList = this.Collection.Aggregate()
        .Match(Builders<Client>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false))
        .Lookup("orders", "_id", "Client", "Orders")
        .Project<Client>(ClientProjectDefinition)
        .Match(filter ?? Builders<Client>.Filter.Empty)
        .Skip(offset)
        .Limit(limit)
        .ToList();
      return readOnlyList;
    }

    public override Client GetById(long id)
    {
      return Collection.Aggregate()
        .Match(Builders<Client>.Filter.Eq(nameof(IRepositoryElement.Id), id))
        .Lookup("orders", "_id", "Client", "Orders")
        .Project<Client>(ClientProjectDefinition).First();
    }

    public ClientRepository(IModel model, IMongoCollection<Client> collection) : base(model, collection)
    {
    }

    public override void Update(Client entity)
    {
      entity.OrdersCountImpl = null;
      entity.OrdersPriceImpl = null;

      base.Update(entity);
    }

    public override IReadOnlyList<Client> GetBySearchString(string searchString, FilterDefinition<Client> filter,
      int offset = 0, int capLimit = 10)
    {
      var searchChunks = searchString.Split(' ');

      var regex = @"^";

      foreach (var searchChunk in searchChunks)
      {
        regex += $"(?=.*{searchChunk})";
      }

      regex += @".*$";

      //Бсондокументы, описывающие стадии агрегации (экспортированы из mongo compass)
      //(добавление поля Signature и его match по сооветствующему составляемому регулярному выражению)
      var match = new BsonDocument("$match",
        new BsonDocument("Signature",
          new BsonDocument("$regex", regex)));
      var addfields = new BsonDocument("$addFields",
        new BsonDocument("Signature",
          new BsonDocument("$concat",
            new BsonArray
            {
              new BsonDocument("$toString", "$_id"),
              " ",
              "$Name",
              " ",
              "$Surname"
            })));
      var filterdef = filter ?? Builders<Client>.Filter.Empty;
      return Collection.Aggregate()
        .Match(filterdef)
        .AppendStage<BsonDocument>(addfields)
        .AppendStage<BsonDocument>(match)
        .As<Client>()
        .Skip(offset)
        .Limit(capLimit)
        .Project<Client>(ClientProjectDefinition)
        .ToList();
    }

    public override long GetSearchStringCount(string searchString, FilterDefinition<Client> filter = null)
    {
      var searchChunks = searchString.Split(' ');

      var regex = @"^";

      foreach (var searchChunk in searchChunks)
      {
        regex += $"(?=.*{searchChunk})";
      }

      regex += @".*$";

      //Бсондокументы, описывающие стадии агрегации (экспортированы из mongo compass)
      //(добавление поля Signature и его match по сооветствующему составляемому регулярному выражению)
      var match = new BsonDocument("$match",
        new BsonDocument("Signature",
          new BsonDocument("$regex", regex)));
      var addfields = new BsonDocument("$addFields",
        new BsonDocument("Signature",
          new BsonDocument("$concat",
            new BsonArray
            {
              new BsonDocument("$toString", "$_id"),
              " ",
              "$Name",
              " ",
              "$Surname"
            })));
      var filterdef = filter ?? Builders<Client>.Filter.Empty;
      try
      {
        var result = Collection.Aggregate()
          .Match(filterdef)
          .AppendStage<BsonDocument>(addfields)
          .AppendStage<BsonDocument>(match).Count().First();
        return result.Count;
      }
      catch (InvalidOperationException e)
      {
        return 0;
      }
    }
  }
}