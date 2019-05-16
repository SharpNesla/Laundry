using System;
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
  /// Interaction logic for OrderEditor.xaml
  /// </summary>
  public class OrderEditorViewModel : EditorScreen<OrderRepository, Order>, IHandle<Client>
  {
    public override string EditorTitle
    {
      get
      {
        return !IsNew
          ? $"Редактирование {EntityName} №{Entity.Id} от {Entity.CreationDate:dd.MM.yyyy, HH:mm}"
          : $"Редактирование нового {EntityName} от от {Entity.CreationDate:dd.MM.yyyy, HH:mm}";
      }
    }

    public ClientSearchViewModel ClientCombo { get; set; }
    public ClothDataGridViewModel ClothInstancesGrid { get; set; }
    public PaginatorViewModel Paginator { get; set; }

    public DiscountEdge Edge { get; private set; }

    public EmployeeSearchViewModel ObtainerCombo { get; set; }
    public ClientSearchViewModel CorpObtainerCombo { get; set; }
    public EmployeeSearchViewModel InCourierCombo { get; set; }
    public EmployeeSearchViewModel WasherCombo { get; set; }
    public EmployeeSearchViewModel OutCourierCombo { get; set; }
    public EmployeeSearchViewModel DistributerCombo { get; set; }
    public ClientSearchViewModel CorpDistributerCombo { get; set; }

    public OrderEditorViewModel(IEventAggregator aggregator, IModel model, ClothDataGridViewModel clothGrid,
      PaginatorViewModel paginator)
      : base(aggregator, model, model.Orders, "заказа")
    {
      aggregator.Subscribe(this);

      this.Entity.CreationDate = DateTime.Now;
      this.Entity.ExecutionDate = DateTime.Now.AddDays(1);

      this.ClothInstancesGrid = clothGrid;
      this.ClothInstancesGrid.Order = this.Entity;

      this.Paginator = paginator;
      this.Paginator.ElementsName = "Вещей";
      this.Paginator.RegisterPaginable(this.ClothInstancesGrid);

      this.ClientCombo = new ClientSearchViewModel(model);
      this.ClientCombo.EntityChanged += OnEntityChanged;
    }

    protected override void OnActivate()
    {
      base.OnActivate();

      #region Инициализация поисковых комбобоксов

      this.ObtainerCombo = new EmployeeSearchViewModel(Model, "Приёмщик",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor));
      this.CorpObtainerCombo = new ClientSearchViewModel(Model, "Приёмщик (корпоративный)",
        Builders<Client>.Filter.Eq(nameof(Client.IsCorporative), true));
      this.InCourierCombo = new EmployeeSearchViewModel(Model, "Курьер, забирающий заказ",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Courier));
      this.WasherCombo = new EmployeeSearchViewModel(Model, "Прачечник",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Washer));
      this.OutCourierCombo = new EmployeeSearchViewModel(Model, "Курьер, вовзращающий заказ",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Courier));
      this.CorpDistributerCombo = new ClientSearchViewModel(Model, "Приёмщик (корпоративный), принимающий заказ",
        Builders<Client>.Filter.Eq(nameof(Client.IsCorporative), true));
      this.DistributerCombo = new EmployeeSearchViewModel(Model, "Приёмщик, выдающий заказ",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor));

      this.ObtainerCombo.EntityChanged += obtainer => this.EntityRepository.SetObtainer(this.Entity, obtainer);
      this.CorpObtainerCombo.EntityChanged +=
        corpObtainer => this.EntityRepository.SetObtainer(this.Entity, corpObtainer);
      this.InCourierCombo.EntityChanged += inCourier => this.EntityRepository.SetInCourier(this.Entity, inCourier);
      this.WasherCombo.EntityChanged += washer => this.EntityRepository.SetWasher(this.Entity, washer);
      this.OutCourierCombo.EntityChanged += outCourier => this.EntityRepository.SetOutCourier(this.Entity, outCourier);
      this.CorpDistributerCombo.EntityChanged += corpDistributer =>
        this.EntityRepository.SetDistributer(this.Entity, corpDistributer);
      this.DistributerCombo.EntityChanged +=
        distributer => this.EntityRepository.SetDistributer(this.Entity, distributer);

      #endregion
    }

    public void AddCloth()
    {
      this.ClothInstancesGrid.Add();
    }

    private void OnEntityChanged(Client obj)
    {
      this.EntityRepository.SetClient(this.Entity, obj);
    }


    public override void Handle(Order message)
    {
      base.Handle(message);

      this.Entity = this.EntityRepository.GetById(message.Id);


      this.ClothInstancesGrid.Order = this.Entity;
      this.Paginator.RefreshPaginable();

      this.ClientCombo.SelectedEntity = EntityRepository.GetClient(this.Entity);
      if (message.IsCorporative)
      {
        CorpObtainerCombo.SelectedEntity = Model.Clients.GetById(message.CorpObtainerId);
        CorpDistributerCombo.SelectedEntity = Model.Clients.GetById(message.CorpDistributerId);
      }
      else
      {
        ObtainerCombo.SelectedEntity = Model.Employees.GetById(message.ObtainerId);
        DistributerCombo.SelectedEntity = Model.Employees.GetById(message.DistributerId);
      }

      InCourierCombo.SelectedEntity = Model.Employees.GetById(message.InCourierId);

      WasherCombo.SelectedEntity = Model.Employees.GetById(message.WasherCourierId);

      OutCourierCombo.SelectedEntity = Model.Employees.GetById(message.OutCourierId);
    }

    public void Handle(Client message)
    {
      this.ClientCombo.SelectedEntity = Model.Clients.GetById(message.Id);
    }
  }
}