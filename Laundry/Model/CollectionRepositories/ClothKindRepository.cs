using System;
using System.Collections.Generic;
using Model.DatabaseClients;
using Laundry.Views;
using MongoDB.Bson;
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

    protected override IAggregateFluent<ClothKind> GetAggregationFluent(bool includeDeleted = false,
      FilterDefinition<ClothKind> filter = null)
    {
      return this.GetAggregationFluentForAggregation();
    }

    private IAggregateFluent<ClothKind> GetAggregationFluentForAggregation(bool includeDeleted = false,
      FilterDefinition<ClothKind> filter = null, FilterDefinition<Order> filterOrder = null)
    {
      var instancesProjectDef = @"{
  ClothKind : '$Instances.ClothKind',
  Amount: '$Instances.Amount'
}";

      PipelineDefinition<Order, BsonDocument> innerpipeline = PipelineDefinition<Order, BsonDocument>
        .Create(new IPipelineStageDefinition[]
        {
          PipelineStageDefinitionBuilder.Match(filterOrder ?? Builders<Order>.Filter.Empty),
          PipelineStageDefinitionBuilder.Unwind<Order>("Instances"),
          PipelineStageDefinitionBuilder.Project<BsonDocument>(instancesProjectDef),
          PipelineStageDefinitionBuilder.Match<BsonDocument>(new BsonDocument("$expr",
            new BsonDocument("$eq",
              new BsonArray
              {
                "$ClothKind",
                "$$kindid"
              }))),
        });

      var groupDef = @"{
  _id: ""$_id"",
  Name :{ $first: ""$Name""},
  MeasureKind: { $first:""$MeasureKind""},
  Price :{ $first: ""$Price""},
  Parent:{$first:""$Parent""},
  Count: {
    $sum:""$ClothInstances.Amount""
  },
  SumPrice:{
    $sum: {$multiply : [""$Price"", ""$ClothInstances.Amount""]}
  }
}";

      var projectDef = @"{
  Name: '$Name',
  MeasureKind: '$MeasureKind',
  Price : '$Price',
  Parent : '$Parent',
  Count : '$Count',
  SumPrice: '$SumPrice',
  ChildrenCount : {$size: '$Children'} 
}";

      var aggregateUnwindOptions = new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true };


      return base.GetAggregationFluent(includeDeleted, filter)
        .Lookup<Order, BsonDocument, IList<BsonDocument>, ClothKind>(
          this.Collection.Database.GetCollection<Order>("orders"),
          new BsonDocument("kindid", "$_id"), innerpipeline, "ClothInstances")
        .Unwind("ClothInstances", aggregateUnwindOptions)
        .Group(groupDef)
        .Lookup("clothkinds", "_id", "Parent", "Children")
        .Project(projectDef)
        .As<ClothKind>()
        .SortBy(x => x.Id);
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
    
    public IReadOnlyList<AggregationResult> AggregateInstances(ChartTime time,
      FilterDefinition<ClothKind> filter, FilterDefinition<Order> orderDateFilter)
    {
      var filters = Builders<ClothKind>.Filter.And(
        Builders<ClothKind>.Filter.Exists(nameof(RepositoryElement.DeletionDate), false),
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
      var readOnlyList = this.GetAggregationFluentForAggregation(false, filter, orderDateFilter)
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
  Count: {$sum: '$Count'}
}";

      var aggregation = this.GetAggregationFluent()
          .Match(filters)
          .Group(groupDef).ToList();


      int things = 0;
      int pairs = 0;
      int kgs = 0;
      try
      {
        things = aggregation[0]["Count"].AsInt32;
      }
      catch (ArgumentOutOfRangeException)
      {
      }

      try
      {
        pairs = aggregation[2]["Count"].AsInt32;
      }
      catch (ArgumentOutOfRangeException)
      {
      }

      try
      {
        kgs = aggregation[1]["Count"].AsInt32;
      }
      catch (ArgumentOutOfRangeException)
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
        var doc = this.GetAggregationFluent()
          .Match(filters)
          .Group(groupDef).ToList()[0]["Price"].AsDouble;
        return doc;
      }
      catch (ArgumentOutOfRangeException)
      {
        return 0;
      }
    }
  }
}