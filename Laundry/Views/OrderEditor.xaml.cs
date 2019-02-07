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
    }

    public ObservableCollection<Client> Clients { get; set; }

    private void OnDisableButtonClick(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.ChangeView(_context);
    }

    private void OnApplyButtonClick(object sender, RoutedEventArgs e)
    {
    }
  }
}