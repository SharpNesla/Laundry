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
using Laundry.Utils;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for ClientDictionary.xaml
  /// </summary>
  public partial class ClientDictionary : ActivityControl
  {
    public ObservableCollection<Client> Clients { get; set; }

    public ClientDictionary() : base(null)
    {
      InitializeComponent();
      
      //BindMainDrawerButton(MenuToggleButton);

      this.DataContext = this;

      /*MenuToggleButton.Checked +=
        (o, args) => (Application.Current as App).ShellView.DrawerHost.IsLeftDrawerOpen = true;

      MenuToggleButton.Unchecked +=
        (o, args) => (Application.Current as App).ShellView.DrawerHost.IsLeftDrawerOpen = false;*/

      Clients = new ObservableCollection<Client>(
        new[]
        {
          new Client("��������������������", "������", "��������"),
          new Client("������", "Rjrjh", "��������"),
          new Client("������", "������", "��������"),
          new Client("������", "������", "��������"),
          new Client("������", "������", "��������"),
        }
      );
    }

    private void OnAddClientButtonClick(object sender, RoutedEventArgs e)
    {
      //App.CurrentWindow.ChangeView(new ClientEditor(this, null));
    }

    private void OnClientInfoGridButtonClick(object sender, RoutedEventArgs e)
    {
    }

    private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
    {
    }
  }
}