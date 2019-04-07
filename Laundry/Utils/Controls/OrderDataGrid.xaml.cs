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
  public class OrderDataGridViewModel : EntityGrid<Order, OrderRepository, OrderCardViewModel>
  {
    public Client Client { get; set; }
    public Employee Employee { get; set; }

    public override void Refresh(int page, int elements)
    {
      if (Client != null)
        this.Entities = Repo.GetForClient(Client, page * elements, elements);
      if (Employee != null)
      {
        this.Entities = Repo.GetForEmployee(Employee, page * elements, elements);
      }
      else
        base.Refresh(page, elements);
    }

    public OrderDataGridViewModel(IEventAggregator eventAggregator, OrderCardViewModel card, IModel model
    ) : base(eventAggregator, card, model.Orders, Screens.OrderEditor)
    {
    }
  }
}