using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Model.CollectionRepositories
{
  public class SubsidiaryRepository : Repository<Subsidiary>
  {
    public SubsidiaryRepository(IModel model, IMongoCollection<Subsidiary> collection) : base(model, collection)
    {
    }

    /// <summary>
    /// Метод, получающий данные по аналитике филиалов
    /// </summary>
    /// <returns>Ридонли лист с результатами для каждого филиала</returns>
    public IReadOnlyList<SubsidiaryAggregationResult> AggregateSubsidiaries(FilterDefinition<Order> filter = null)
    {
      var projectDef = @"{
  Price: {$sum: '$Orders.Price'},
  Orders : '$Orders',
  Signature: {$concat :
  [
    {$toString:'$_id'}, ' ',
   '$Name', ' ',
   '$Street', ' ', '$House'
  ]}
}";

      var groupDef = @"
{
  _id: '$_id',
  Signature: {$first : '$Signature'},
  Price: {$first :'$Price'},
  Count : { 
    $sum : { 
      $cond:[
          {$in: ['$ClothKind.MeasureKind', [0,2]]},
          '$Orders.Instances.Amount', 
          0]
      }
  },
    UnCountableCount : { 
    $sum : { 
      $cond:[
          {$eq: ['$ClothKind.MeasureKind', 1]},
          '$Orders.Instances.Amount', 
          0]
      }
  },
}";
      //Вложенный pipeline для pipeline-let lookup-а заказов в аналитике филиалов
      var pipeline = PipelineDefinition<Order, Order>.Create(new[]
        {
          PipelineStageDefinitionBuilder.Match<Order>(new BsonDocument("$expr",
            new BsonDocument("$or",
              new BsonArray
              {
                new BsonDocument("$eq",
                  new BsonArray
                  {
                    "$InSubsidiary",
                    "$$kindid"
                  }),
                new BsonDocument("$eq",
                  new BsonArray
                  {
                    "$OutSubsidiary",
                    "$$kindid"
                  })
              }))),
          PipelineStageDefinitionBuilder.Match(filter ?? Builders<Order>.Filter.Empty)
        }
      );

      var aggregateUnwindOptions = new AggregateUnwindOptions<BsonDocument> {PreserveNullAndEmptyArrays = true};
      var aggregation =
        this.GetAggregationFluent()
          .Lookup<Order, Order, List<Order>, Subsidiary>(
            this.Collection.Database.GetCollection<Order>("orders"), new BsonDocument("kindid", "$_id"), pipeline,
            "Orders")
          .Project(projectDef)
          .Unwind("Orders", aggregateUnwindOptions)
          .Unwind("Orders.Instances", aggregateUnwindOptions)
          .Lookup("clothkinds", "Orders.Instances.ClothKind", "_id", "ClothKind")
          .Unwind("ClothKind", aggregateUnwindOptions)
          .Group(groupDef).As<SubsidiaryAggregationResult>().SortBy(x => x.SubsidiaryId).ToList();
      return aggregation;
    }
  }
}