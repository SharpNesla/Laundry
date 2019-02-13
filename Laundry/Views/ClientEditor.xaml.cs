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
using Laundry.Model;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClientEditor.xaml
  /// </summary>
  public partial class ClientEditor : UserControl
  {
    private UserControl _context;
    public Client Client { get; set; }

    public ClientEditor(UserControl context, Client client)
    {
      InitializeComponent();
      this.DataContext = this;

      var kind = new ClothKind { MeasureKind = MeasureKind.Kg, Name = "Носки" };

      
      this._context = context;
      this.Client = new Client("Андрей", "Карлов", "Иванович");

      this.Client.Orders.Add(new Order(){});

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

    private void OnDisableButtonClick(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.ChangeView(_context);
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
      
    }
  }
}