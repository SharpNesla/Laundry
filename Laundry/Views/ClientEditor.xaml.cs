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

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClientEditor.xaml
  /// </summary>
  public class ClientEditorViewModel : ActivityScreen, IHandle<Client>
  {
    public Client Client { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateBirth { get; set; }
    public ObservableCollection<Order> Orders { get; set; }
    public string PhoneNumber { get; set; }
    public int House { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }
    public bool IsPremiumClient { get; set; }
    public string Comment { get; set; }

    public ClientEditorViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
      this.EventAggregator.Subscribe(this);
    }

    public void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    public void AddOrder(object sender, RoutedEventArgs e)
    {
      ChangeApplicationScreen(Screens.OrderEditor);
      this.EventAggregator.BeginPublishOnUIThread(this.Model.AddOrder());
    }


    public void Handle(Client message)
    {
      this.Client = message;
      this.EventAggregator.Unsubscribe(this);
      CopyClientInfo();
    }

    void CopyClientInfo()
    {
      if (Client != null)
      {
        this.Name = Client.Name;
        this.Surname = Client.Surname;
        this.Patronymic = Client.Patronymic;
        this.DateBirth = Client.DateBirth;
        this.Orders = Client.Orders;
        this.PhoneNumber = Client.PhoneNumber;
        this.House = Client.House;
        this.Street = Client.Street;
        this.City = Client.City;
        this.ZipCode = Client.ZipCode;
        this.IsPremiumClient = Client.IsPremiumClient;
        this.Comment = Client.Comment;
      }
    }

    public void ApplyChanges()
    {
      if (Client == null)
      {
        Client = Model.AddClient();
      }

      Client.Name = this.Name;
      Client.Surname = this.Surname;
      Client.Patronymic = this.Patronymic;
      Client.DateBirth = this.DateBirth;
      Client.Orders = this.Orders;
      Client.PhoneNumber = this.PhoneNumber;
      Client.House = this.House;
      Client.Street = this.Street;
      Client.City = this.City;
      Client.ZipCode = this.ZipCode;
      Client.IsPremiumClient = this.IsPremiumClient;
      Client.Comment = this.Comment;

      ChangeApplicationScreen(Screens.Context);
    }
  }
}