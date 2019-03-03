using System.Collections.ObjectModel;

namespace Laundry.Model
{
  public interface IModel
  {
    Employee CurrentUser { get;}
    ReadOnlyObservableCollection<Employee> Employees { get; }
    ReadOnlyObservableCollection<Client> Clients { get; }
    ReadOnlyObservableCollection<ClothKind> ClothKinds { get; }
    ReadOnlyObservableCollection<Car> Cars { get; }
    ReadOnlyObservableCollection<Subsidiary> Subsidiaries { get; }
  }
}