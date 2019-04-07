using System;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Utils;
using Laundry.Utils.Controls;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for OrderEditor.xaml
  /// </summary>
  public class OrderEditorViewModel : EditorScreen<OrderRepository, Order>, IHandle<Client>
  {
    public ClientSearchViewModel ClientCombo { get; set; }
    public OrderEditorViewModel(IEventAggregator aggregator, IModel model,ClientSearchViewModel clientSearch , ClothEditor editor) 
      : base(aggregator, model, model.Orders, "заказа")
    {
      aggregator.Subscribe(this);
      this.ClientCombo = clientSearch;
      this.ClientCombo.ClientChanged += OnClientChanged;
    }

    private void OnClientChanged(Client obj)
    {
      this.Entity.ClientId = obj.Id;
      this.Entity.Client = Model.Clients.GetById(obj.Id);
    }
    
    public void Handle(Client message)
    {
      this.Entity = new Order();
      this.IsNew = true;
      this.Entity.Client = Model.Clients.GetById(message.Id);
    }
  }
}