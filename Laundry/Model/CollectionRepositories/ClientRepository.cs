using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Laundry.Model.CollectionRepositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Linq;

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
        .Skip(offset)
        .Limit(capLimit)
        .ToList().Select(x => BsonSerializer.Deserialize<Client>(x)).ToList();
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