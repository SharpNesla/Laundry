using MongoDB.Driver;
using Model;
using Model.DatabaseClients;

namespace Laundry.Utils.Controls.EntitySearchControls
{
  public class EmployeeSearchViewModel : EntitySearchBox<Employee, EmployeeRepository>
  {
    public EmployeeSearchViewModel(IModel model, string label="Работник", FilterDefinition<Employee> filter = null, bool isRequired = true)
      : base(model.Employees, label, filter, isRequired)
    {
    }
  }
}
