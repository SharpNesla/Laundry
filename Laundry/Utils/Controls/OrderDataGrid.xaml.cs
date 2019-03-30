using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Views;
using MaterialDesignThemes.Wpf;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>
  
  public class OrderDataGridViewModel : PropertyChangedBase
  {
    private OrderCard _card;
    private IEventAggregator _eventAggregator;
    private IModel _model;
    public event Action<Order> OrderInfoClicked;
    public Client SpecifiedClient { get; set; }
    public IList<Order> Orders { get; set; }
    public Order SelectedOrder { get; set; }

    public event Action<Order> RemoveOrderButtonClick;

    public OrderDataGridViewModel(OrderCard card, IEventAggregator eventAggregator, IModel model)
    {
      this._card = card;
      this._eventAggregator = eventAggregator;
      this._model = model;
    }

    public void ShowClientInfo(Order context)
    {
      _card.Order = context;
      DialogHost.Show(_card);
    }

    public void EditOrder()
    {
      _eventAggregator.PublishOnUIThread(Screens.OrderEditor);
      _eventAggregator.PublishOnUIThread(this.SelectedOrder);
    }

    public void RemoveOrder()
    {
      RemoveOrderButtonClick?.Invoke(SelectedOrder);
    }
  }
}
