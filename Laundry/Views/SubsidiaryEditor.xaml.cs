using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using Laundry.Utils.Controls.EntitySearchControls;
using MongoDB.Driver;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for SubsidiaryEditor.xaml
  /// </summary>
  public class SubsidiaryEditorViewModel : EditorScreen<Repository<Subsidiary>, Subsidiary>
  {
    public EmployeeSearchViewModel MainAdvisor { get; }
    public EmployeeSearchViewModel AdvisorSearch { get; }

    public EmployeeDataGridViewModel AdvisorGrid { get; }

    public SubsidiaryEditorViewModel(IEventAggregator aggregator, EmployeeDataGridViewModel employeeGrid, IModel model,
      string entityTitleName = "Филиала") : base(aggregator, model, model.Subsidiaries, entityTitleName)
    {
      AdvisorSearch = new EmployeeSearchViewModel(model, "Добавляемый приёмщик",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor));

      MainAdvisor = new EmployeeSearchViewModel(model, "Добавляемый приёмщик",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor));

      AdvisorGrid = employeeGrid;
    }

    public override void Handle(Subsidiary message)
    {
      AdvisorGrid.Filter = Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), message.Id);
    }
  }
}