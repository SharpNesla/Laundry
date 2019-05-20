﻿using System;
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
    public CarRepository(IModel model, IMongoCollection<Car> collection) : base(model, collection)
    {
    }
  }
}