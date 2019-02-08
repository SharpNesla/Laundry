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
  /// Interaction logic for OrderEditor.xaml
  /// </summary>
  public partial class OrderEditor : UserControl
  {
    private UserControl _context;
    public DateTime DateTime { get; set; }

    public OrderEditor(UserControl context)
    {
      InitializeComponent();
      this._context = context;

      this.DataContext = this;
      Clients = new ObservableCollection<Client>(
        new[]
        {
          new Client("Антрипотийединиколей", "Карлов", "Иванович"),
          new Client("Андрей", "Rjrjh", "Иванович"),
          new Client("Андрей", "Карлов", "Иванович"),
          new Client("Андрей", "Карлов", "Иванович"),
          new Client("Андрей", "Карлов", "Иванович"),
        }
      );

      var kind = new ClothKind {MeasureKind = MeasureKind.Kg, Name = "Носки"};

      this.ClothInstances = new ObservableCollection<ClothInstance>(
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

    public ObservableCollection<Client> Clients { get; set; }

    public ObservableCollection<ClothInstance> ClothInstances { get; set; }

    private void OnDisableButtonClick(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.ChangeView(_context);
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
    }
  }
}