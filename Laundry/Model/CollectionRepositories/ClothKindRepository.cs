using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using Laundry.Views;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Laundry.Model.CollectionRepositories
{
  public class ClothKindAggregationResult
  {
    [BsonId]
    public DateTime DateTime { get; set; }

    MeasureKind MeasureKind { get; set; }
    public double Price { get; set; }
    public long Count { get; set; }
  }

  public class ClothKindRepository : Repository<ClothKind>
  {
    private static readonly BsonArray AggrStages = new BsonArray
    {
      new BsonDocument("$lookup",
        new BsonDocument
        {
          {"from", "orders"},
          {
            "let",
            new BsonDocument("kindid", "$_id")
          },
          {
            "pipeline",
            new BsonArray
            {
              new BsonDocument("$unwind",
                new BsonDocument("path", "$Instances")),
              new BsonDocument("$replaceRoot",
                new BsonDocument("newRoot", "$Instances")),
              new BsonDocument("$match",
                new BsonDocument("$expr",
                  new BsonDocument("$eq",
                    new BsonArray
                    {
                      "$ClothKind",
                      "$$kindid"
                    })))
            }
          },
          {"as", "ClothInstances"}
        }),
      new BsonDocument("$unwind",
        new BsonDocument
        {
          {"path", "$ClothInstances"},
          {"preserveNullAndEmptyArrays", true}
        }),
      new BsonDocument("$group",
        new BsonDocument
        {
          {"_id", "$_id"},
          {
            "Name",
            new BsonDocument("$first", "$Name")
          },
          {
            "MeasureKind",
            new BsonDocument("$first", "$MeasureKind")
          },
          {
            "Price",
            new BsonDocument("$first", "$Price")
          },
          {
            "Parent",
            new BsonDocument("$first", "$Parent")
          },
          {
            "Count",
            new BsonDocument("$sum", "$ClothInstances.Amount")
          },
          {
            "SumPrice",
            new BsonDocument("$sum",
              new BsonDocument("$multiply",
                new BsonArray
                {
                  "$Price",
                  "$ClothInstances.Amount"
                }))
          }
        }),
      new BsonDocument("$sort",
        new BsonDocument("_id", 1))
    };


    public ClothKindRepository(IModel model, IMongoCollection<ClothKind> collection) : base(model, collection)
    {
    }

    public override IReadOnlyList<ClothKind> Get(int offset, int limit, FilterDefinition<ClothKind> filter = null)
    {
      var filters =
        filter ?? Builders<ClothKind>.Filter.Empty;

      var clothKinds =
        this.Collection
          .Aggregate()
          .Match(Builders<ClothKind>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false))
          .AppendStage<BsonDocument>(AggrStages[0].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[1].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[2].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[3].AsBsonDocument)
          .As<ClothKind>()
          .Match(filters).ToList();

      foreach (var clothKind in clothKinds)
      {
        clothKind.ChildrenCount = GetChildrenCount(clothKind);
      }

      return clothKinds;
    }

    public ClothKind GetFullTree()
    {
      var root = this.GetById(0);
      FetchChildrenRecursively(root);
      return root;
    }

    private void FetchChildrenRecursively(ClothKind subroot)
    {
      FetchChildren(subroot);
      foreach (var subrootChild in subroot.Children)
      {
        FetchChildrenRecursively(subrootChild);
      }
    }

    public override ClothKind GetById(long id)
    {
      var clothKind = this.Collection
        .Aggregate()
        .Match(x => x.Id == id)
        .AppendStage<BsonDocument>(AggrStages[0].AsBsonDocument)
        .AppendStage<BsonDocument>(AggrStages[1].AsBsonDocument)
        .AppendStage<BsonDocument>(AggrStages[2].AsBsonDocument)
        .AppendStage<BsonDocument>(AggrStages[3].AsBsonDocument)
        .As<ClothKind>().First();

      clothKind.ChildrenCount = GetChildrenCount(clothKind);
      return clothKind;
    }

    public long GetChildrenCount(ClothKind clothKind)
    {
      return this.GetCount(Builders<ClothKind>.Filter.Eq(nameof(ClothKind.Parent), clothKind.Id));
    }

    public void FetchChildren(ClothKind clothKind)
    {
      clothKind.Children =
        this.Get(0, int.MaxValue, Builders<ClothKind>.Filter.Eq(nameof(ClothKind.Parent), clothKind.Id));
    }

    public override void Remove(ClothKind entity)
    {
      this.FetchChildren(entity);

      foreach (var child in entity.Children)
      {
        this.Remove(child);
      }

      base.Remove(entity);
    }

    public override long GetSearchStringCount(string searchString, FilterDefinition<ClothKind> filter)
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
            })));
      var filterdef = filter ?? Builders<ClothKind>.Filter.Empty;
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