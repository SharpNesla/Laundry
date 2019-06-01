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
  /// �������� ��������
  /// </summary>
  public class SubsidiaryEditorViewModel : EditorScreen<Repository<Subsidiary>, Subsidiary>
  {
    /// <summary>
    /// ����, ����������� ����� �������� ��������
    /// ��� �������� ������ �������
    /// </summary>
    public bool MainAdvisorSearchEnabled => !IsNew;

    public EmployeeSearchViewModel MainAdvisorSearch { get; }
    public EmployeeDataGridViewModel Advisors { get; set; }

    public SubsidiaryEditorViewModel(IEventAggregator aggregator, EmployeeDataGridViewModel advisorsGrid, IModel model,
      string entityTitleName = "�������")
      : base(aggregator, model, model.Subsidiaries, entityTitleName)
    {
      this.Advisors = advisorsGrid;

      this.Advisors.DisplaySelectionColumn = false;
      this.Advisors.IsCompact = true;

      this.MainAdvisorSearch = new EmployeeSearchViewModel(model, "������� �������",
        Builders<Employee>.Filter.And(
          Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), this.Entity.Id),
          Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
        ), false);
    }

    /// <summary>
    /// ��� ��������� ������� �� �������������� ������������
    /// ��������� �������� � ���������� ������� ��������� �������
    /// </summary>
    /// <param name="message">������</param>
    public override void Handle(Subsidiary message)
    {
      base.Handle(message);

      MainAdvisorSearch.Filter = Builders<Employee>.Filter.And(
        Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), this.Entity.Id),
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
      );

      Advisors.Filter = Builders<Employee>.Filter.And(
        Builders<Employee>.Filter.Eq(nameof(Employee.Subsidiary), this.Entity.Id),
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor)
      );

      Advisors.Refresh(0, int.MaxValue);
    }
  }
}