using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Laundry.Model.DatabaseClients;

namespace Laundry.Model
{
  public interface IModel
  {
    Employee CurrentUser { get; }

    CollectionActions<Client> Clients { get; set; }
    EmployeeCollectionActions Employees { get; set; }
    OrderCollectionActions Orders { get; set; }

    void Connect(string username, string password);
    event Action ConnectionLost;
  }
}