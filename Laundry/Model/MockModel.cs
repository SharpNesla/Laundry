﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laundry.Model
{
  public class MockModel : IModel
  {
    public Employee CurrentUser { get; private set; }
    public ReadOnlyObservableCollection<Employee> Employees { get; private set; }
    public ReadOnlyObservableCollection<Client> Clients { get; private set; }
    public ReadOnlyObservableCollection<ClothKind> ClothKinds { get; private set; }
    public ReadOnlyObservableCollection<Car> Cars { get; private set; }
    public ReadOnlyObservableCollection<Subsidiary> Subsidiaries { get; private set; }

    public MockModel()
    {
      this.CurrentUser = new Employee("F", "f", "D"){Profession = EmployeeProfession.Courier};
    }
  }
}