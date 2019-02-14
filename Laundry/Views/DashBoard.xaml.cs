using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace Laundry.Views
{
  /// <summary>
  /// Interaction logic for DashBoard.xaml
  /// </summary>
  public partial class DashBoard : UserControl
  {
    public DashBoard()
    {
      InitializeComponent();

      MenuToggleButton.Checked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = true;

      MenuToggleButton.Unchecked +=
        (o, args) => (Application.Current as App).MainWindow.DrawerHost.IsLeftDrawerOpen = false;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      App.CurrentWindow.ChangeView(new OrderEditor(this));
    }
  }
}