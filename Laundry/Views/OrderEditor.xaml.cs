using System;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for OrderEditor.xaml
  /// </summary>
  public class OrderEditorViewModel : ActivityScreen, IHandle<Order>, IHandle<Client>
  {
    private bool _isNewOrder;
    public Order Order { get; set; }
    public BindableCollection<ClothInstance> ClothInstances { get; set; }
    public ClientSearchViewModel ClientCombo { get; set; }
    public OrderEditorViewModel(IEventAggregator aggregator, IModel model,ClientSearchViewModel clientSearch , ClothEditor editor) : base(aggregator, model)
    {
      aggregator.Subscribe(this);
      this.ClientCombo = clientSearch;
      this.ClientCombo.ClientChanged += OnClientChanged;
    }

    private void OnClientChanged(Client obj)
    {
      this.Order.ClientId = obj.Id;
      this.Order.Client = Model.Clients.GetById(obj.Id);
    }

    public void Cancel()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }


    public void ApplyChanges()
    {
      if (_isNewOrder)
      {
        Model.Orders.Add(this.Order);
      }
      else
      {
        Model.Orders.Update(this.Order);
      }
      ChangeApplicationScreen(Screens.Context);
    }

    public void Handle(Order message)
    {
      this.Order = this.Model.Orders.GetById(message.Id);
      this._isNewOrder = false;
      this.ClientCombo.SelectedClient = this.Order.Client;
      this.EventAggregator.Unsubscribe(this);
    }

    public void Handle(Client message)
    {
      this.Order = new Order();
      this._isNewOrder = true;
      this.Order.ClientId = message.Id;
      this.Order.Client = Model.Clients.GetById(message.Id);
    }
  }
}