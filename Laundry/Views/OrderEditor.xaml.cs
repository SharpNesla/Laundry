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
    private readonly ClothEditorViewModel _clothKindEditor;
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

    public OrderEditorViewModel(IEventAggregator aggregator, IModel model,
      ClothEditorViewModel editor, ClothDataGridViewModel clothGrid, PaginatorViewModel paginator) 
      : base(aggregator, model, model.Orders, "заказа")
    {
      aggregator.Subscribe(this);
      this._clothKindEditor = editor;
      this.ClothInstancesGrid = clothGrid;

      this.Paginator = paginator;
      this.Paginator.ElementsName = "Вещей";
      this.Paginator.RegisterPaginable(this.ClothInstancesGrid);

      this.ClientCombo = new ClientSearchViewModel(model);
      this.ClientCombo.EntityChanged += OnEntityChanged;

      this.ObtainerCombo = new EmployeeSearchViewModel(model){Label="Приёмщик"};
      this.CorpObtainerCombo = new ClientSearchViewModel(model, "Приёмщик (корпоративный)");
      this.InCourierCombo = new EmployeeSearchViewModel(model) { Label= "Курьер, забирающий заказ" };
      this.WasherCombo = new EmployeeSearchViewModel(model) { Label= "Прачечник"};
      this.OutCourierCombo = new EmployeeSearchViewModel(model) { Label= "Курьер, вовзращающий заказ" };
      this.CorpDistributerCombo = new ClientSearchViewModel(model, "Приёмщик (корпоративный), принимающий заказ");
      this.DistributerCombo = new EmployeeSearchViewModel(model) { Label= "Приёмщик, выдающий заказ" };

      this.ObtainerCombo.EntityChanged += obtainer => this.EntityRepository.SetObtainer(this.Entity ,obtainer);
      this.CorpObtainerCombo.EntityChanged += corpObtainer => this.EntityRepository.SetObtainer(this.Entity, corpObtainer);
      this.InCourierCombo.EntityChanged += inCourier => this.EntityRepository.SetInCourier(this.Entity, inCourier);
      this.WasherCombo.EntityChanged += washer => this.EntityRepository.SetWasher(this.Entity, washer);
      this.OutCourierCombo.EntityChanged += outCourier => this.EntityRepository.SetOutCourier(this.Entity, outCourier);
      this.CorpDistributerCombo.EntityChanged += corpDistributer => this.EntityRepository.SetDistributer(this.Entity, corpDistributer);
      this.DistributerCombo.EntityChanged += distributer => this.EntityRepository.SetDistributer(this.Entity, distributer);
    }

    public async void AddOrder()
    {
      await DialogHostExtensions.ShowCaliburnVM(_clothKindEditor);
    }

    private void OnEntityChanged(Client obj)
    {
      this.EntityRepository.SetClient(this.Entity, obj);
    }
    public override void Handle(Order message)
    {
      base.Handle(message);
      this.ClientCombo.SelectedEntity = EntityRepository.GetClient(this.Entity);
    }
    public void Handle(Client message)
    {
      this.Entity = new Order{CreationDate = DateTime.Now};
      this.IsNew = true;
      this.ClientCombo.SelectedEntity = Model.Clients.GetById(message.Id);
    }
  }
}