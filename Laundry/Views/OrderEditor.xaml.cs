using System;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;
using Laundry.Utils.Controls.EntitySearchControls;

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
    public OrderEditorViewModel(IEventAggregator aggregator, IModel model,ClientSearchViewModel clientSearch,
      ClothEditorViewModel editor, ClothDataGridViewModel clothGrid, PaginatorViewModel paginator) 
      : base(aggregator, model, model.Orders, "заказа")
    {
      aggregator.Subscribe(this);
      this._clothKindEditor = editor;
      this.ClothInstancesGrid = clothGrid;

      this.Paginator = paginator;
      this.Paginator.ElementsName = "Вещей";
      this.Paginator.RegisterPaginable(this.ClothInstancesGrid);

      this.ClientCombo = clientSearch;
      this.ClientCombo.ClientChanged += OnClientChanged;
    }

    public async void AddOrder()
    {
      await DialogHostExtensions.ShowCaliburnVM(_clothKindEditor);
    }

    private void OnClientChanged(Client obj)
    {
      
    }
    
    public void Handle(Client message)
    {
      this.Entity = new Order();
      this.IsNew = true;
      this.Entity.Client = Model.Clients.GetById(message.Id);
    }
  }
}