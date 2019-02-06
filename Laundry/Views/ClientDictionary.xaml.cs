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
  /// Interaction logic for ClientDictionary.xaml
  /// </summary>
  public partial class ClientDictionary : UserControl
  {
    public ObservableCollection<Client> Clients { get; set; }

    public ClientDictionary()
    {
      InitializeComponent();

      MenuToggleButton.Checked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = true;

      MenuToggleButton.Unchecked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = false;

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
    }

    private void OnAddClientButtonClick(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.ChangeView(new ClientEditor(this, null));
    }

    private void OnClientInfoGridButtonClick(object sender, RoutedEventArgs e)
    {
    }
  }
}