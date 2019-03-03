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

    public ClientEditorViewModel(IEventAggregator aggregator, IModel model) : base(aggregator, model)
    {
      this.EventAggregator.Subscribe(this);

      var kind = new ClothKind { MeasureKind = MeasureKind.Kg, Name = "Носки" };


      this.Client = new Client("Андрей", "Карлов", "Иванович");

      this.Client.Orders.Add(new Order() { });

      this.Client.Orders[0].ClothInstances = new ObservableCollection<ClothInstance>(
        new[]
        {
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
          new ClothInstance {Amount = 3, Kind = kind, WearPercentage = 0},
        }
      );

    }

    public void Discard()
    {
      ChangeApplicationScreen(Screens.Context);
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
      
    }

    public void AddOrder(object sender, RoutedEventArgs e)
    {
      ChangeApplicationScreen(Screens.OrderEditor);
    }


    public void Handle(Client message)
    {
      this.Client = message;
      this.EventAggregator.Unsubscribe(this);
    }
  }
}