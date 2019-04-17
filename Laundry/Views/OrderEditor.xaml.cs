using System;
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
  /// Interaction logic for OrderEditor.xaml
  /// </summary>
  public class OrderEditorViewModel : EditorScreen<OrderRepository, Order>, IHandle<Client>
  {
    public ClientSearchViewModel ClientCombo { get; set; }
    public ClothDataGridViewModel ClothInstancesGrid { get; set; }
    public PaginatorViewModel Paginator { get; set; }

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
      this.ClothInstancesGrid = clothGrid;

      this.Paginator = paginator;
      this.Paginator.ElementsName = "Вещей";
      this.Paginator.RegisterPaginable(this.ClothInstancesGrid);

      this.ClientCombo = new ClientSearchViewModel(model);
      this.ClientCombo.EntityChanged += OnEntityChanged;

      #region Инициализация поисковых комбобоксов

      this.ObtainerCombo = new EmployeeSearchViewModel(model, "Приёмщик",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Advisor));
      this.CorpObtainerCombo = new ClientSearchViewModel(model, "Приёмщик (корпоративный)",
        Builders<Client>.Filter.Eq(nameof(Client.IsCorporative), true));
      this.InCourierCombo = new EmployeeSearchViewModel(model, "Курьер, забирающий заказ",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Courier));
      this.WasherCombo = new EmployeeSearchViewModel(model, "Прачечник",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Washer));
      this.OutCourierCombo = new EmployeeSearchViewModel(model, "Курьер, вовзращающий заказ",
        Builders<Employee>.Filter.Eq(nameof(Employee.Profession), EmployeeProfession.Courier));
      this.CorpDistributerCombo = new ClientSearchViewModel(model, "Приёмщик (корпоративный), принимающий заказ",
        Builders<Client>.Filter.Eq(nameof(Client.IsCorporative), true));
      this.DistributerCombo = new EmployeeSearchViewModel(model, "Приёмщик, выдающий заказ",
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

    public async void AddCloth()
    {
      var clothInstanceEditor = new ClothEditorViewModel(this.EventAggregator, this.Model);
      clothInstanceEditor.Order = this.Entity;
      await DialogHostExtensions.ShowCaliburnVM(clothInstanceEditor);
    }

    private void OnEntityChanged(Client obj)
    {
      this.EntityRepository.SetClient(this.Entity, obj);
    }

    public override void Handle(Order message)
    {
      base.Handle(message);
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
      this.Entity = new Order {CreationDate = DateTime.Now};
      this.IsNew = true;
      this.ClientCombo.SelectedEntity = Model.Clients.GetById(message.Id);
    }
  }
}