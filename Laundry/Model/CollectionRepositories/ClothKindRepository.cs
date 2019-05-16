using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DatabaseClients;
using Laundry.Views;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Model.CollectionRepositories
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
            "Comment",
            new BsonDocument("$first", "$Comment")
          },
          {
            "WashPrice",
            new BsonDocument("$first", "$WashPrice")
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

    private static readonly BsonArray AggrStagesForChart = new BsonArray
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
              new BsonDocument("$addFields",
                new BsonDocument("Instances.ExecutionDate", "$ExecutionDate")),
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
        new BsonDocument("path", "$ClothInstances")),
      new BsonDocument("$project",
        new BsonDocument
        {
          {
            "SumPrice",
            new BsonDocument("$multiply",
              new BsonArray
              {
                "$Price",
                "$ClothInstances.Amount"
              })
          },
          {"MeasureKind", "$MeasureKind"},
          {"Price", "$Price"},
          {"Count", "$Count"},
          {"WashPrice", "$WashPrice"},
          {"Name", "$Name"},
          {"Amount", "$ClothInstances.Amount"},
          {
            "DateTime",
            new BsonDocument("$dateFromParts",
              new BsonDocument
              {
                {
                  "year",
                  new BsonDocument("$year", "$ClothInstances.ExecutionDate")
                },
                {
                  "day",
                  new BsonDocument("$dayOfMonth", "$ClothInstances.ExecutionDate")
                },
                {
                  "month",
                  new BsonDocument("$month", "$ClothInstances.ExecutionDate")
                }
              })
          }
        }),
      new BsonDocument("$group",
        new BsonDocument
        {
          {"_id", "$DateTime"},
          {
            "UnCountableCount",
            new BsonDocument("$sum",
              new BsonDocument("$cond",
                new BsonArray
                {
                  new BsonDocument("$eq",
                    new BsonArray
                    {
                      "$MeasureKind",
                      1
                    }),
                  "$Amount",
                  0
                }))
          },
          {
            "Price",
            new BsonDocument("$sum", "$SumPrice")
          },
          {
            "Count",
            new BsonDocument("$sum",
              new BsonDocument("$cond",
                new BsonArray
                {
                  new BsonDocument("$in",
                    new BsonArray
                    {
                      "$MeasureKind",
                      new BsonArray
                      {
                        0,
                        2
                      }
                    }),
                  "$Amount",
                  0
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

    public IReadOnlyList<AggregationResult> AggregateInstances(ChartTime time,
      FilterDefinition<ClothKind> filter = null)
    {
      var filters = Builders<ClothKind>.Filter.And(
        Builders<ClothKind>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false),
        filter ?? Builders<ClothKind>.Filter.Empty);

      var daysPart = time == ChartTime.Day
        ? "'day': {  '$dayOfMonth': '$ClothInstances.ExecutionDate'  },"
        : string.Empty;
      var monthPart = time == ChartTime.Day || time == ChartTime.Mounth
        ? "'month' : {'$month': '$ClothInstances.ExecutionDate'},"
        : string.Empty;
      var projectDef = $@"
{{
      SumPrice: {{$multiply:['$Price', '$ClothInstances.Amount']}}, 
      MeasureKind: '$MeasureKind',
      Price : '$Price',
      Count: '$Count',
      WashPrice: '$WashPrice',
      Name: '$Name',
      Amount: '$ClothInstances.Amount',
      DateTime: {{
        '$dateFromParts': {{
          {daysPart}
          {monthPart}
          'year': {{
            '$year': '$ClothInstances.ExecutionDate'
          }}        
        }}
      }}
}}";
      var readOnlyList = this.Collection
        .Aggregate()
        .AppendStage<BsonDocument>(AggrStages[0].AsBsonDocument)
        .AppendStage<BsonDocument>(AggrStages[1].AsBsonDocument)
        .AppendStage<BsonDocument>(AggrStages[2].AsBsonDocument)
        .AppendStage<BsonDocument>(AggrStages[3].AsBsonDocument)
        .As<ClothKind>()
        .Match(filters)
        .AppendStage<BsonDocument>(AggrStagesForChart[0].AsBsonDocument)
        .AppendStage<BsonDocument>(AggrStagesForChart[1].AsBsonDocument)
        .Project(projectDef)
        .AppendStage<BsonDocument>(AggrStagesForChart[3].AsBsonDocument)
        .AppendStage<BsonDocument>(AggrStagesForChart[4].AsBsonDocument)
        .As<AggregationResult>().ToList();
      return readOnlyList;
    }

    public string GetAggregatedInstacesCount(FilterDefinition<ClothKind> filter)
    {
      var filters =
        filter ?? Builders<ClothKind>.Filter.Empty;

      var groupDef = @"
{
  _id: '$MeasureKind',
  Count: {$sum: '$Amount'}
}";

      var aggregation =
        this.Collection
          .Aggregate()
          .Match(Builders<ClothKind>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false))
          .AppendStage<BsonDocument>(AggrStages[0].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[1].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[2].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[3].AsBsonDocument)
          .As<ClothKind>()
          .Match(filters)
          .Group(groupDef).ToList();


      int things = 0;
      int pairs = 0;
      int kgs = 0;
      try
      {
        things = aggregation[0]["Count"].AsInt32;
      }
      catch (ArgumentOutOfRangeException e)
      {
      }

      try
      {
        pairs = aggregation[2]["Count"].AsInt32;
      }
      catch (ArgumentOutOfRangeException e)
      {
      }

      try
      {
        kgs = aggregation[1]["Count"].AsInt32;
      }
      catch (ArgumentOutOfRangeException e)
      {
      }

      return
        $"{things + pairs}шт, {kgs}кг";
    }

    public double GetAggregatedPrice(FilterDefinition<ClothKind> filter)
    {
      var groupDef = @"{
  _id: 1,
  Price: {$sum: {$multiply: ['$Price', '$Count']}}
}";
      var filters =
        filter ?? Builders<ClothKind>.Filter.Empty;

      //В случае, если при агрегации в последовательности не оказалось элементов возвращаем ноль
      try
      {
        var doc = this.Collection
          .Aggregate()
          .Match(Builders<ClothKind>.Filter.Exists(nameof(IRepositoryElement.DeletionDate), false))
          .AppendStage<BsonDocument>(AggrStages[0].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[1].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[2].AsBsonDocument)
          .AppendStage<BsonDocument>(AggrStages[3].AsBsonDocument)
          .As<ClothKind>()
          .Match(filters)
          .Group(groupDef).ToList()[0]["Price"].AsDouble;
        return doc;
      }
      catch (ArgumentOutOfRangeException e)
      {
        return 0;
      }
    }
  }
}