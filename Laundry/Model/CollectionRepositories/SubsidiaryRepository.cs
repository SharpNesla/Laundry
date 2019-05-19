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
  }
}
