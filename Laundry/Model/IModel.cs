using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Model.CollectionRepositories;
using Model.DatabaseClients;

namespace Model
{
  /// <summary>
  /// Интерфейс модели
  /// </summary>
  public interface IModel
  {
    Employee CurrentUser { get; }

    ClientRepository Clients { get; }
    EmployeeRepository Employees { get; }
    OrderRepository Orders { get; }
    SubsidiaryRepository Subsidiaries { get; }
    CarRepository Cars { get; }
    ClothKindRepository ClothKinds { get; }
    DiscountSystemRepository DiscountEdges { get; }

    void Connect(string username, string password);
    event Action ConnectionLost;
    event Action<Employee> Connected;
  }
}