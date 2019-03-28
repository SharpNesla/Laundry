using System;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

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

    public OrderEditorViewModel(IEventAggregator aggregator, IModel model, ClothEditor editor) : base(aggregator, model)
    {
      aggregator.Subscribe(this);
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
        Model.AddOrder(this.Order);
      }
      else
      {
        Model.UpdateOrder(this.Order);
      }
      ChangeApplicationScreen(Screens.Context);
    }

    public void Handle(Order message)
    {
      this.Order = this.Model.GetOrderById(message.Id);
      this._isNewOrder = false;
      this.EventAggregator.Unsubscribe(this);
    }

    public void Handle(Client message)
    {
      this.Order = new Order();
      this._isNewOrder = true;
      this.Order.ClientId = message.Id;
      this.Order.Client = Model.GetClientById(message.Id);
    }
  }
}