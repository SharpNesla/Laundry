using Caliburn.Micro;
using Model;
using Model.CollectionRepositories;
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

      this.Advisors.DisplaySelectionColumn = false;
      this.Advisors.IsCompact = true;

      this.MainAdvisorSearch = new EmployeeSearchViewModel(model, "Главный приёмщик",
        Builders<Employee>.Filter.And(
          Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), this.Entity.Id),
          Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
        ),false);
    }

    
    public override void Handle(Subsidiary message)
    {
      base.Handle(message);
      this.Advisors.Refresh(0, int.MaxValue);

      MainAdvisorSearch.Filter = Builders<Employee>.Filter.And(
        Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), this.Entity.Id),
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
      );

      Advisors.Filter = Builders<Employee>.Filter.And(
        Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), this.Entity.Id),
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
      );
    }
  }
}