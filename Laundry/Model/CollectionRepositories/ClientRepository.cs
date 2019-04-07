using System.Collections.Generic;
using Laundry.Model.CollectionRepositories;
using MongoDB.Driver;

namespace Laundry.Model.DatabaseClients
{
  public class ClientRepository : Repository<Client>
  {
    public ClientRepository(IModel model, IMongoCollection<Client> collection) : base(model, collection)
    {
    }
  }
}