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
    public bool MainAdvisorSearchEnabled => !IsNew;

    public EmployeeSearchViewModel MainAdvisorSearch { get; }
    public EmployeeDataGridViewModel Advisors { get; set; }

    public SubsidiaryEditorViewModel(IEventAggregator aggregator, EmployeeDataGridViewModel advisorsGrid, IModel model,
      string entityTitleName = "филиала")
      : base(aggregator, model, model.Subsidiaries, entityTitleName)
    {
      this.Advisors = advisorsGrid;
      Advisors.Filter = Builders<Employee>.Filter.And(
        Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), this.Entity.Id),
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
      );
      this.Advisors.DisplaySelectionColumn = false;
      this.Advisors.IsCompact = true;

      

      this.MainAdvisorSearch = new EmployeeSearchViewModel(model, "Главный приёмщик",
        Builders<Employee>.Filter.And(
          Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), this.Entity.Id),
          Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
        ));
      if (!IsNew)
      {
        this.Advisors.Refresh(0, int.MaxValue);
      }
    }
  }
}