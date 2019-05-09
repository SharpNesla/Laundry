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
      new BsonDocument("$unwind",
        new BsonDocument("path", "$Instances")),
      new BsonDocument("$replaceRoot",
        new BsonDocument("newRoot", "$Instances")),
      new BsonDocument("$lookup",
        new BsonDocument
        {
          {"from", "clothkinds"},
          {"localField", "ClothKind"},
          {"foreignField", "_id"},
          {"as", "ClothKind"}
        }),
      new BsonDocument("$unwind",
        new BsonDocument("path", "$ClothKind")),
      new BsonDocument("$addFields",
        new BsonDocument("ClothKind.Amount", "$Amount")),
      new BsonDocument("$replaceRoot",
        new BsonDocument("newRoot", "$ClothKind")),
      new BsonDocument("$group",
        new BsonDocument
        {
          {"_id", "$_id"},
          {
            "Name",
            new BsonDocument("$first", "$Name")
          },
          {
            "Parent",
            new BsonDocument("$first", "$Parent")
          },
          {
            "MeasureKind",
            new BsonDocument("$first", "$MeasureKind")
          },
          {
            "SumPrice",
            new BsonDocument("$sum",
              new BsonDocument("$multiply",
                new BsonArray
                {
                  "$Price",
                  "$Amount"
                }))
          },
          {
            "Count",
            new BsonDocument("$sum", "$Amount")
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
      var clothKindsFirst =
        this.Collection
          .Database
          .GetCollection<BsonDocument>("orders")
          .Aggregate()
          .AppendStage<BsonDocument>(AggrStages[0].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[1].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[2].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[3].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[4].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[5].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[6].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[7].AsBsonDocument)
          .As<ClothKind>()
          .Match(filters).ToList();

      var second = this.Collection
        .Aggregate()
        .Match(Builders<ClothKind>.Filter.And(
          Builders<ClothKind>.Filter.Nin(nameof(ClothKind.Id), clothKindsFirst.ToList().Select(x => x.Id)), filters))
        .ToList();

      var clothKinds = clothKindsFirst.Concat(second).Skip(offset).Take(limit).ToList();

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
      var clothKind = base.GetById(id);

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