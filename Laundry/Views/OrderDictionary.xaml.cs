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
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  public class OrderDictionaryViewModel : DrawerActivityScreen
  {
    private ClientCard _card;
    public BindableCollection<Client> Clients { get; set; }

    public bool IsSearchDrawerOpened { get; set; }

    public void AddClient(object sender, RoutedEventArgs e)
    {
      ChangeApplicationScreen(Screens.ClientEditor);
    }

    public async void ShowClientInfo(Client context)
    {
      _card.Client = context;
      await DialogHost.Show(_card);
    }

    public OrderDictionaryViewModel(IEventAggregator aggregator, IModel model, ClientCard card) : base(aggregator, model)
    {
      this.Clients = new BindableCollection<Client>(model.Clients);
      this._card = card;
    }

    public void ChangeSearchDrawerState()
    {
      IsSearchDrawerOpened = !IsSearchDrawerOpened;
    }
  }
}