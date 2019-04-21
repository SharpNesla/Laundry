using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Laundry.Model.CollectionRepositories;
using Laundry.Model.DatabaseClients;

namespace Laundry.Model
{
  public interface IModel
  {
    Employee CurrentUser { get; }

    ClientRepository Clients { get; }
    EmployeeRepository Employees { get; }
    OrderRepository Orders { get; }
    ClothInstancesRepository ClothInstances { get; }
    SubsidiaryRepository Subsidiaries { get; }
    CarRepository Cars { get; }
    ClothKindRepository ClothKinds { get; set; }

    void Connect(string username, string password);
    event Action ConnectionLost;
    event Action<Employee> Connected;
  }
}