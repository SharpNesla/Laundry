using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Laundry.Model.CollectionRepositories
{
  public class ClothInstancesRepository : Repository<ClothInstance>
  {
    public ClothInstancesRepository(IModel model, IMongoCollection<ClothInstance> collection) : base(model, collection)
    {
    }
  }
}
