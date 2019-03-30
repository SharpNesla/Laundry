using System.Collections.Generic;
using System.Collections.ObjectModel;
using Laundry.Model.DatabaseClients;

namespace Laundry.Model
{
  public interface IModel
  {
    Employee CurrentUser { get;}

    CollectionActions<Client> Clients { get; set; }
    CollectionActions<Employee> Employees { get; set; }
    OrderCollectionActions Orders { get; set; }
    
  }
}