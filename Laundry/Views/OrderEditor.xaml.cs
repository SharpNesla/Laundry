using System;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for OrderEditor.xaml
  /// </summary>
  public class OrderEditorViewModel : ActivityScreen, IHandle<Order>
  {
    public Order Order { get; set; }
    public DateTime DateTime { get; set; }
    public BindableCollection<ClothInstance> ClothInstances { get; set; }

    public OrderEditorViewModel(IEventAggregator aggregator, IModel model, ClothEditor editor) : base(aggregator, model)
    {
      
    }

    public void Cancel()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public void Handle(Order message)
    {
      this.Order = this.Model.GetOrderById(message.Id);
      this.EventAggregator.Unsubscribe(this);
    }
  }
}