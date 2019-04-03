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
    public ClientCardViewModel ClientCard { get; set; }


    public IList<Client> Clients { get; set; }
    public Client SelectedClient { get; set; }

    public void AddClient()
    {
      ChangeApplicationScreen(Screens.ClientEditor);
    }

    public void ShowClientInfo(Client context)
    {
     
      //Ищем View для ViewModel карточки клиента (Caliburn)
      var view = ViewLocator.LocateForModel(ClientCard, null, null);
      ViewModelBinder.Bind(this.ClientCard, view, null);

      this.EventAggregator.PublishOnUIThread(context);

      DialogHost.Show(view);
    }

    public int Count { get; set; } = 5;

    public ClientDictionaryViewModel(IEventAggregator aggregator, IModel model, ClientCardViewModel cardViewModel,
      PaginatorViewModel paginator, IWindowManager wm) : base(aggregator,
      model)
    {
      this.ClientCard = cardViewModel;

      this.Paginator = paginator;
      this.Paginator.ElementsName = "Заказов";

      this.Paginator.Changed += RefreshClients;
    }

    private void RefreshClients(int page, int elements)
    {
      this.Clients = Model.Clients.Get(page * elements, elements);
    }

    public PaginatorViewModel Paginator { get; set; }

    protected override void OnActivate()
    {
      base.OnActivate();
      Paginator.Count = Model.Clients.GetCount();
      RefreshClients(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public void RemoveClient()
    {
      Model.Clients.Remove(SelectedClient);
      this.Paginator.Count = Clients.Count;
      RefreshClients(this.Paginator.CurrentPage - 1, this.Paginator.ElementsPerPage);
    }

    public void EditClient()
    {
      ChangeApplicationScreen(Screens.ClientEditor);
      EventAggregator.PublishOnUIThread(this.SelectedClient);
    }
  }
}