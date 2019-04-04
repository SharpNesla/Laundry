using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Model.DatabaseClients;
using Laundry.Utils;
using Laundry.Utils.Controls;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClientEditor.xaml
  /// </summary>
  public class ClientEditorViewModel : EditorScreen<ClientRepository ,Client>
  {
    public bool IsOrdersEnabled
    {
      get { return !IsNew; }
    }

    #region TabBindings

    [AlsoNotifyFor(nameof(InfoVisibility))]
    public bool InfoChecked { get; set; }

    [AlsoNotifyFor(nameof(OrderGridVisibility))]
    public bool OrderChecked { get; set; }


    public Visibility InfoVisibility
    {
      get { return InfoChecked ? Visibility.Visible : Visibility.Collapsed; }
    }

    public Visibility OrderGridVisibility
    {
      get { return OrderChecked ? Visibility.Visible : Visibility.Collapsed; }
    }

    #endregion

    public OrderDataGridViewModel OrderDataGrid { get; set; }

    public ClientEditorViewModel(IEventAggregator aggregator, IModel model, OrderDataGridViewModel grid, PaginatorViewModel paginator) : base(
      aggregator, model, model.Clients)
    {
      InfoChecked = true;
      OrderDataGrid = grid;
      
      Paginator = paginator;
      paginator.ElementsName = "Заказов";
      Paginator.RegisterPaginable(OrderDataGrid);
    }

    private void RefreshOrders(int page, int elements)
    {
      var client = Client;
      if (client != null)
        OrderDataGrid.Entities = Model.Orders.GetForClient(client, page * elements, elements);
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      Paginator.Count = Model.Orders.GetForClientCount(Client);
      Client.OrdersCount = Model.Orders.GetForClientCount(Client);
      RefreshOrders(Paginator.CurrentPage - 1, Paginator.ElementsPerPage);
    }

    public PaginatorViewModel Paginator { get; set; }

    public void AddOrder(object sender, RoutedEventArgs e)
    {
      ChangeApplicationScreen(Screens.OrderEditor);
      Paginator.Count = Model.Orders.GetForClientCount(Client);
      RefreshOrders(Paginator.CurrentPage - 1, Paginator.ElementsPerPage);
      EventAggregator.BeginPublishOnUIThread(Client);
    }

    public void RemoveOrder(Order order)
    {
      Model.Orders.Remove(order);
      Client.OrdersCount = Model.Orders.GetForClientCount(Client);
      Paginator.Count = Model.Orders.GetForClientCount(Client);
      RefreshOrders(Paginator.CurrentPage - 1, Paginator.ElementsPerPage);
    }
    
    public override void Handle(Client client)
    {
      base.Handle(client);
      Paginator.Count = Model.Orders.GetForClientCount(Client);
      RefreshOrders(Paginator.CurrentPage - 1, Paginator.ElementsPerPage);
    }
  }
}