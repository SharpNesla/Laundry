using System;
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

    public ReadOnlyObservableCollection<Client> Clients => new ReadOnlyObservableCollection<Client>(_clients);

    public ReadOnlyObservableCollection<ClothKind> ClothKinds { get; private set; }
    public ReadOnlyObservableCollection<Car> Cars { get; private set; }
    public ReadOnlyObservableCollection<Subsidiary> Subsidiaries { get; private set; }

    private readonly ObservableCollection<Client> _clients;

    public MockModel()
    {
      this.CurrentUser = new Employee("F", "f", "D"){Profession = EmployeeProfession.Courier};

      this._clients = new ObservableCollection<Client>(
        new[]
        {
          new Client("Антрипотийединиколей", "Карлов", "Иванович"),
          new Client("Андрей", "Rjrjh", "Иванович"),
          new Client("Андрей", "Карлов", "Иванович"),
          new Client("Андрей", "Карлов", "Иванович"),
          new Client("Андрей", "Карлов", "Иванович"),
        }
      );
      
    }
  }
}