using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using Laundry.Model;
using Laundry.Utils;
using MaterialDesignThemes.Wpf;
using ICollectionView = Laundry.Utils.ICollectionView;

namespace Laundry.Views
{
  public class ClientDictionaryViewModel : DrawerActivityScreen
  {
    private readonly ClientCard _card;
    public ICollectionView Clients { get; set; }

    public void AddClient()
    {
      ChangeApplicationScreen(Screens.ClientEditor);
    }

    public async void ShowClientInfo(Client context)
    {
      _card.Client = context;
      await DialogHost.Show(_card);
      
    }

    public int Count { get; set; } = 5;

    public ClientDictionaryViewModel(IEventAggregator aggregator, IModel model, ClientCard card) : base(aggregator,
      model)
    {
      this.Clients = new ICollectionView(this.Model.Clients, 2);
      this._card = card;
      this.Clients.Filter = ClientsFilter;
    }

    private bool ClientsFilter(object p)
    {
      var client = p as Client;
      return client.DateBirth == default(DateTime);
    }
  }
}