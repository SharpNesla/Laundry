using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.CollectionRepositories;
using Laundry.Views;
using MaterialDesignThemes.Wpf;

namespace Laundry.Utils.Controls
{
  /// <summary>
  /// Interaction logic for OrderDataGrid.xaml
  /// </summary>

  public class OrderDataGridViewModel : EntityGrid<Order, OrderRepository>
  {
    public Client Client { get; set; }
    public OrderDataGridViewModel(IEventAggregator eventAggregator,IModel model, OrderCardViewModel card) 
      : base(eventAggregator, card, model.Orders, Screens.OrderEditor)
    {
    }

    public override void Refresh(int page, int elements)
    {
      
      if (Client==null)
      {
        Refresh(page, elements);
      }
      else
      {
        Repo.GetForClient(Client ,page * elements, elements);
      }
    }
  }
}
