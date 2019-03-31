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
using Laundry.Utils;
using Laundry.Utils.Controls;
using PropertyChanged;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClientEditor.xaml
  /// </summary>
  public class ClientEditorViewModel : ActivityScreen, IHandle<Client>
  {
    
    public Client Client { get; set; }

    [AlsoNotifyFor(nameof(IsOrdersEnabled), nameof(EditorTitle))]
    public bool IsNew { get; set; }
    public bool IsOrdersEnabled
    {
      get { return !IsNew; }
    }

    public string EditorTitle
    {
      get { return !IsNew ? $"Редактирование клиента №{Client.Id}" : "Редактирование нового клиента"; }
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
      aggregator, model)
    {
      this.EventAggregator.Subscribe(this);
      this.InfoChecked = true;
      this.OrderDataGrid = grid;

      this.IsNew = true;
      this.Client = new Client();

      this.Paginator = paginator;
      paginator.ElementsName = "Заказов";
      this.Paginator.Changed += RefreshOrders;
    }

    private void RefreshOrders(int page, int elements)
    {
      var client = this.Client;
      if (client != null)
        this.OrderDataGrid.Orders = Model.Orders.GetForClient(client, page * elements, elements);
    }

    protected override void OnActivate()
    {
      base.OnActivate();
      RefreshOrders(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public PaginatorViewModel Paginator { get; set; }


    public void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public void AddOrder(object sender, RoutedEventArgs e)
    {
      ChangeApplicationScreen(Screens.OrderEditor);
      this.EventAggregator.BeginPublishOnUIThread(this.Client);
    }


    public void Handle(Client client)
    {
      this.Client = this.Model.Clients.GetById(client.Id);
      this.IsNew = false;
      
      if (this.Client != null) Paginator.Count = Model.Orders.GetForClientCount(this.Client);

      RefreshOrders(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
      this.EventAggregator.Unsubscribe(this);
    }

    public void ApplyChanges()
    {
      if (IsNew)
      {
        Model.Clients.Add(this.Client);
      }
      else
      {
        Model.Clients.Update(this.Client);
      }

      ChangeApplicationScreen(Screens.Context);
    }
  }
}