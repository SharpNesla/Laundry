using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils.Controls.EntitySearchControls;
using Laundry.Views;
using MaterialDesignThemes.Wpf;
using MongoDB.Driver;
using NPOI.XSSF.UserModel;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  public class OrderDataGridViewModel : EntityGrid<Order, OrderRepository, OrderCardViewModel>
  {
    private readonly IEventAggregator _eventAggregator;
    private EmployeeProfession _profession;

    public override FilterDefinition<Order> Filter
    {
      get
      {
        var filter = BaseFilter;

        if (this.IsByCreationDate)
        {
          filter = Builders<Order>.Filter.And(
            filter,
            Builders<Order>.Filter.Gte(nameof(Order.CreationDate), this.LowCreationDateBound ?? DateTime.MinValue),
            Builders<Order>.Filter.Lte(nameof(Order.CreationDate), this.HighCreationDateBound ?? DateTime.MaxValue));
        }

        if (this.IsByExecutionDate)
        {
          filter = Builders<Order>.Filter.And(
            filter,
            Builders<Order>.Filter.Gte(nameof(Order.ExecutionDate), this.LowExecutionDateBound ?? DateTime.MinValue),
            Builders<Order>.Filter.Lte(nameof(Order.ExecutionDate), this.HighExecutionDateBound ?? DateTime.MaxValue));
        }

        if (this.IsByClient)
        {
          filter = Builders<Order>.Filter.And(
            filter,
            Builders<Order>.Filter.Eq(nameof(Order.ClientId), this.ClientCombo.SelectedEntity?.Id ?? -1));
        }

        if (this.IsByEmployee)
        {
          filter = Builders<Order>.Filter.And(
            filter,
            Builders<Order>.Filter.Or(
              Builders<Order>.Filter.Eq(nameof(Order.ObtainerId), this.EmployeeCombo.SelectedEntity?.Id ?? -1),
              Builders<Order>.Filter.Eq(nameof(Order.InCourierId), this.EmployeeCombo.SelectedEntity?.Id ?? -1),
              Builders<Order>.Filter.Eq(nameof(Order.WasherCourierId), this.EmployeeCombo.SelectedEntity?.Id ?? -1),
              Builders<Order>.Filter.Eq(nameof(Order.OutCourierId), this.EmployeeCombo.SelectedEntity?.Id ?? -1),
              Builders<Order>.Filter.Eq(nameof(Order.DistributerId), this.EmployeeCombo.SelectedEntity?.Id ?? -1)
            ));
        }

        return filter;
      }

      set { base.Filter = value; }
    }

    #region Св-ва, описывающие поисковые параметры грида

    public bool IsByCreationDate { get; set; }
    public DateTime? LowCreationDateBound { get; set; }
    public DateTime? HighCreationDateBound { get; set; }

    public bool IsByExecutionDate { get; set; }
    public DateTime? LowExecutionDateBound { get; set; }
    public DateTime? HighExecutionDateBound { get; set; }

    public bool IsByClient { get; set; }
    public bool IsCorporative { get; set; }
    public ClientSearchViewModel ClientCombo { get; set; }

    public bool IsByEmployee { get; set; }

    public EmployeeProfession Profession
    {
      get { return _profession; }
      set
      {
        _profession = value;
        EmployeeCombo.Filter = Builders<Employee>.Filter.Eq(nameof(Employee.Profession), value);
      }
    }
    public EmployeeSearchViewModel EmployeeCombo { get; }

    public bool IsBySubsidiary { get; set; }
    public SubsidiarySearchViewModel SubsidiaryCombo { get; set; }

    #endregion

    public Client Client { get; set; }
    public Employee Employee { get; set; }

    public override void Refresh(int page, int elements)
    {
      if (Client != null)
      {
        this.Entities = Repo.GetForClient(Client, page * elements, elements);
        return;
      }

      if (Employee != null)
      {
        this.Entities = Repo.GetForEmployee(Employee, page * elements, elements);
      }
      else
        base.Refresh(page, elements);
    }

    protected override XSSFWorkbook PrepareWorkBook(XSSFWorkbook workbook)
    {
      throw new NotImplementedException();
    }

    public override long Count
    {
      get
      {
        if (Client != null)
          return Repo.GetForClientCount(Client);
        if (Employee != null)
        {
          return Repo.GetForEmployeeCount(Employee);
        }

        return base.Count;
      }
    }

    public OrderDataGridViewModel(IEventAggregator eventAggregator, OrderCardViewModel card,
      DeleteDialogViewModel deleteDialog, IModel model
    ) : base(eventAggregator, card, model.Orders, deleteDialog, Screens.OrderEditor)
    {
      _eventAggregator = eventAggregator;
      eventAggregator.Subscribe(this);

      this.ClientCombo = new ClientSearchViewModel(model);

      this.SubsidiaryCombo = new SubsidiarySearchViewModel(model);

      this.EmployeeCombo = new EmployeeSearchViewModel(model) {Label = "Работник"};

      this.Profession = EmployeeProfession.Courier;

    }

    //public void Handle(Client message)
    //{
    //  this.IsSearchDrawerOpened = true;
    //  this.IsByClient = true;
    //  this.ClientCombo.SelectedEntity = message;
    //  this.EventAggregator.Unsubscribe(this);
    //}
  }
}