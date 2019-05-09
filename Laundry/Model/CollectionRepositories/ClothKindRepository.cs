using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laundry.Model.DatabaseClients;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laundry.Model.CollectionRepositories
{
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
      var filters = Builders<ClothKind>.Filter.And(
        filter ?? Builders<ClothKind>.Filter.Empty,
        Builders<ClothKind>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false)
      );
      var clothKinds =
        this.Collection
          .Aggregate()
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
          .Match(x=>x.Id == id)
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
  }
}