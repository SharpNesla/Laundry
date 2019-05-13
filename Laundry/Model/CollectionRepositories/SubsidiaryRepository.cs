using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Laundry.Model.CollectionRepositories
{
  public class SubsidiaryRepository : Repository<Subsidiary>
  {
    public SubsidiaryRepository(IModel model, IMongoCollection<Subsidiary> collection) : base(model, collection)
    {
    }

    public override IReadOnlyList<Subsidiary> GetBySearchString(string searchString, FilterDefinition<Subsidiary> filter,
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
            })));
      var filterdef = filter ?? Builders<Subsidiary>.Filter.Empty;
      return Collection.Aggregate()
        .Match(filterdef)
        .AppendStage<BsonDocument>(addfields)
        .AppendStage<BsonDocument>(match)
        .Skip(offset)
        .Limit(capLimit)
        .ToList().Select(x => BsonSerializer.Deserialize<Subsidiary>(x)).ToList();
    }

    public override long GetSearchStringCount(string searchString, FilterDefinition<Subsidiary> filter = null)
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
            })));
      var filterdef = filter ?? Builders<Subsidiary>.Filter.Empty;
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

    public double GetAggregatedPrice(FilterDefinition<Subsidiary> filter)
    {
      var group = new BsonArray
      {
        new BsonDocument("$lookup",
          new BsonDocument
          {
            {"from", "orders"},
            {"localField", "_id"},
            {"foreignField", "InSubsidiary"},
            {"as", "Order"}
          }),
        new BsonDocument("$project",
          new BsonDocument("Price",
            new BsonDocument("$sum", "$Order.Price"))),
        new BsonDocument("$group",
          new BsonDocument
          {
            {"_id", 1},
            {
              "Price",
              new BsonDocument("$sum", "$Price")
            }
          })
      };

      try
      {
        var aggregation = this.Collection.Aggregate()
          .Match(filter)
          .AppendStage<BsonDocument>(group[0].AsBsonDocument)
          .AppendStage<BsonDocument>(group[1].AsBsonDocument)
          .AppendStage<BsonDocument>(group[2].AsBsonDocument).ToList().First();
        return aggregation["Price"].AsDouble;
      }
      catch (InvalidOperationException e)
      {
        return 0;
      }

    }
  }
}
