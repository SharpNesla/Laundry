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
  public class CarRepository : Repository<Car>
  {
    //В базовый конструктор передаются критерии поиска автомобиля
    public CarRepository(IModel model, IMongoCollection<Car> collection)
      : base(model, collection,
        new[]
        {
          nameof(Car.BrandAndModel), nameof(Car.VIN),
          nameof(Car.BodyID), nameof(Car.Color), nameof(Car.Sign)
        })
    {
    }
  }
}