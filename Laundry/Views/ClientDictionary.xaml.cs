using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using Laundry.Utils.Controls;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  public class ClientDictionaryViewModel : DrawerActivityScreen
  {
    private readonly ClientCard _card;


    public IList<Client> Clients { get; set; }
    public Client SelectedClient { get; set; }

    public void AddClient()
    {
      ChangeApplicationScreen(Screens.ClientEditor);
    }

    public async void ShowClientInfo(Client context)
    {
      this.EventAggregator.PublishOnUIThread(context);
      await DialogHost.Show(_card);
    }

    public int Count { get; set; } = 5;

    public ClientDictionaryViewModel(IEventAggregator aggregator, IModel model, ClientCard card,
      PaginatorViewModel paginator) : base(aggregator,
      model)
    {
      this._card = card;

      this.Paginator = paginator;
      this.Paginator.ElementsName = "Заказов";

      this.Paginator.Changed += RefreshClients;
    }

    private void RefreshClients(int page, int elements)
    {
      this.Clients = Model.GetClients(page * elements, elements);
    }

    public PaginatorViewModel Paginator { get; set; }

    protected override void OnActivate()
    {
      base.OnActivate();
      Paginator.Count = Model.GetClientsCount();
      RefreshClients(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public void RemoveClient()
    {
      this.Paginator.Count = Clients.Count;
      Model.RemoveClient(SelectedClient);
      RefreshClients(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public void EditClient()
    {
      ChangeApplicationScreen(Screens.ClientEditor);
      EventAggregator.PublishOnUIThread(this.SelectedClient);
    }
  }
}