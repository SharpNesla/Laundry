using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Laundry.Model.DatabaseClients;

namespace Laundry.Model
{
  public interface IModel
  {
    Employee CurrentUser { get; }

    Repository<Client> Clients { get; set; }
    EmployeeRepository Employees { get; set; }
    OrderRepository Orders { get; set; }
    Repository<Subsidiary> Subsidiaries { get; set; }

    void Connect(string username, string password);
    event Action ConnectionLost;
    event Action<Employee> Connected;
  }
}